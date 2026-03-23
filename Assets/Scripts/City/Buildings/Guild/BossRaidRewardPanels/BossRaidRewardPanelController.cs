using System.Collections.Generic;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using Models.Data.Rewards;
using Models.Guilds;
using UiExtensions.Panels;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.BossRaidRewardPanels
{
    public class BossRaidRewardPanelController : UiPanelController<BossRaidRewardPanelView>
    {
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;

        private GuildBossMission m_mission;
        private readonly List<BossRewardView> m_rewardControllers = new();

        public void Open(GuildBossMission mission)
        {
            TryFillRewardPanel(mission);
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<BossRaidRewardPanelController>(
                openType: OpenType.Additive);
        }

        private void TryFillRewardPanel(GuildBossMission mission)
        {
            if (m_mission != null && m_mission == mission) return;

            m_mission = mission;
            foreach (BossRewardView rewardController in m_rewardControllers)
                UnityEngine.Object.Destroy(rewardController.gameObject);

            m_rewardControllers.Clear();

            foreach (BossWinRewardModel bossRewardModel in m_mission.RewardModels)
            {
                GameReward data = new GameReward(bossRewardModel.Reward, m_commonDictionaries);
                BossRewardView rewardViewPrefab =
                    UnityEngine.Object.Instantiate(View.BossRewardView, View.RewardsScroll.content);
                rewardViewPrefab.SetData(data, bossRewardModel.StartIndex, bossRewardModel.EndIndex, View.RewardsScroll,
                    m_commonDictionaries);
                rewardViewPrefab.RewardController.SetDetailsController(m_subjectDetailController);
                m_rewardControllers.Add(rewardViewPrefab);
            }
        }
    }
}