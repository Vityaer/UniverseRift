using Db.CommonDictionaries;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UIController.Rewards;

namespace Models.Guilds
{
    public class GuildBossMission
    {
        private CommonDictionaries _commonDictionaries;

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20, CustomAddFunction = nameof(AddBoss))]
        public List<BossModel> BossModels = new();

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20, CustomAddFunction = nameof(AddReward))]
        public List<RewardModel> RewardModels = new();

        public void SetCommonDictionary(CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
        }

        private void AddBoss()
        {
            var boss = new BossModel();
            boss.CommonDictionaries = _commonDictionaries;
            BossModels.Add(boss);
        }

        private void AddReward()
        {
            var reward = new RewardModel();
            reward.CommonDictionaries = _commonDictionaries;
            RewardModels.Add(reward);
        }
    }
}
