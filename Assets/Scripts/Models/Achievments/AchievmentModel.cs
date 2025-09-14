using City.Achievements;
using City.Acievements;
using Models.Common.BigDigits;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UIController.Rewards;
using Utils;

namespace Models.Achievments
{
    public class AchievmentModel : BaseModel
    {
        public ProgressType ProgressType;
        public List<AchievmentStageModel> Stages;
        public string ImplementationName;


        public BigDigit GetRequireFinishCount()
        {
            return Stages[^1].RequireCount;
        }

        public BigDigit GetRequireCount(int stage)
        {
            return Stages[stage].RequireCount;
        }

        public RewardModel GetReward(int currentStage)
        {
            return Stages[currentStage].Reward;
        }
    }
}