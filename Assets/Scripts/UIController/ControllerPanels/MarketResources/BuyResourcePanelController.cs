using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.City.Markets;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City;
using System;
using System.Collections.Generic;
using UIController.ControllerPanels.AlchemyPanels;
using UIController.ControllerPanels.MarketResources;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace UIController
{
    public class BuyResourcePanelController : UiPanelController<BuyResourcePanelView>, IStartable
    {
        [Inject] private readonly ResourceStorageController _storageController;
        [Inject] private readonly IObjectResolver _resolver;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly CommonGameData _ñommonGameData;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly AlchemyPanelController _alchemyPanelController;

        private Dictionary<ResourceType, ResourceProductContainer> _resources = new();
        private GameResource _resourceForSell;
        private GameResource _cost;
        private ResourceProductContainer _currentProduct;

        public override void Start()
        {
            base.Start();
            View.CountController.OnChangeCount.Subscribe(ChangeCount).AddTo(Disposables);
            _resolver.Inject(View.ResourceObjectCost);
            View.BuyButton.OnClick += StartBuy;
        }

        public bool GetCanSellThisResource(ResourceType typeResource)
        {
            var result = false;
            switch (typeResource)
            {
                case ResourceType.Gold:
                    result = true;
                    break;
                default:
                    result = _resources.ContainsKey(typeResource);
                    break;
            }
            return result;
        }

        public void Open(ResourceType typeResource)
        {
            switch (typeResource)
            {
                case ResourceType.Gold:
                    MessagesPublisher.OpenWindowPublisher.OpenWindow<AlchemyPanelController>(openType: OpenType.Exclusive);
                    return;
            }

            var product = _resources[typeResource];
            _currentProduct = product;

            _cost = new GameResource(product.ResourceProduct.Cost);

            _resourceForSell = new GameResource(product.ResourceProduct.Subject);
            ChangeCount(count: View.CountController.MinCount);
            View.MainImage.SetData(_resourceForSell);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<BuyResourcePanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            var market = _commonDictionaries.Markets["ResourceMarket"];
            var marketData = _ñommonGameData.City.MallSave;

            foreach (var productId in market.Products)
            {
                var productModel = _commonDictionaries.Products[productId] as ResourceProductModel;
                if (productModel == null)
                    continue;

                var purchase = marketData.PurchaseDatas.Find(purchase => purchase.ProductId == productModel.Id);
                var countPurchase = (purchase != null) ? purchase.PurchaseCount : 0;

                var container = new ResourceProductContainer(productModel, countPurchase, productModel.CountSell);
                _resources.Add(productModel.Subject.Type, container);
            }
        }

        private void ChangeCount(int count)
        {
            View.BuyButton.ChangeCost(_cost * count);
        }

        private void StartBuy()
        {
            Buy().Forget();
        }

        private async UniTaskVoid Buy()
        {
            int count = View.CountController.Count;

            var message = new BuyProductMessage
            {
                PlayerId = _ñommonGameData.PlayerInfoData.Id,
                ProductId = _currentProduct.ResourceProduct.Id,
                Count = count
            };

            var result = await DataServer.PostData(message);

            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            var cost = new GameResource(_currentProduct.ResourceProduct.Cost) * count;
            _resourceStorageController.SubtractResource(cost);
            _storageController.AddResource(_resourceForSell * count);
        }

        public override void Dispose()
        {
            View.BuyButton.OnClick -= StartBuy;
            base.Dispose();
        }

    }
}