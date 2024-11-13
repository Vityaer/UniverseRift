using City.Panels.Inventories;
using City.Panels.SubjectPanels;
using Common;
using Common.Inventories.Splinters;
using Db.CommonDictionaries;
using Models.Common;
using Models.Items;
using System;
using System.Collections.Generic;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace UIController.Inventory
{
    public class InventoryController : UiPanelController<InventoryView>, IInitializable
    {
        private const int CELLS_COUNT = 40;

        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly SplinterPanelController _splinterPanelController;
        [Inject] private readonly ItemPanelController _itemPanelController;
        [Inject] private readonly GameInventory _gameInventory;

        public bool WaitSelected;

        private bool _isOpen;
        private List<SubjectCell> _cells = new List<SubjectCell>();
        private ReactiveCommand<BaseObject> _onObjectSelect = new ReactiveCommand<BaseObject>();
        private IDisposable _inventoryChangeDisposable;


        public ReactiveCommand OnClose = new ReactiveCommand();
        public GameInventory GameInventory => _gameInventory;
        public IObservable<BaseObject> OnObjectSelect => _onObjectSelect;

        public new void Initialize()
        {
            for (var i = 0; i < CELLS_COUNT; i++)
            {
                var cell = UnityEngine.Object.Instantiate(View.CellPrefab, View.GridParent);
                cell.OnSelect.Subscribe(OnCellClick);
                _cells.Add(cell);
            }

            base.Initialize();
        }

        public override void OnShow()
        {
            _inventoryChangeDisposable = _gameInventory.OnChange.Subscribe(_ => RefreshUi());
            base.OnShow();
        }

        private void RefreshUi()
        {
            ShowAll();
        }

        private void OnCellClick(SubjectCell cell)
        {
            if (!WaitSelected)
            {
                switch (cell.Subject)
                {
                    case GameItem item:
                        _itemPanelController.ShowData(item);
                        break;
                    case GameSplinter splinter:
                        _splinterPanelController.ShowData(splinter, true);
                        break;
                }
            }
            else
            {
                _onObjectSelect.Execute(cell.Subject);
                Close();
            }
        }

        public void ShowAll()
        {
            var items = new List<GameItem>();
            var splinters = new List<GameSplinter>();

            _gameInventory.GetObjectByType(items);
            _gameInventory.GetObjectByType(splinters);

            var allObjects = new List<BaseObject>();

            allObjects.AddRange(items);
            allObjects.AddRange(splinters);

            ShowItems(allObjects);

            OpenWindow();
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

        public override void OnHide()
        {
            OnClose.Execute();
            base.OnHide();
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

        public void Open(ItemType typeItems, HeroItemCellController cellItem = null)
        {
            _gameInventory.GetItemByType(typeItems, out var items);
            if (cellItem != null)
            {
                if (cellItem.Item != null)
                {
                    var equalsItem = items.Find(item => item.Id.Equals(cellItem.Item.Id));
                    if (equalsItem != null)
                        items.Remove(equalsItem);
                }
            }
            ShowItems(items);
            OpenWindow();
        }

        private void OpenWindow()
        {
            _isOpen = true;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<InventoryController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            _inventoryChangeDisposable?.Dispose();
            _isOpen = false;
            base.Close();
        }
    }
}