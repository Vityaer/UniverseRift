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
        
        public GameReward(RewardModel rewardData)
        {
            Objects = rewardData.Objects.Select(obj => obj.CreateGameObject()).ToList();
        }

        public GameReward(AutoRewardData autoReward)
        {
            Objects = autoReward.Objects.Select(obj => obj.CreateGameObject()).ToList();
        }
    }
}
