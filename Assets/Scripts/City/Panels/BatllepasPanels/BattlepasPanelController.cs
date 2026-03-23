using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.Widgets.Particles;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Network.DataServer;
using Network.DataServer.Messages.Battlepases;
using UIController.Rewards;
using UiExtensions.Misc;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasPanelController : UiPanelController<BattlepasPanelView>
    {
        private const string BATTLEPAS_NAME = "BattlepasRewards";

        private const int REWARD_COUNT = 30;

        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] protected readonly ClientRewardService ClientRewardService;

        private List<RewardModel> m_listRewards;
        private readonly List<BattlepasRewardView> m_rewardsUi = new(REWARD_COUNT);

        public override void Start()
        {
            View.MainSliderController.OnFillReward.Subscribe(OnFillReward).AddTo(Disposables);
            
#if UNITY_EDITOR
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Z))
                .Subscribe(_ => AddResource())
                .AddTo(Disposables);
#endif
            base.Start();
        }
        
#if UNITY_EDITOR
        private void AddResource()
        {
            Debug.Log("cheat add resource");
            m_resourceStorageController.AddResource(new GameResource(ResourceType.EventAgentMonet, 100));
        }
#endif
        
        private void OnFillReward(int rewardIndexOpen)
        {
            m_rewardsUi[rewardIndexOpen - 1].OpenWithAnimation();
        }

        protected override void OnLoadGame()
        {
            var currentMonetResource = m_resourceStorageController.Resources[ResourceType.EventAgentMonet];
            View.MainSliderController.SetValue(currentMonetResource.ConvertToInt(),
                m_commonDictionaries.RewardContainerModels[BATTLEPAS_NAME].Rewards.Count * 100);

            var index = 0;
            foreach (var rewardModel in m_commonDictionaries.RewardContainerModels[BATTLEPAS_NAME].Rewards)
            {
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.BattlepasRewardViewPrefab, View.Content);

                var data = new GameBattlepasReward(rewardModel);
                rewardViewPrefab.SetData(data, View.Scroll, m_commonDictionaries);
                rewardViewPrefab.OnSelect.Subscribe(OnTryGetReward).AddTo(Disposables);

                var status = ScrollableViewStatus.Close;
                if (index < CommonGameData.BattlepasData.CurrentDailyBattlepasStage)
                    status = ScrollableViewStatus.Completed;
                else if (index == CommonGameData.BattlepasData.CurrentDailyBattlepasStage)
                    status = ScrollableViewStatus.Open;


                Resolver.Inject(rewardViewPrefab);
                rewardViewPrefab.SetStatus(status);
                m_rewardsUi.Add(rewardViewPrefab);
            }

            View.MainSliderController.transform.SetAsLastSibling();
        }

        private void OnTryGetReward(ScrollableUiView<GameBattlepasReward> rewardView)
        {
            var previousIndex = CommonGameData.BattlepasData.CurrentDailyBattlepasStage;
            var index = rewardView.transform.GetSiblingIndex();
            if ((index > previousIndex + 1) || (index <= previousIndex))
                return;

            var currentMonet = m_resourceStorageController.Resources[ResourceType.EventAgentMonet].ConvertToInt();
            if (currentMonet >= (index + 1) * 100)
                GetReward(rewardView).Forget();
        }

        private async UniTaskVoid GetReward(ScrollableUiView<GameBattlepasReward> rewardView)
        {
            var message = new GetBattlepasRewardMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(rewardView.GetData.RewardModel, m_commonDictionaries);
                ClientRewardService.ShowReward(reward);
                CommonGameData.BattlepasData.CurrentDailyBattlepasStage += 1;
                rewardView.SetStatus(ScrollableViewStatus.Completed);
                var cost = new GameResource(ResourceType.EventAgentMonet, 100);
                m_resourceStorageController.SubtractResource(cost);
            }
        }

        public override void OnShow()
        {
            var currentMonetResource = m_resourceStorageController.Resources[ResourceType.EventAgentMonet];
            OnGetMonet(currentMonetResource);
            base.OnShow();
        }

        private void OnGetMonet(GameResource res)
        {
            res.ConvertToInt();
            View.MainSliderController.SetNewValueWithAnim(res.ConvertToInt());
        }
    }
}