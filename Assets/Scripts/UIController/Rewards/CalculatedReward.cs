using Common.Inventories.Splinters;
using Common.Resourses;
using System.Collections;
using System.Collections.Generic;
using UIController.Inventory;
using UnityEngine;

namespace UIController.Rewards
{
    [System.Serializable]
    public class CalculatedReward
    {
        [SerializeField] private List<GameResource> _resources = new List<GameResource>();
        [SerializeField] private List<GameItem> _items = new List<GameItem>();
        [SerializeField] private List<GameSplinter> _splinters = new List<GameSplinter>();
        public int AllCount  => _resources.Count + _items.Count + _splinters.Count; 

        public CalculatedReward() { }

        public CalculatedReward(List<GameResource> listRes, List<GameItem> items, List<GameSplinter> splinters)
        {
            _resources = listRes;
            this._items = items;
            this._splinters = splinters;
        }

        //public CalculatedReward Clone()
        //{
            //ListResource listRes = (ListResource)_resources.Clone();
            //List<ItemController> items = new List<ItemController>();
            //List<SplinterController> splinters = new List<SplinterController>();
            //foreach (ItemController item in this._items) { items.Add((ItemController)item.Clone()); }
            //foreach (SplinterController splinter in this._splinters) { splinters.Add((SplinterController)splinter.Clone()); }
            //return new CalculatedReward(listRes, items, splinters);
        //}

    }
}