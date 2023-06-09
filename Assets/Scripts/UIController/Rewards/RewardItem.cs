using UIController.Inventory;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardItem
    {
        public ItemName ID = ItemName.Stick;
        public int amount = 1;
        private Item item = null;
        public Item GetItem { get { if (item == null) item = new Item($"{ID}", amount); return item; } }
        public RewardItem Clone() { return new RewardItem(ID, amount); }
        public RewardItem(ItemName ID, int amount)
        {
            this.ID = ID;
            this.amount = amount;
        }
        public RewardItem(Item item)
        {
            ID = ItemName.Stick;
            this.item = item;
        }
    }
}