using Models.Requiremets;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class RequirementStorageModel : BaseModel
    {
        public List<AchievementSave> saveMainRequirements = new List<AchievementSave>();
        public List<AchievementSave> saveEveryTimeTasks = new List<AchievementSave>();
        public SimpleBuildingModel eventAgentProgress;
    }
}
