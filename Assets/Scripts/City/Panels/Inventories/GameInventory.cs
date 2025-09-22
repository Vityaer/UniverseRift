using System;
using System.Collections.Generic;
using Common;
using Common.Db.CommonDictionaries;
using Common.Inventories.Splinters;
using Models.Common;
using Models.Items;
using UIController.Inventory;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Panels.Inventories
{
    [Serializable]
    public class GameInventory : IInitializable, IDisposable
    {
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly GameController m_gameController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;

        private Dictionary<string, BaseObject> m_items = new Dictionary<string, BaseObject>();
        private CompositeDisposable m_disposables = new();
        private readonly ReactiveCommand<BaseObject> m_onChange = new ReactiveCommand<BaseObject>();

        public Dictionary<string, BaseObject> InventoryObjects => m_items;
        public IObservable<BaseObject> OnChange => m_onChange;

        public void Initialize()
        {
            m_gameController.OnLoadedGameData.Subscribe(_ => OnLoadData()).AddTo(m_disposables);
        }

        private void OnLoadData()
        {
            var inventoryData = m_commonGameData.InventoryData;

            foreach (var item in inventoryData.Items)
            {
                var model = m_commonDictionaries.Items[item.Id];
                var gameItem = new GameItem(model, item.Amount);
                m_items.Add(gameItem.Id, gameItem);
            }

            foreach (var splinter in inventoryData.Splinters)
            {
                var model = m_commonDictionaries.Splinters[splinter.Id];
                var gameSplinter = new GameSplinter(model, m_commonDictionaries, splinter.Amount);
                m_items.Add(splinter.Id, gameSplinter);
            }
        }

        public void GetObjectByType<T>(List<T> values) where T : BaseObject
        {
            values ??= new List<T>();

            foreach (var item in m_items)
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
            if (!m_items.TryAdd(inventoryObject.Id, inventoryObject))
            {
                m_items[inventoryObject.Id].Add(inventoryObject.Amount);
            }

            m_onChange.Execute(inventoryObject);

        }

        public void Remove(BaseObject inventoryObject)
        {
            m_items[inventoryObject.Id].Remove(inventoryObject.Amount);
            if (m_items[inventoryObject.Id].EqualsZero)
            {
                m_items.Remove(inventoryObject.Id);
            }

            m_onChange.Execute(inventoryObject);
        }

        public void Remove(string id, int amount)
        {
            var changedItem = m_items[id];
            changedItem.Remove(amount);
            if (changedItem.EqualsZero)
            {
                m_items.Remove(id);
            }

            m_onChange.Execute(changedItem);
        }

        public void Dispose()
        {
            m_disposables.Dispose();
        }
    }
}