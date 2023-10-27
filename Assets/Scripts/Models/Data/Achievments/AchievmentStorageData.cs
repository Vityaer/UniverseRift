using Models.Data;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class AchievmentStorageData : BaseDataModel
    {
        public List<AchievmentData> Achievments = new();
    }
}
