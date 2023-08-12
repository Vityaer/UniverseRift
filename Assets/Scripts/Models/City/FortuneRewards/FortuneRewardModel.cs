using Models.Data.Inventories;
using System;

namespace Models.City.FortuneRewards
{
    [Serializable]
    public class FortuneRewardModel : BaseModel
    {
        public float Probability;
        public float FactorDelta;
        [NonSerialized] public InventoryBaseItem Subject;
    }
}