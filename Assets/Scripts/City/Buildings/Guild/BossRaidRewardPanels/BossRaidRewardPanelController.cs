using System.Collections.Generic;
using City.Panels.SubjectPanels.Common;
using Common.Rewards;
using Db.CommonDictionaries;
using Models.Guilds;
using UiExtensions.Scroll.Interfaces;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Guild.BossRaidRewardPanels
{
    public class BossRaidRewardPanelController : UiPanelController<BossRaidRewardPanelView>
    {
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly SubjectDetailController _subjectDetailController;

        private GuildBossMission _mission;
        private List<BossRewardView> _rewardControllers = new();

        public void Open(GuildBossMission mission)
        {
            TryFillRewardPanel(mission);
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<BossRaidRewardPanelController>(openType: OpenType.Additive);
        }

        private void TryFillRewardPanel(GuildBossMission mission)
        {
            if(_mission != null && _mission == mission)
            {
                return;
            }
            
            _mission = mission;
            foreach (var rewardController in _rewardControllers)
            {
                UnityEngine.Object.Destroy(rewardController.gameObject);
            }
            
            _rewardControllers.Clear();
            
            foreach (var bossRewardModel in _mission.RewardModels)
            {
                var data = new GameReward(bossRewardModel.Reward, _commonDictionaries);
                var rewardViewPrefab = UnityEngine.Object.Instantiate(View.BossRewardView, View.RewardsScroll.content);
                rewardViewPrefab.SetData(data, bossRewardModel.StartIndex, bossRewardModel.EndIndex, View.RewardsScroll);
                rewardViewPrefab.RewardController.SetDetailsController(_subjectDetailController);
                _rewardControllers.Add(rewardViewPrefab);
            }
        }
    }
}