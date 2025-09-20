using Sirenix.OdinInspector;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Models.Data.Rewards;
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
        public List<BossWinRewardModel> RewardModels = new();

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
            RewardModels.Add(new BossWinRewardModel(reward));
        }

        public GuildBossMission Clone()
        {
            GuildBossMission result = new GuildBossMission();
            foreach (var bossModel in BossModels)
            {
                result.BossModels.Add(bossModel.Clone());
            }
            
            result.RewardModels = RewardModels;

            return result;
        }
    }
}
