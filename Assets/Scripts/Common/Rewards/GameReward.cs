using Db.CommonDictionaries;
using Models.Data.Rewards;
using System.Collections.Generic;
using System.Linq;
using UIController.Rewards;

namespace Common.Rewards
{
    public class GameReward
    {
        public List<BaseObject> Objects = new List<BaseObject>();

        public GameReward() { }
        
        public GameReward(RewardModel rewardData, CommonDictionaries commonDictionaries)
        {
            rewardData.Items.ForEach(x => x.CommonDictionaries = commonDictionaries);
            rewardData.Splinters.ForEach(x => x.CommonDictionaries = commonDictionaries);
            Objects = rewardData.Objects.Select(obj => obj.CreateGameObject()).ToList();
        }

        public GameReward(AutoRewardData autoReward, CommonDictionaries commonDictionaries)
        {
            autoReward.Items.ForEach(x => x.CommonDictionaries = commonDictionaries);
            autoReward.Splinters.ForEach(x => x.CommonDictionaries = commonDictionaries);
            Objects = autoReward.Objects.Select(obj => obj.CreateGameObject()).ToList();
        }
    }
}
