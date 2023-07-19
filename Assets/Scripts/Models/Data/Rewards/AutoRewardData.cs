using Common.Resourses;
using Common.Rewards;
using Models.Data.Inventories;
using System;
using UIController.Rewards.PosibleRewards;
using Utils;

namespace Models.Data.Rewards
{
    [Serializable]
    public class AutoRewardData : PosibleRewardData
    {
        public SerializableDictionary<ResourceType, ResourceData> BaseResource;

        public GameReward GetCaculateReward(int countTact)
        {
            var reward = new GameReward();
            foreach (var resourceData in BaseResource.Values)
            {
                var resource = new GameResource(resourceData.Type, resourceData.Amount) * countTact;
                reward.Objects.Add(resource);
            }
            return reward;
        }
    }
}