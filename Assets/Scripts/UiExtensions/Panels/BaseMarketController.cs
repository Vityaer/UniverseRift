using System;
using City.Buildings.Abstractions;
using City.Buildings.Market;
using ClientServices;
using Cysharp.Threading.Tasks;
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
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Object = UnityEngine.Object;

namespace UiExtensions.Panels
{
    public abstract class BaseMarketController<T> : BaseBuilding<T>
        where T : BaseMarketView
    {
        [Inject] protected readonly CommonGameData СommonGameData;
        [Inject] protected readonly CommonDictionaries CommonDictionaries;
        [Inject] protected readonly ResourceStorageController ResourceStorageController;
        [Inject] protected readonly InventoryPanelController InventoryPanelController;

        protected readonly List<MarketProductController> ProductControllers = new();

        protected abstract string MarketContainerName { get; }

        private void CreateProducts()
        {
            var market = CommonDictionaries.Markets[MarketContainerName];
            var allMarketProducts = new List<string>(market.Products.Count);
            allMarketProducts.AddRange(market.Products);

            var promotions = СommonGameData.City.MallSave.ShopPromotions
                .FindAll(promo => promo.MarketName.Equals(MarketContainerName))
                .Select(promo => promo.ProductId)
                .ToList();

            foreach (var promo in promotions)
            {
                if (!CommonDictionaries.Products.ContainsKey(promo))
                {
                    Debug.LogError($"Market: {Name}, promotion {promo} not found");
                    continue;
                }

                allMarketProducts.Add(promo);
            }

            foreach (string productId in allMarketProducts)
            {
                try
                {

                    var productModel = CommonDictionaries.Products[productId];

                    BaseMarketProduct baseMarketProduct = null;

                    switch (productModel)
                    {
                        case ResourceProductModel resourceProductModel:
                            var resource = new GameResource(resourceProductModel.Subject);
                            baseMarketProduct = new MarketProduct<GameResource>(resourceProductModel, resource);
                            break;
                        case ItemProductModel itemProductModel:
                            var itemModel = CommonDictionaries.Items[itemProductModel.Subject.Id];
                            var item = new GameItem(itemModel, itemProductModel.Subject.Amount);
                            baseMarketProduct = new MarketProduct<GameItem>(itemProductModel, item);
                            break;
                        case SplinterProductModel splinterProductModel:
                            var splinterModel = CommonDictionaries.Splinters[splinterProductModel.Subject.Id];
                            var splinter = new GameSplinter(splinterModel, splinterProductModel.Subject.Amount);
                            baseMarketProduct = new MarketProduct<GameSplinter>(splinterProductModel, splinter);
                            break;
                    }



                var controller = Object.Instantiate(View.Prefab, View.ScrollRect.content);
                ProductControllers.Add(controller);
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
            var marketData = СommonGameData.City.MallSave;
            CreateProducts();

            foreach (var controller in ProductControllers)
            {
                var purchase = marketData.PurchaseDatas.Find(purchase => controller.SubjectId == purchase.ProductId);
                if (purchase == null)
                    continue;

                controller.SetPurchaseCount(purchase.PurchaseCount);
            }
        }

        private async UniTaskVoid TryBuyProductOnServer(string productId, int coutSell)
        {
            var message = new BuyProductMessage { PlayerId = СommonGameData.PlayerInfoData.Id, ProductId = productId, Count = coutSell };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var product = CommonDictionaries.Products[productId];
                var cost = new GameResource(product.Cost);
                ResourceStorageController.SubtractResource(cost);

                switch (product)
                {
                    case ResourceProductModel resourceProduct:
                        var resource = new GameResource(resourceProduct.Subject);
                        ResourceStorageController.AddResource(resource);
                        break;
                    case ItemProductModel itemProduct:
                        var itemModel = CommonDictionaries.Items[itemProduct.Subject.Id];
                        var item = new GameItem(itemModel, itemProduct.Subject.Amount);
                        InventoryPanelController.Add(item);
                        break;
                }

                var controller = ProductControllers.Find(controller => controller.SubjectId == productId);
                controller.FinishBuy();
            }
        }
    }
}
