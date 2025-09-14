using System;
using City.Buildings.Abstractions;
using City.Buildings.Market;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.City.Markets;
using Models.Common;
using Network.DataServer.Messages.City;
using Network.DataServer;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.Misc.Widgets;
using UnityEngine;
using VContainer;
using Common.Inventories.Splinters;
using System.Linq;
using City.Panels.Inventories;
using Object = UnityEngine.Object;

namespace UiExtensions.Panels
{
    public abstract class BaseMarketController<T> : BaseBuilding<T>
        where T : BaseMarketView
    {
        [Inject] protected readonly CommonGameData _сommonGameData;
        [Inject] protected readonly CommonDictionaries _commonDictionaries;
        [Inject] protected readonly ResourceStorageController _resourceStorageController;
        [Inject] protected readonly InventoryPanelController InventoryPanelController;

        protected List<MarketProductController> productControllers = new();

        protected abstract string MarketContainerName { get; }

        protected void CreateProducts()
        {
            var market = _commonDictionaries.Markets[MarketContainerName];
            var allMarketProducts = new List<string>(market.Products.Count);
            allMarketProducts.AddRange(market.Products);

            var promotions = _сommonGameData.City.MallSave.ShopPromotions
                .FindAll(promo => promo.MarketName.Equals(MarketContainerName))
                .Select(promo => promo.ProductId)
                .ToList();

            foreach (var promo in promotions)
            {
                if (!_commonDictionaries.Products.ContainsKey(promo))
                {
                    Debug.LogError($"Market: {Name}, promotion {promo} not found");
                    continue;
                }

                allMarketProducts.Add(promo);
            }

            foreach (var productId in allMarketProducts)
            {
                try
                {

                    var productModel = _commonDictionaries.Products[productId];

                    BaseMarketProduct baseMarketProduct = null;

                    switch (productModel)
                    {
                        case ResourceProductModel resourceProductModel:
                            var resource = new GameResource(resourceProductModel.Subject);
                            baseMarketProduct = new MarketProduct<GameResource>(resourceProductModel, resource);
                            break;
                        case ItemProductModel itemProductModel:
                            var itemModel = _commonDictionaries.Items[itemProductModel.Subject.Id];
                            var item = new GameItem(itemModel, itemProductModel.Subject.Amount);
                            baseMarketProduct = new MarketProduct<GameItem>(itemProductModel, item);
                            break;
                        case SplinterProductModel splinterProductModel:
                            var splinterModel = _commonDictionaries.Splinters[splinterProductModel.Subject.Id];
                            var splinter = new GameSplinter(splinterModel, splinterProductModel.Subject.Amount);
                            baseMarketProduct = new MarketProduct<GameSplinter>(splinterProductModel, splinter);
                            break;
                    }



                var controller = Object.Instantiate(View.Prefab, View.ScrollRect.content);
                productControllers.Add(controller);
                Resolver.Inject(controller);
                Resolver.Inject(controller.ButtonCost);
                controller.SetData(productId, baseMarketProduct, () => TryBuyProductOnServer(productId, 1).Forget());
                controller.SetScroll(View.ScrollRect);
                }
                catch(Exception ex)
                {
                    Debug.LogError($"Failed to load product: {productId} in market {Name} exception: {ex.Message}");
                }
            }
        }

        protected override void OnLoadGame()
        {
            var marketData = _сommonGameData.City.MallSave;
            CreateProducts();

            foreach (var controller in productControllers)
            {
                var purchase = marketData.PurchaseDatas.Find(purchase => controller.SubjectId == purchase.ProductId);
                if (purchase == null)
                    continue;

                controller.SetPurchaseCount(purchase.PurchaseCount);
            }
        }

        private async UniTaskVoid TryBuyProductOnServer(string productId, int coutSell)
        {
            var message = new BuyProductMessage { PlayerId = _сommonGameData.PlayerInfoData.Id, ProductId = productId, Count = coutSell };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var product = _commonDictionaries.Products[productId];
                var cost = new GameResource(product.Cost);
                _resourceStorageController.SubtractResource(cost);

                switch (product)
                {
                    case ResourceProductModel ResourceProduct:
                        var resource = new GameResource(ResourceProduct.Subject);
                        _resourceStorageController.AddResource(resource);
                        break;
                    case ItemProductModel ItemProduct:
                        var itemModel = _commonDictionaries.Items[ItemProduct.Subject.Id];
                        var item = new GameItem(itemModel, ItemProduct.Subject.Amount);
                        InventoryPanelController.Add(item);
                        break;
                }

                var controller = productControllers.Find(controller => controller.SubjectId == productId);
                controller.FinishBuy();
            }

        }
    }
}
