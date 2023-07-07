using UnityEngine;
using System;
using UIController.Inventory;
using Common.Resourses;
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
        public GameResource requireResource;

        [Header("Reward")]
        public string IDReward;

        private GameItem _reward = null;
        public GameItem reward
        {
            get
            {
                //if (_reward == null) _reward = GetItem(IDReward);
                return _reward;
            }
        }
        private GameItem _requireItem = null;
        public GameItem requireItem
        {
            get
            {
                //if (_requireItem == null) _requireItem = GetItem(IDRequireItem);
                return _requireItem;
            }
        }
    }
}