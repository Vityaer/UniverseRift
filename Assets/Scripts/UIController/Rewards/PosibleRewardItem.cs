using System;
using UIController.Inventory;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardItem : PosibleRewardObject
    {
        public ItemName ID = ItemName.Stick;
        public Item GetItem { get { return new Item($"{ID}", 1); } }
    }
}