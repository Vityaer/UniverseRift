using System;
using System.Collections.Generic;
using City.Panels.SubjectPanels;
using City.Panels.SubjectPanels.Resources;
using ClientServices;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using Db.CommonDictionaries;
using Models.Common;
using Models.Items;
using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.Inventories
{
    public class InventoryPanelController : UiPanelController<InventoryView>, IInitializable
    {
        private const int CELLS_COUNT = 40;

        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly SplinterPanelController m_splinterPanelController;
        [Inject] private readonly ItemPanelController m_itemPanelController;
        [Inject] private readonly ResourcePanelController m_resourcePanelController;
        [Inject] private readonly GameInventory m_gameInventory;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;

        public bool WaitSelected;

        private bool m_isOpen;
        private readonly List<SubjectCell> m_cells = new List<SubjectCell>();
        private readonly ReactiveCommand<BaseObject> m_onObjectSelect = new ReactiveCommand<BaseObject>();
        private IDisposable m_inventoryChangeDisposable;

        private InventoryPageType? m_currentPage = null;

        public ReactiveCommand OnClose = new ReactiveCommand();
        public GameInventory GameInventory => m_gameInventory;
        public IObservable<BaseObject> OnObjectSelect => m_onObjectSelect;

        public new void Initialize()
        {
            for (var i = 0; i < CELLS_COUNT; i++)
            {
                var cell = UnityEngine.Object.Instantiate(View.CellPrefab, View.GridParent);
                cell.SetScroll(View.ScrollRect);
                cell.OnSelect.Subscribe(OnCellClick).AddTo(Disposables);
                m_cells.Add(cell);
            }

            foreach (var pageButton in View.PagesButton)
                pageButton.Value.Button.OnClickAsObservable()
                    .Subscribe(_ => OpenPage(pageButton.Key))
                    .AddTo(Disposables);


            base.Initialize();
        }

        public override void Start()
        {
            OpenPage(InventoryPageType.Splinters);
            View.GridOverrider.RecalculateGridSize();
            base.Start();
        }

        private void OpenPage(InventoryPageType pageButtonKey)
        {
            if (m_currentPage.HasValue) View.PagesButton[m_currentPage.Value].Unselect();

            View.PagesButton[pageButtonKey].Select();
            m_currentPage = pageButtonKey;

            switch (pageButtonKey)
            {
                case InventoryPageType.Items:
                    OpenItemsByType<GameItem>();
                    break;
                case InventoryPageType.Splinters:
                    OpenItemsByType<GameSplinter>();
                    break;
                case InventoryPageType.Resources:
                    OpenResources();
                    break;
            }
        }

        private void OpenResources()
        {
            var allObjects = new List<BaseObject>();
            foreach (var resource in m_resourceStorageController.Resources)
            {
                if (View.BannedResourceTypes.Contains(resource.Key))
                    continue;

                if (resource.Value.Amount.Mantissa <= 0.01f)
                    continue;

                allObjects.Add(resource.Value);
            }

            ShowItems(allObjects);
        }

        public override void OnShow()
        {
            m_inventoryChangeDisposable = m_gameInventory.OnChange.Subscribe(_ => RefreshUi());
            View.GridOverrider.RecalculateGridSize();
            base.OnShow();

            if (m_currentPage.HasValue)
                OpenPage(m_currentPage.Value);
        }

        private void OpenItemsByType<T>()
            where T : BaseObject
        {
            var items = new List<T>();

            m_gameInventory.GetObjectByType(items);

            var allObjects = new List<BaseObject>();

            allObjects.AddRange(items);

            ShowItems(allObjects);
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
                        m_itemPanelController.ShowData(item);
                        break;
                    case GameSplinter splinter:
                        m_splinterPanelController.ShowData(splinter, true);
                        break;
                    case GameResource resource:
                        m_resourcePanelController.ShowData(resource);
                        break;
                }
            }
            else
            {
                m_onObjectSelect.Execute(cell.Subject);
                Close();
            }
        }

        public void ShowAll()
        {
            var items = new List<GameItem>();
            var splinters = new List<GameSplinter>();

            m_gameInventory.GetObjectByType(items);
            m_gameInventory.GetObjectByType(splinters);

            var allObjects = new List<BaseObject>();

            allObjects.AddRange(items);
            allObjects.AddRange(splinters);

            ShowItems(allObjects);
            View.ControllerPanel.SetActive(true);
            OpenWindow();
        }

        private void ShowItems<T>(List<T> list) where T : BaseObject
        {
            for (var i = 0; i < list.Count; i++) m_cells[i].SetData(list[i]);

            for (var i = list.Count; i < m_cells.Count; i++) m_cells[i].Clear();
        }

        public override void OnHide()
        {
            OnClose.Execute();
            base.OnHide();
        }

        public void Add<T>(T obj) where T : BaseObject
        {
            m_gameInventory.Add(obj);
        }

        public void Add<T>(List<T> list) where T : BaseObject
        {
            foreach (var item in list) m_gameInventory.Add(item);
        }

        public void Remove<T>(T obj) where T : BaseObject
        {
            m_gameInventory.Remove(obj);
        }

        public void Remove<T>(List<T> list) where T : BaseObject
        {
            foreach (var item in list) m_gameInventory.Remove(item);
        }

        public void Open(ItemType typeItems, HeroItemCellController cellItem = null)
        {
            m_gameInventory.GetItemByType(typeItems, out var items);
            if (cellItem != null)
                if (cellItem.Item != null)
                {
                    var equalsItem = items.Find(item => item.Id.Equals(cellItem.Item.Id));
                    if (equalsItem != null)
                        items.Remove(equalsItem);
                }

            ShowItems(items);
            View.ControllerPanel.SetActive(false);
            OpenWindow();
        }

        private void OpenWindow()
        {
            m_isOpen = true;
            MessagesPublisher.OpenWindowPublisher.OpenWindow<InventoryPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            m_inventoryChangeDisposable?.Dispose();
            m_isOpen = false;
            base.Close();
        }
    }
}