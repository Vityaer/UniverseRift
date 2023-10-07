using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.Data.Dailies
{
    public class DailyRewardModel : BaseModel
    {
        public List<InventoryBaseItem> Rewards = new List<InventoryBaseItem>();
    }
}
