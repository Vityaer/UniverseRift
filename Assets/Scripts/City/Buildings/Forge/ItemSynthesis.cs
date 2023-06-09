using UnityEngine;
using System;
using Common.Resourses;
using UIController.Inventory;
#if UNITY_EDITOR_WIN
#endif

namespace City.Buildings.Forge
{
    [Serializable]
    public class ItemSynthesis
    {
        [Header("Require")]
        public string IDRequireItem;
        public int countRequireItem;
        public Resource requireResource;

        [Header("Reward")]
        public string IDReward;

        private Item _reward = null;
        public Item reward
        {
            get
            {
                if (_reward == null) _reward = GetItem(IDReward);
                return _reward;
            }
        }
        private Item _requireItem = null;
        public Item requireItem
        {
            get
            {
                if (_requireItem == null) _requireItem = GetItem(IDRequireItem);
                return _requireItem;
            }
        }
        private Item GetItem(string ID)
        {
            return Item.GetItem(ID);
        }

    }
}