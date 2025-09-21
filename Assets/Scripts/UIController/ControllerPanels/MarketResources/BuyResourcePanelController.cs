using System.Collections.Generic;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Models.City.Markets;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City;
using UIController.ControllerPanels.AlchemyPanels;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace UIController.ControllerPanels.MarketResources
{
    public class BuyResourcePanelController : UiPanelController<BuyResourcePanelView>
    {
        [Inject] private readonly ResourceStorageController m_storageController;
        [Inject] private readonly IObjectResolver m_resolver;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly AlchemyPanelController m_alchemyPanelController;

        private readonly Dictionary<ResourceType, ResourceProductContainer> m_resources = new();
        private GameResource m_resourceForSell;
        private GameResource m_cost;
        private ResourceProductContainer m_currentProduct;

        public override void Start()
        {
            base.Start();
            View.CountController.OnChangeCount.Subscribe(ChangeCount).AddTo(Disposables);
            m_resolver.Inject(View.ResourceObjectCost);
            View.BuyButton.OnClick += StartBuy;
        }

        public bool GetCanSellThisResource(ResourceType typeResource)
        {
            var result = typeResource switch
            {
                ResourceType.Gold => true,
                _ => m_resources.ContainsKey(typeResource)
            };
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

            var product = m_resources[typeResource];
            m_currentProduct = product;

            m_cost = new GameResource(product.ResourceProduct.Cost);

            m_resourceForSell = new GameResource(product.ResourceProduct.Subject);
            ChangeCount(count: View.CountController.MinCount);
            View.MainImage.SetData(m_resourceForSell);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<BuyResourcePanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            var market = m_commonDictionaries.Markets["ResourceMarket"];
            var marketData = m_commonGameData.City.MallSave;

            foreach (var productId in market.Products)
            {
                var productModel = m_commonDictionaries.Products[productId] as ResourceProductModel;
                if (productModel == null)
                    continue;

                var purchase = marketData.PurchaseDatas.Find(purchase => purchase.ProductId == productModel.Id);
                var countPurchase = purchase?.PurchaseCount ?? 0;

                var container = new ResourceProductContainer(productModel, countPurchase, productModel.CountSell);
                m_resources.Add(productModel.Subject.Type, container);
            }
        }

        private void ChangeCount(int count)
        {
            View.BuyButton.ChangeCost(m_cost * count);
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
                PlayerId = m_commonGameData.PlayerInfoData.Id,
                ProductId = m_currentProduct.ResourceProduct.Id,
                Count = count
            };

            var result = await DataServer.PostData(message);

            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            var cost = new GameResource(m_currentProduct.ResourceProduct.Cost) * count;
            m_resourceStorageController.SubtractResource(cost);
            m_storageController.AddResource(m_resourceForSell * count);
        }

        public override void Dispose()
        {
            View.BuyButton.OnClick -= StartBuy;
            base.Dispose();
        }

    }
}