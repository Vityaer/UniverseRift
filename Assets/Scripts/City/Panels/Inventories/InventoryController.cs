using City.Panels.Inventories;
using Common;
using Common.Inventories.Splinters;
using Models.Common;
using Models.Items;
using System.Collections.Generic;
using System.Linq;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using UniRx;
using Models;
using VContainerUi.Model;
using Db.CommonDictionaries;
using System;

namespace UIController.Inventory
{
    public class InventoryController : UiPanelController<InventoryView>, IInitializable
    {
        private const int CELLS_COUNT = 40;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly CommonGameData _commonGameData;

        private GameInventory _gameInventory;
        private List<SubjectCell> _cells = new List<SubjectCell>();
        private ReactiveCommand<BaseObject> _onObjectSelect = new ReactiveCommand<BaseObject>();

        public IObservable<BaseObject> OnObjectSelect => _onObjectSelect;
        public GameInventory GameInventory => _gameInventory;
        public ReactiveCommand OnClose = new ReactiveCommand();

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

        private void OnCellClick(SubjectCell cell)
        {
            _onObjectSelect.Execute(cell.Subject);
            Close();
        }

        protected override void OnLoadGame()
        {
            _gameInventory = new GameInventory(_commonDictionaries, _commonGameData.InventoryData);
            GameController.OnGameSave.Subscribe(_ => OnSaveGame()).AddTo(Disposables);
        }

        private void OnSaveGame()
        {
            _commonGameData.InventoryData = new InventoryData(_gameInventory);
        }

        public void ShowAll()
        {
            var items = new List<GameItem>();
            _gameInventory.GetObjectByType(items);
            ShowItems(items);
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
            ShowItems(items);
            OpenWindow();
        }

        private void OpenWindow()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<InventoryController>(openType: OpenType.Additive);
        }


    }
}