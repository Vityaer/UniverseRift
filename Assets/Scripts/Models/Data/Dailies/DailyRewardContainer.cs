using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.Data.Dailies
{
    public class DailyRewardContainer : SimpleBuildingData
    {
        public int CurrentDailyReward = 0;
        public bool CanGetDailyReward = false;
        public List<InventoryBaseItem> Rewards = new List<InventoryBaseItem>();
    }
}
