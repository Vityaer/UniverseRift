using Assets.Scripts.ClientServices;
using City.Buildings.Abstractions;
using ClientServices;
using Common;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.City.Markets;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City;
using System.Collections.Generic;
using UIController.Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Market
{
    public class MarketController : BaseBuilding<MarketView>, IInitializable
    {
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly InventoryController _inventoryController;

        public CycleRecover cycle;

        private List<MarketProductController> productControllers = new List<MarketProductController>();

        public void Initialize()
        {
            var market = _commonDictionaries.Markets["CityMarket"];
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
                }

                var controller = Object.Instantiate(View.Prefab, View.Content);
                controller.SetData(productId, baseMarketProduct, () => TryBuyProductOnServer(productId, 1).Forget());
                productControllers.Add(controller);
                _resolver.Inject(controller.ButtonCost);
            }
        }

        protected override void OnLoadGame()
        {
            var marketData = _сommonGameData.City.MallSave;
            foreach (var purchase in marketData.PurchaseDatas)
            {
                var controller = productControllers.Find(controller => controller.SubjectId == purchase.ProductId);
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

        private void UpdateUIProducts()
        {
            foreach (var product in productControllers)
            {
                product.UpdateUI();
            }
        }
    }
}