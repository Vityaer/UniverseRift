using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class InventorySave
    {
        public List<ItemModel> listItem = new List<ItemModel>();
        public List<SplinterModel> listSplinter = new List<SplinterModel>();
        public InventorySave(Inventory inventory)
        {
            foreach (ItemController item in inventory.items)
                listItem.Add(new ItemModel(item));
            foreach (SplinterController splinter in inventory.splinters)
                listSplinter.Add(new SplinterModel(splinter));
        }
    }
}
