using Db.CommonDictionaries;
using Models.Data.Rewards;
using Models.Fights.Campaign;
using System;
using UnityEngine;

namespace Campaign
{
    [Serializable]
    public class CampaignMissionModel : MissionModel
    {
        [Header("Auto fight reward")]
        public AutoRewardData AutoFightReward;

        public CampaignMissionModel(CommonDictionaries dictionaries) : base(dictionaries)
        {
        }
    }
}