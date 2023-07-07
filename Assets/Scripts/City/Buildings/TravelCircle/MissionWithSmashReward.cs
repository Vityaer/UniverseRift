using Models.Fights.Campaign;
using UIController.Rewards;
using UnityEngine;

namespace City.Buildings.TravelCircle
{
    [System.Serializable]
    public class MissionWithSmashReward : MissionModel
    {
        [SerializeField] protected RewardData smashReward;
        public RewardData SmashReward { get => smashReward; }
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