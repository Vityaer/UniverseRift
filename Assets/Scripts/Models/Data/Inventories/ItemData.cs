using Common.Resourses;
using UIController.Inventory;

namespace Models.Data.Inventories
{
    public class ItemData : BaseDataModel
    {
        public string Id;
        public int Amount;

        public ItemData() { }

        public ItemData(GameItem item)
        {
            Id = item.Id;
            Amount = item.Amount;
        }
    }
}
