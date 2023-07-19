using Common;
using Common.Inventories.Splinters;
using Db.CommonDictionaries;
using Models;
using Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace UIController.Inventory
{
    [Serializable]
    public class GameInventory
    {
        private CommonDictionaries _commonDictionaries;
        private Dictionary<string, BaseObject> _items = new Dictionary<string, BaseObject>();

        private ReactiveCommand<BaseObject> _onChange = new ReactiveCommand<BaseObject>();

        public IReadOnlyDictionary<string, BaseObject> InventoryObjects => _items;
        public IObservable<BaseObject> OnChange => _onChange;

        public GameInventory() { }

        public GameInventory(CommonDictionaries commonDictionaries, InventoryData inventoryData)
        {
            _commonDictionaries = commonDictionaries;

            if (inventoryData == null)
                return;

            foreach (var item in inventoryData.Items)
            {
                var model = _commonDictionaries.Items[item.Id];
                var gameItem = new GameItem(model, item.Amount);
                _items.Add(gameItem.Id, gameItem);
            }

            foreach (var splinter in inventoryData.Splinters)
            {
                var gameSplinter = new GameSplinter(splinter.Id, splinter.Amount);
                _items.Add(splinter.Id, gameSplinter);
            }
        }

        public void GetObjectByType<T>(List<T> values) where T : BaseObject
        {
            if(values == null)
                values = new List<T>();

            foreach (var item in _items)
            {
                if (item.Value.GetType() == typeof(T))
                    values.Add(item.Value as T);
            }
        }

        public void GetItemByType(ItemType typeItems, out List<GameItem> list)
        {
            var items = new List<GameItem>();
            GetObjectByType(items);
            list = items.FindAll(item => item.Type == typeItems);
        }

        public void Add(BaseObject inventoryObject)
        {
            if (_items.ContainsKey(inventoryObject.Id))
            {
                _items[inventoryObject.Id].Add(inventoryObject.Amount);
            }
            else
            {
                _items.Add(inventoryObject.Id, inventoryObject);
            }
            _onChange.Execute(inventoryObject);

        }

        public void Remove(BaseObject inventoryObject)
        {
            _items[inventoryObject.Id].Remove(inventoryObject.Amount);
            if (_items[inventoryObject.Id].EqualsZero)
            {
                _items.Remove(inventoryObject.Id);
            }

            _onChange.Execute(inventoryObject);
        }
    }
}