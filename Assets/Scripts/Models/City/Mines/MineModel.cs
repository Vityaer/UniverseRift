using City.TrainCamp;
using Models.Data.Inventories;
using System;
using System.Collections.Generic;

namespace Models.City.Mines
{
    [Serializable]
    public class MineModel : BaseModel
    {
        public MineType Type;
        public string SpritePath;
        public CostLevelUpContainer IncomesContainer;
        public CostLevelUpContainer CostLevelUpContainer;
        public List<ResourceData> CreateCost = new List<ResourceData>();
    }
}
