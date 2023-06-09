using Models.Fights.Campaign;
using UIController.Rewards;
using UnityEngine;

namespace City.Buildings.TravelCircle
{
    [System.Serializable]
    public class MissionWithSmashReward : MissionModel
    {
        [SerializeField] protected Reward smashReward;
        public Reward SmashReward { get => smashReward; }
        public MissionWithSmashReward Clone()
        {
            return new MissionWithSmashReward
            {
                Name = this.Name,
                ListEnemy = this.ListEnemy,
                WinReward = (Reward)this.WinReward.Clone(),
                smashReward = smashReward.Clone(),
                Location = this.Location
            };
        }
    }
}