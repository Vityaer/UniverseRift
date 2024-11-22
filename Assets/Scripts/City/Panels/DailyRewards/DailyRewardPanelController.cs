using City.Buildings.CityButtons.DailyReward;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.DailyRewards;
using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.DailyRewards;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Buildings.CityButtons
{
    public class DailyRewardPanelController : UiPanelController<DailyRewardPanelView>
    {
        private const string DAILY_REWARDS_CONTAIER_NAME = "DailyRewards";

        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly SubjectDetailController _subjectDetailController;

        private List<DailyRewardUI> _rewardControllers = new List<DailyRewardUI>();

        protected override void OnLoadGame()
        {
            var index = 0;
            foreach (var rewardModel in _commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAIER_NAME].Rewards)
            {
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.RewardPrefab, View.Content);

                var data = new GameReward(rewardModel, _commonDictionaries);
                Resolver.Inject(rewardViewPrefab);
                rewardViewPrefab.SetData(data, View.Scroll);
                var status = (index <= CommonGameData.City.DailyReward.CurrentDailyReward)
                    ?
                    ScrollableViewStatus.Completed
                    :
                    ScrollableViewStatus.Open;

                rewardViewPrefab.SetStatus(status);
                _rewardControllers.Add(rewardViewPrefab);
                rewardViewPrefab.RewardController.SetDetailsController(_subjectDetailController);
                index++;
            }

            if (CommonGameData.City.DailyReward.CanGetDailyReward)
            {
                TryGetNextDailyReward().Forget();
            }
        }

        private async UniTaskVoid TryGetNextDailyReward()
        {
            var message = new GetDailyRewardMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                CommonGameData.City.DailyReward.CurrentDailyReward += 1;
                var index = CommonGameData.City.DailyReward.CurrentDailyReward;
                var reward = new GameReward(_commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAIER_NAME].Rewards[index], _commonDictionaries);
                _clientRewardService.GetReward(reward);
                _rewardControllers[index].SetStatus(ScrollableViewStatus.Completed);
                CommonGameData.City.DailyReward.CanGetDailyReward = false;
            }
        }
    }
}