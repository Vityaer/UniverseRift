using City.Acievements;
using Common;
using Models.Common.BigDigits;
using System;
using System.Collections.Generic;
using UIController.Rewards;

namespace Models.Achievments
{
    public class AchievmentModel : BaseModel
    {
        public AchievmentType Type;
        public ProgressType ProgressType;
        public List<AchievmentStageModel> Stages;

        public BigDigit GetRequireFinishCount()
        {
            return Stages[Stages.Count - 1].RequireCount;
        }

        public BigDigit GetRequireCount(int stage)
        {
            return Stages[stage].RequireCount;
        }

        public RewardData GetReward(int currentStage)
        {
            return Stages[currentStage].Reward;
        }
    }
}