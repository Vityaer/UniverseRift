using City.Panels.Inventories;
using City.Panels.SubjectPanels;
using City.Panels.SubjectPanels.Resources;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Models.Items;
using System.Collections.Generic;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UIController.Inventory
{
    public class InventoryController : UiPanelController<InventoryView>, IInitializable
    {
        [Inject] private readonly SplinterPanelController _splinterPanelController;
        [Inject] private readonly ResourcePanelController _resourcePanelController;
        [Inject] private readonly ItemPanelController _itemPanelController;

        private const int CELLS_COUNT = 40;

        private GameInventory _gameInventory;

        private HeroItemCellController cellItem = null;
        private bool isOpenInventory = false;

        private List<SubjectCell> _cells = new List<SubjectCell>();

        public GameInventory GameInventory => _gameInventory;

        public void Initialize()
        {
            //for (var i = 0; i < CELLS_COUNT; i++)
            //{
            //    var cell = Object.Instantiate(View.CellPrefab, View.GridParent);
            //    _cells.Add(cell);
            //}
        }

        private void ShowItems<T>(List<T> list) where T : BaseObject
        {
            for (int i = 0; i < list.Count; i++)
            {
                _cells[i].SetData(list[i]);
            }

            for (int i = list.Count; i < _cells.Count; i++)
            {
                _cells[i].Clear();
            }
        }


        public bool CheckItems(GameItem item, int count = 1)
        {
            var result = false;
            var workItem = _gameInventory.InventoryObjects[item.Id];

            if (workItem != null)
                result = workItem.CheckCount(count);

            return result;
        }

        public void Add<T>(T obj) where T : BaseObject
        {
            _gameInventory.Add(obj);
        }

        public void Add<T>(List<T> list) where T : BaseObject
        {
            foreach (var item in list)
            {
                _gameInventory.Add(item);
            }
        }

        public void Remove<T>(T obj) where T : BaseObject
        {
            _gameInventory.Remove(obj);
        }

        public void Remove<T>(List<T> list) where T : BaseObject
        {
            foreach (var item in list)
            {
                _gameInventory.Remove(item);
            }
        }

        public void SelectItem(GameItem gameItem)
        {
            if (cellItem != null)
            {
                cellItem.SetItem(gameItem);
                gameItem.Remove(1);
                if (gameItem.EqualsZero)
                    _gameInventory.Remove(gameItem);
                cellItem = null;
                gameItem = null;
            }
            CloseAll();
        }

        public void Open(ItemType type)
        {
            Open(type, cellItem: null);
        }

        public void Open(ItemType typeItems, HeroItemCellController cellItem = null)
        {
            this.cellItem = cellItem;
            _gameInventory.GetItemByType(typeItems, out var items);
            ShowItems(items);
        }

        public void OpenInfoItem(GameItem item)
        {
            _itemPanelController.ShowData(item, cellItem);
        }

        public void OpenInfoItem(GameResource res)
        {
            _resourcePanelController.ShowData(res);
        }

        public void OpenInfoItem(GameItem item, ItemType typeItems, HeroItemCellController cellItem)
        {
            // canvasInventory.enabled = true;
            this.cellItem = cellItem;
            _itemPanelController.ShowData(item, this.cellItem, onHero: true);
        }

        public void OpenInfoItem(GameSplinter splinter)
        {
            //_splinterPanelController.ShowData(spliter);
        }

        public void OpenInfoItem(GameSplinter splinterController, bool withControl)
        {
            //selectSplinter = splinterController;
            //panelInfoSplinter.OpenInfoAboutSplinter(splinterController, withControl);
        }

        [ContextMenu("Close")]
        public void Close()
        {
            //panelInfoItem.Close();
            //panelInfoSplinter.Close();
            //if (isOpenInventory)
            //{
            //    panelInventory.SetActive(false);
            //    isOpenInventory = false;
            //}
            //canvasInventory.enabled = false;
            //cellItem = null;
        }

        public void CloseAll()
        {
            //panelInfoItem.Close();
            //panelInventory.SetActive(false);
            //isOpenInventory = false;
            //canvasInventory.enabled = false;
        }

        void Start()
        {
            //LoadInformation();
            //inventory.RegisterOnChange(Refresh);
        }

        void OnApplicationPause(bool pauseStatus)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
		SaveLoadController.SaveInventory(inventory);
#endif
        }

        void OnDestroy()
        {
            //SaveLoadController.SaveInventory(inventory);
            //inventory.UnregisterOnChange(Refresh);
        }

        void LoadInformation()
        {
            //inventory = SaveLoadController.LoadInventory();
        }
    }
}