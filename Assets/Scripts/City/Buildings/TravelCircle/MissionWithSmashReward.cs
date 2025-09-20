using Common.Db.CommonDictionaries;
using Models.Fights.Campaign;
using UIController.Rewards;
using UnityEngine;

namespace City.Buildings.TravelCircle
{
    [System.Serializable]
    public class MissionWithSmashReward : MissionModel
    {
        public RewardModel SmashReward;

        public MissionWithSmashReward()
        {
        }

        public MissionWithSmashReward(CommonDictionaries dictionaries) : base(dictionaries)
        {
        }

        public MissionWithSmashReward Clone()
        {
            return new MissionWithSmashReward
            {
                Name = this.Name,
                Units = this.Units,
                //WinReward = (GameReward)this.WinReward.Clone(),
                //smashReward = smashReward.Clone(),
                Location = this.Location
            };
        }
    }
}