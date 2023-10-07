using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.Data.Dailies
{
    public class DailyRewardContainer : SimpleBuildingData
    {
        public List<InventoryBaseItem> Rewards = new List<InventoryBaseItem>();
    }
}
