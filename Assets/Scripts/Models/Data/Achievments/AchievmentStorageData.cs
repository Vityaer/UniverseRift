using Models.Data;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class AchievmentStorageData : BaseDataModel
    {
        public List<AchievmentData> MainRequirements = new List<AchievmentData>();
        public List<AchievmentData> EveryTimeTasks = new List<AchievmentData>();
        public SimpleBuildingData EventAgentProgress;
    }
}
