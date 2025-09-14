using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.Widgets.Particles;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Network.DataServer;
using Network.DataServer.Messages.Battlepases;
using UIController.Rewards;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasPanelController : UiPanelController<BattlepasPanelView>
    {
        private const string BATTLEPAS_NAME = "BattlepasRewards";

        private const int REWARD_COUNT = 30;

        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] protected readonly ClientRewardService _clientRewardService;

        private int _currentProgress = 0;
        private List<RewardModel> _listRewards;
        private List<int> _idReceivedReward = new List<int>(REWARD_COUNT);
        private List<BattlepasRewardView> _rewardsUi = new(REWARD_COUNT);

        public override void Start()
        {
            View.MainSliderController.OnFillReward.Subscribe(OnFillReward).AddTo(Disposables);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Z))
                .Subscribe(_ => AddResource())
                .AddTo(Disposables);
            base.Start();
        }

        private void AddResource()
        {
            Debug.Log("cheat add resource");
            _resourceStorageController.AddResource(new GameResource(ResourceType.EventAgentMonet, 100));
        }

        private void OnFillReward(int rewardIndexOpen)
        {
            _rewardsUi[rewardIndexOpen - 1].OpenWithAnimation();
        }

        protected override void OnLoadGame()
        {
            var currentMonetResource = _resourceStorageController.Resources[ResourceType.EventAgentMonet];
            View.MainSliderController.SetValue(currentMonetResource.ConvertToInt(),
                _commonDictionaries.RewardContainerModels[BATTLEPAS_NAME].Rewards.Count * 100);

            var index = 0;
            foreach (var rewardModel in _commonDictionaries.RewardContainerModels[BATTLEPAS_NAME].Rewards)
            {
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.BattlepasRewardViewPrefab, View.Content);

                var data = new GameBattlepasReward(rewardModel);
                rewardViewPrefab.SetData(data, View.Scroll);
                rewardViewPrefab.OnSelect.Subscribe(OnTryGetReward).AddTo(Disposables);

                var status = ScrollableViewStatus.Close;
                if (index < CommonGameData.BattlepasData.CurrentDailyBattlepasStage)
                    status = ScrollableViewStatus.Completed;
                else if (index == CommonGameData.BattlepasData.CurrentDailyBattlepasStage)
                    status = ScrollableViewStatus.Open;


                Resolver.Inject(rewardViewPrefab);
                rewardViewPrefab.SetStatus(status);
                _rewardsUi.Add(rewardViewPrefab);
            }

            View.MainSliderController.transform.SetAsLastSibling();
        }

        private void OnTryGetReward(ScrollableUiView<GameBattlepasReward> rewardView)
        {
            var previousIndex = CommonGameData.BattlepasData.CurrentDailyBattlepasStage;
            var index = rewardView.transform.GetSiblingIndex();
            if ((index > previousIndex + 1) || (index <= previousIndex))
                return;

            var currentMonet = _resourceStorageController.Resources[ResourceType.EventAgentMonet].ConvertToInt();
            if (currentMonet >= (index + 1) * 100)
                GetReward(rewardView).Forget();
        }

        private async UniTaskVoid GetReward(ScrollableUiView<GameBattlepasReward> rewardView)
        {
            var message = new GetBattlepasRewardMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(rewardView.GetData.RewardModel, _commonDictionaries);
                _clientRewardService.ShowReward(reward);
                CommonGameData.BattlepasData.CurrentDailyBattlepasStage += 1;
                rewardView.SetStatus(ScrollableViewStatus.Completed);
                var cost = new GameResource(ResourceType.EventAgentMonet, 100);
                _resourceStorageController.SubtractResource(cost);
            }
        }

        public override void OnShow()
        {
            var currentMonetResource = _resourceStorageController.Resources[ResourceType.EventAgentMonet];
            OnGetMonet(currentMonetResource);
            base.OnShow();
        }

        private void OnGetMonet(GameResource res)
        {
            _currentProgress = res.ConvertToInt();
            View.MainSliderController.SetNewValueWithAnim(res.ConvertToInt());
        }
    }
}