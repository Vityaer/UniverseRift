using Models.Common.BigDigits;
using UIController.Rewards;
using UnityEngine;

namespace City.Acievements
{
    [System.Serializable]
    public class AchievmentStageModel
    {
        public BigDigit RequireCount;
        public RewardData Reward;
    }
}