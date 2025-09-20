using Models.Data.Rewards;
using Models.Fights.Campaign;
using System;
using Common.Db.CommonDictionaries;
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
            if(AutoFightReward != null)
                AutoFightReward.SetCommonDictionaries(dictionaries);
        }
    }
}