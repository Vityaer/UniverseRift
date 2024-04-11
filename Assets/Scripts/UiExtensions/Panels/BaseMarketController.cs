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

namespace UiExtensions.Panels
{
    public abstract class BaseMarketController<T> : BaseBuilding<T>
        where T : BaseMarketView
    {
        [Inject] protected readonly CommonGameData _сommonGameData;
        [Inject] protected readonly CommonDictionaries _commonDictionaries;
        [Inject] protected readonly ResourceStorageController _resourceStorageController;
        [Inject] protected readonly InventoryController _inventoryController;

        protected List<MarketProductController> productControllers = new();

        protected abstract string MarketContainerName { get; }

        protected void CreateProducts()
        {
            var market = _commonDictionaries.Markets[MarketContainerName];
            foreach (var productId in market.Products)
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

                var controller = Object.Instantiate(View.Prefab, View.Content);
                productControllers.Add(controller);
                Resolver.Inject(controller.ButtonCost);
                controller.SetData(productId, baseMarketProduct, () => TryBuyProductOnServer(productId, 1).Forget());
            }
        }

        protected override void OnLoadGame()
        {
            CreateProducts();
            var marketData = _сommonGameData.City.MallSave;

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
                        _inventoryController.Add(item);
                        break;
                }

                var controller = productControllers.Find(controller => controller.SubjectId == productId);
                controller.FinishBuy();
            }

        }
    }
}
