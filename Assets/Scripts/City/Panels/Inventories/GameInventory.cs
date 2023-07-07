using Common;
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
        private Dictionary<string, BaseObject> _inventoryObjects = new Dictionary<string, BaseObject>();

        private ReactiveCommand<BaseObject> _onChange = new ReactiveCommand<BaseObject>();

        public IReadOnlyDictionary<string, BaseObject> InventoryObjects => _inventoryObjects;
        public IObservable<BaseObject> OnChange => _onChange;

        public GameInventory() { }

        public void GetObjectByType<T>(List<T> values) where T : BaseObject
        {
            values = _inventoryObjects.Values.Where(baseItem => baseItem is T).Select(baseItem => (T)baseItem).ToList();
        }

        public void GetItemByType(ItemType typeItems, out List<GameItem> list)
        {
            var items = new List<GameItem>();
            GetObjectByType(items);
            list = items.Where(item => item.Type == typeItems).ToList();
        }

        public void Add(BaseObject inventoryObject)
        {
            if (!_inventoryObjects.ContainsKey(inventoryObject.Id))
            {
                _inventoryObjects[inventoryObject.Id].Add(inventoryObject.Amount);
            }
            else
            {
                _inventoryObjects.Add(inventoryObject.Id, inventoryObject);
            }
            _onChange.Execute(inventoryObject);
        }

        public void Remove(BaseObject inventoryObject)
        {
            _inventoryObjects[inventoryObject.Id].Remove(inventoryObject.Amount);
            if (_inventoryObjects[inventoryObject.Id].EqualsZero)
            {
                _inventoryObjects.Remove(inventoryObject.Id);
            }

            _onChange.Execute(inventoryObject);
        }
    }
}