using Common.Resourses;
using Common.Rewards;
using Models.Data.Inventories;
using System;
using UIController.Rewards.PosibleRewards;
using Utils;

namespace Models.Data.Rewards
{
    [Serializable]
    public class AutoRewardData : PosibleReward
    {
        public SerializableDictionary<ResourceType, ResourceData> BaseResource;

        private ResourceData workResource;
        private ItemData workItem;
        private SplinterData workSplinter;

        public GameReward GetCaculateReward(int countTact)
        {
            GameReward reward = new GameReward();
            foreach (var resourceData in BaseResource.Values)
            {
                var resource = new GameResource(resourceData.Type, resourceData.Amount) * countTact;
                reward.Objects.Add(resource);
            }
            return reward;
        }


    }
}