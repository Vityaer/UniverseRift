using Db.CommonDictionaries;
using Models;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace UIController.Rewards.PosibleRewards
{
    [Serializable]
    public class PosibleRewardData
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        public List<PosibleObjectData<ResourceData>> Resources = new List<PosibleObjectData<ResourceData>>();

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveItem), CustomAddFunction = nameof(AddItem))]
        public List<PosibleObjectData<ItemData>> Items = new List<PosibleObjectData<ItemData>>();

        [ListDrawerSettings(HideRemoveButton = false, DraggableItems = false, Expanded = true,
            NumberOfItemsPerPage = 20,
            CustomRemoveElementFunction = nameof(RemoveSplinter), CustomAddFunction = nameof(AddSplinter))]
        public List<PosibleObjectData<SplinterData>> Splinters = new List<PosibleObjectData<SplinterData>>();

        public List<PosibleObjectData> Objects
        {
            get
            {
                var result = new List<PosibleObjectData>(Resources.Count + Items.Count + Splinters.Count);
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
            var newItem = new ItemData(CommonDictionaries);
            Items.Add(new PosibleObjectData<ItemData> { Value = newItem });
        }

        private void RemoveItem(PosibleObjectData<ItemData> light, object b, List<PosibleObjectData<ItemData>> lights)
        {
            Items.Remove(light);
        }

        protected void AddSplinter()
        {
            var newItem = new SplinterData(CommonDictionaries);
            Splinters.Add(new PosibleObjectData<SplinterData> { Value = newItem });
        }

        private void RemoveSplinter(PosibleObjectData<SplinterData> light, object b, List<PosibleObjectData<SplinterData>> lights)
        {
            Splinters.Remove(light);
        }
    }
}