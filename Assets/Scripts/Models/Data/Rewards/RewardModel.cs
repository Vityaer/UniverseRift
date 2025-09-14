using Db.CommonDictionaries;
using Models;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardModel : BaseModel
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

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
            Items.Add(new ItemData(CommonDictionaries));
        }

        private void RemoveItem(ItemData light, object b, List<ItemData> lights)
        {
            Items.Remove(light);
        }
        
        public static RewardModel operator *(RewardModel a, float factor)
        {
            RewardModel result = new RewardModel();
            foreach (ResourceData res in a.Resources)
            {
                result.Resources.Add(new ResourceData() {Type = res.Type, Amount = res.Amount * factor});
            }
            
            foreach (ItemData itemData in a.Items)
            {
                result.Items.Add(new ItemData() {Id = itemData.Id, Amount = itemData.Amount});
            }
            
            foreach (SplinterData splinter in a.Splinters)
            {
                int count = (int) Math.Clamp(Math.Floor(splinter.Amount * factor), 1, 100);
                result.Splinters.Add(new SplinterData() {Id = splinter.Id, Amount = count});
            }

            return result;
        }
    }
}