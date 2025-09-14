using System.Collections.Generic;
using City.Buildings.CityButtons.DailyReward;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Network.DataServer;
using Network.DataServer.Messages.DailyRewards;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Panels.DailyRewards
{
    public class DailyRewardPanelController : UiPanelController<DailyRewardPanelView>
    {
        private const string DAILY_REWARDS_CONTAIER_NAME = "DailyRewards";

        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;

        private readonly List<DailyRewardUI> m_rewardControllers = new();

        protected override void OnLoadGame()
        {
            var index = 0;
            foreach (var rewardModel in m_commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAIER_NAME].Rewards)
            {
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.RewardPrefab, View.Content);

                var data = new GameReward(rewardModel, m_commonDictionaries);
                Resolver.Inject(rewardViewPrefab);
                rewardViewPrefab.SetData(data, View.Scroll);
                var status = (index <= CommonGameData.City.DailyReward.CurrentDailyReward)
                    ?
                    ScrollableViewStatus.Completed
                    :
                    ScrollableViewStatus.Open;

                rewardViewPrefab.SetStatus(status);
                m_rewardControllers.Add(rewardViewPrefab);
                rewardViewPrefab.RewardController.SetDetailsController(m_subjectDetailController);
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
                var reward = new GameReward(m_commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAIER_NAME].Rewards[index], m_commonDictionaries);
                m_clientRewardService.GetReward(reward);
                m_rewardControllers[index].SetStatus(ScrollableViewStatus.Completed);
                CommonGameData.City.DailyReward.CanGetDailyReward = false;
            }
        }
    }
}