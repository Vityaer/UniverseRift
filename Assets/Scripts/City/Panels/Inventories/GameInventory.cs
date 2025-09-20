using Common;
using Common.Inventories.Splinters;
using Models.Common;
using Models.Items;
using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace UIController.Inventory
{
    [Serializable]
    public class GameInventory : IInitializable, IDisposable
    {
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly GameController _gameController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private Dictionary<string, BaseObject> _items = new Dictionary<string, BaseObject>();
        private CompositeDisposable _disposables = new();
        private ReactiveCommand<BaseObject> _onChange = new ReactiveCommand<BaseObject>();

        public Dictionary<string, BaseObject> InventoryObjects => _items;
        public IObservable<BaseObject> OnChange => _onChange;

        public void Initialize()
        {
            _gameController.OnLoadedGameData.Subscribe(_ => OnLoadData()).AddTo(_disposables);
        }

        private void OnLoadData()
        {
            var inventoryData = _commonGameData.InventoryData;

            foreach (var item in inventoryData.Items)
            {
                var model = _commonDictionaries.Items[item.Id];
                var gameItem = new GameItem(model, item.Amount);
                _items.Add(gameItem.Id, gameItem);
            }

            foreach (var splinter in inventoryData.Splinters)
            {
                var model = _commonDictionaries.Splinters[splinter.Id];
                var gameSplinter = new GameSplinter(model, _commonDictionaries, splinter.Amount);
                _items.Add(splinter.Id, gameSplinter);
            }
        }

        public void GetObjectByType<T>(List<T> values) where T : BaseObject
        {
            if (values == null)
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

        public void Remove(string id, int amount)
        {
            var changedItem = _items[id];
            changedItem.Remove(amount);
            if (changedItem.EqualsZero)
            {
                _items.Remove(id);
            }

            _onChange.Execute(changedItem);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}