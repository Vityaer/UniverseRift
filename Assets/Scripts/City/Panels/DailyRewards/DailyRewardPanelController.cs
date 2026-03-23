using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Network.DataServer;
using Network.DataServer.Messages.DailyRewards;
using UIController.Rewards;
using UiExtensions.Panels;
using VContainer;

namespace City.Panels.DailyRewards
{
    public class DailyRewardPanelController : UiPanelController<DailyRewardPanelView>
    {
        private const string DAILY_REWARDS_CONTAINER_NAME = "DailyRewards";

        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;

        private readonly List<DailyRewardUI> m_rewardControllers = new();

        protected override void OnLoadGame()
        {
            int index = 0;
            foreach (RewardModel rewardModel in m_commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAINER_NAME]
                         .Rewards)
            {
                DailyRewardUI rewardViewPrefab = UnityEngine.Object.Instantiate(View.RewardPrefab, View.Content);

                GameReward data = new GameReward(rewardModel, m_commonDictionaries);
                Resolver.Inject(rewardViewPrefab);
                rewardViewPrefab.SetData(data, View.Scroll, m_commonDictionaries);
                ScrollableViewStatus status = (index <= CommonGameData.City.DailyReward.CurrentDailyReward)
                    ? ScrollableViewStatus.Completed
                    : ScrollableViewStatus.Open;

                rewardViewPrefab.SetStatus(status);
                m_rewardControllers.Add(rewardViewPrefab);
                rewardViewPrefab.RewardController.SetDetailsController(m_subjectDetailController);
                index++;
            }

            if (CommonGameData.City.DailyReward.CanGetDailyReward) TryGetNextDailyReward().Forget();
        }

        private async UniTaskVoid TryGetNextDailyReward()
        {
            GetDailyRewardMessage message = new GetDailyRewardMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                CommonGameData.City.DailyReward.CurrentDailyReward += 1;
                int index = CommonGameData.City.DailyReward.CurrentDailyReward;
                GameReward reward =
                    new GameReward(
                        m_commonDictionaries.RewardContainerModels[DAILY_REWARDS_CONTAINER_NAME].Rewards[index],
                        m_commonDictionaries);
                m_clientRewardService.GetReward(reward);
                m_rewardControllers[index].SetStatus(ScrollableViewStatus.Completed);
                CommonGameData.City.DailyReward.CanGetDailyReward = false;
            }
        }
    }
}