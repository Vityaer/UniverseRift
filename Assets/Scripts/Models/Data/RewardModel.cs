using Db.CommonDictionaries;
using Models;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardModel : BaseModel
    {
        [NonSerialized] public CommonDictionaries _dictionaries;

        public List<ResourceData> Resources = new List<ResourceData>();

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
        NumberOfItemsPerPage = 20,
        CustomRemoveElementFunction = nameof(RemoveItem), CustomAddFunction = nameof(AddItem))]
        public List<ItemData> Items = new List<ItemData>();

        public List<SplinterData> Splinters = new List<SplinterData>();

        public List<InventoryBaseItem> Objects
        {
            get
            {
                var result = new List<InventoryBaseItem>(Resources.Count + Items.Count + Splinters.Count);
                foreach (var item in Resources)
                    result.Add(item);

                foreach (var item in Items)
                    result.Add(item);

                foreach (var item in Splinters)
                    result.Add(item);

                return result;
            }
        }

        protected void AddItem()
        {
            Items.Add(new ItemData(_dictionaries));
        }

        private void RemoveItem(ItemData light, object b, List<ItemData> lights)
        {
            Items.Remove(light);
        }
    }
}