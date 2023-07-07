using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Models.Data;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UIController.Inventory;
using UnityEngine;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardData
    {
        public List<BaseDataModel> Objects;

        public void Add(BaseDataModel newObj)
        {
            if(Objects == null)
                Objects = new List<BaseDataModel>();

            Objects.Add(newObj);
        }

        [HorizontalGroup("Split", 0.33f)]
        [Button("Add Resource")]
        private void AddResource()
        {
            Add(new ResourceData());
        }

        [HorizontalGroup("Split", 0.33f)]
        [Button("Add Item")]
        private void AddItem()
        {
            Add(new ItemData());
        }

        [HorizontalGroup("Split", 0.33f)]
        [Button("Add Splinter")]
        private void AddSplinter()
        {
            Add(new SplinterData());
        }
    }
}