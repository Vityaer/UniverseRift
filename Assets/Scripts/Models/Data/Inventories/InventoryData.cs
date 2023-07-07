using Common.Inventories.Splinters;
using Common.Resourses;
using Models.Data;
using Models.Data.Inventories;
using System.Collections.Generic;
using UIController.Inventory;

namespace Models
{
    [System.Serializable]
    public class InventoryData : BaseDataModel
    {
        public List<ResourceData> Resources = new List<ResourceData>();
        public List<ItemData> Items = new List<ItemData>();
        public List<SplinterData> Splinters = new List<SplinterData>();

        public InventoryData(GameInventory inventory)
        {
            foreach (var obj in inventory.InventoryObjects.Values)
            {
                switch (obj)
                {
                    case GameResource res:
                        Resources.Add(new ResourceData() { Type = res.Type, Amount = res.Amount});
                        break;
                    case GameItem res:
                        Items.Add(new ItemData(res));
                        break;
                    case GameSplinter res:
                        Splinters.Add(new SplinterData(res));
                        break;
                }
            }
        }
    }
}
