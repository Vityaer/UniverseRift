using System;
using City.Buildings.Abstractions;
using City.Panels.Inventories;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using LocalizationSystems;
using Models.City.Alchemies;
using Models.City.Markets;
using Models.Common;
using Models.Data.Inventories;
using Network.DataServer;
using Network.DataServer.Messages.City;
using Network.DataServer.Messages.City.Alchemies;
using Services.TimeLocalizeServices;
using UIController.Buttons;
using UIController.Inventory;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace UIController.ControllerPanels.AlchemyPanels
{
    public class AlchemyPanelController : BaseBuilding<AlchemyPanelView>
    {
        private readonly TimeSpan m_alchemyTimespan = TimeSpan.FromHours(8);

        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly TimeLocalizeService m_timeLocalizeService;
        [Inject] private readonly CommonGameData m_сommonGameData;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly InventoryPanelController m_inventoryPanelController;

        private AlchemyPanelBuildingModel m_alchemyPanelBuildingModel;
        private DateTime m_lastGetAlchemyDateTime;

        public readonly ReactiveCommand OnGetAchemyGold = new();

        protected override void OnStart()
        {
            View.AlchemyButton.OnClickAsObservable().Subscribe(_ => GetAlchemyGold().Forget()).AddTo(Disposables);
            View.SliderTimeAlchemy.Init(m_localizationSystem, m_timeLocalizeService);
            View.SliderTimeAlchemy.OnTimerFinish.Subscribe(_ => FinishAlchemySlider()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            m_lastGetAlchemyDateTime = TimeUtils.ParseTime(CommonGameData.CycleEventsData.LastGetAlchemyDateTime);
            CheckAlchemyButton();

            try
            {
                var reward = new GameReward(m_commonDictionaries.Rewards["Alchemy"], m_commonDictionaries);
                View.RewardView.ShowReward(reward);
                ShowProducts();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }

        }

        private void ShowProducts()
        {
            if (View.ProductButtons == null)
            {
                Debug.LogError("View.ProductButtons is null");
                return;
            }
            
            m_alchemyPanelBuildingModel = m_commonDictionaries.Buildings[nameof(AlchemyPanelBuildingModel)]
                as AlchemyPanelBuildingModel;

            if (m_alchemyPanelBuildingModel == null)
            {
                return;
            }

            for(int i = 0; i < m_alchemyPanelBuildingModel.Products.Count && i < View.ProductButtons.Count; i++)
            {
                string productName = m_alchemyPanelBuildingModel.Products[i];
                if (!m_commonDictionaries.Products.TryGetValue(productName, out BaseProductModel basProductModel))
                {
                    continue;
                }
                ResourceProductModel productModel = basProductModel as ResourceProductModel;
                if (productModel == null)
                {
                    Debug.LogError(productName);
                    continue;
                }

                GameResource cost = new GameResource(productModel.Cost);
                var view = View.ProductButtons[i];
                view.ChangeCost(cost, () => BuyProduct(view, productModel));
                var rewardModel = new RewardModel();
                
                var resourceData = productModel.Subject as ResourceData;
                if (resourceData == null)
                {
                    Debug.LogError(productModel.Subject);
                    continue;
                }

                rewardModel.Resources.Add(resourceData);
                View.ProductsRewards[i].ShowReward(new GameReward(rewardModel, m_commonDictionaries));
            }
        }

        private void BuyProduct(ButtonWithObserverResource viewProductButton, BaseProductModel productModel)
        {
            TryBuyProductOnServer(productModel.Id, viewProductButton).Forget();
        }

        private void CheckAlchemyButton()
        {
            var delta = DateTime.UtcNow - m_lastGetAlchemyDateTime;
            View.SliderTimeAlchemy.SetData(m_lastGetAlchemyDateTime, m_alchemyTimespan);
            View.AlchemyButton.interactable = delta >= m_alchemyTimespan;
        }

        private void FinishAlchemySlider()
        {
            View.AlchemyButton.interactable = true;
        }

        private async UniTaskVoid GetAlchemyGold()
        {
            View.AlchemyButton.interactable = false;
            var message = new GetAlchemyMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(m_commonDictionaries.Rewards["Alchemy"], m_commonDictionaries);
                m_clientRewardService.ShowReward(reward);
                m_lastGetAlchemyDateTime = DateTime.UtcNow;
                OnGetAchemyGold.Execute();
            }

            CheckAlchemyButton();
        }
        
        private async UniTaskVoid TryBuyProductOnServer(string productId, ButtonWithObserverResource view)
        {
            var message = new BuyProductMessage { PlayerId = m_сommonGameData.PlayerInfoData.Id, ProductId = productId, Count = 1 };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var product = m_commonDictionaries.Products[productId];
                var cost = new GameResource(product.Cost);
                m_resourceStorageController.SubtractResource(cost);

                switch (product)
                {
                    case ResourceProductModel resourceProduct:
                        var resource = new GameResource(resourceProduct.Subject);
                        m_resourceStorageController.AddResource(resource);
                        break;
                    case ItemProductModel itemProduct:
                        var itemModel = m_commonDictionaries.Items[itemProduct.Subject.Id];
                        var item = new GameItem(itemModel, itemProduct.Subject.Amount);
                        m_inventoryPanelController.Add(item);
                        break;
                }
            }
        }
    }
}