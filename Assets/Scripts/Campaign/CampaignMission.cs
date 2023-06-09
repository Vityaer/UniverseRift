using GeneralObject;
using Models.Fights.Campaign;
using System;
using UIController.Rewards;
using UnityEngine;

namespace Campaign
{
    [System.Serializable]
    public class CampaignMission : MissionModel, ICloneable
    {
        [Header("Auto fight reward")]
        [SerializeField] private AutoReward _autoFightReward;

        public AutoReward AutoFightReward => _autoFightReward;

        public object Clone()
        {
            return new CampaignMission
            {
                Name = this.Name,
                ListEnemy = this.ListEnemy,
                WinReward = (Reward)this.WinReward.Clone(),
                _autoFightReward = _autoFightReward,
                Location = this.Location
            };
        }
    }
}