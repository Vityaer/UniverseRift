using City.TrainCamp;
using Models.Data;
using System.Collections.Generic;

namespace Models.Common
{
    [System.Serializable]
    public class CostLevelUpModel
    {
        public int level;
        public List<ResourceData> Cost;
        public CostIncreaseType typeIncrease = CostIncreaseType.Mulitiply;
        public List<float> ListIncrease = new List<float>();
    }
}
