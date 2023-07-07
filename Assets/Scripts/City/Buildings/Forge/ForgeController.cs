using Assets.Scripts.ClientServices;
using City.Buildings.Abstractions;
using City.Panels.Messages;
using Common.Resourses;
using Db.CommonDictionaries;
using Models.Common.BigDigits;
using System;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards;
using UniRx;
using VContainer;

namespace City.Buildings.Forge
{
    public class ForgeController : BaseBuilding<ForgeView>
    {
        private const int ITEMS_COUNT = 16;

        [Inject] private readonly InventoryController _inventoryController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private List<ForgeItemVisual> _listPlace = new List<ForgeItemVisual>();
        private List<GameResource> _listCostItems = new List<GameResource>();

        private List<ItemSynthesis> _weapons;
        private List<ItemSynthesis> _armors;
        private List<ItemSynthesis> _necklaces;
        private List<ItemSynthesis> _boots;
        private List<ItemSynthesis> _currentItems;

        private int _currentIndex;
        private ItemSynthesis _currentItem;
        private ForgeItemVisual _currentCell = null;
        private ReactiveCommand<BigDigit> _onCraft = new ReactiveCommand<BigDigit>();

        public IObservable<BigDigit> OnCraft => _onCraft;

        protected override void OnStart()
        {
            //View.WeaponPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_weapons)).AddTo(Disposables);
            //View.ArmorPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_armors)).AddTo(Disposables);
            //View.BootsPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_necklaces)).AddTo(Disposables);
            //View.AmuletPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_boots)).AddTo(Disposables);
            //View.ButtonSynthesis.OnClickAsObservable().Subscribe(_ => SynthesisItem()).AddTo(Disposables);

            //for (var i = 0; i < ITEMS_COUNT; i++)
            //{
            //    var itemVisual = UnityEngine.Object.Instantiate(View.ForgeItemVisualPrefab, View.Content);
            //    itemVisual.OnSelected.Subscribe(SelectItem).AddTo(Disposables);
            //    _listPlace.Add(itemVisual);
            //}

            //OpenList(_weapons);
            //SelectItem(_listPlace[0]);
        }

        public void OpenList(List<ItemSynthesis> items)
        {
            _currentItems = items;

            for (int i = 0; i < items.Count; i++)
            {
                _listPlace[i].SetItem(items[i]);
                CheckItemCount(i);
            }

            if (_currentCell != null)
                SelectItem(_currentCell);

            _currentCell?.UIItem.Select();
        }

        public void SelectItem(ForgeItemVisual selectedCell)
        {
            _currentCell?.UIItem.Diselect();
            _currentItem = selectedCell.Thing;
            _currentIndex = _listPlace.FindIndex(place => place == selectedCell);
            _currentCell = selectedCell;
            View.LeftItem.SetItem(_currentItem.requireItem, _currentItem.countRequireItem);
            View.RightItem.SetResource(_currentItem.requireResource);
            CheckDemands();
        }

        public void CheckDemands()
        {
            View.ButtonSynthesis.interactable = _resourceStorageController.CheckResource(_currentItem.requireResource) && _inventoryController.CheckItems(_currentItem.requireItem, _currentItem.countRequireItem);
            RecalculateDemands();
        }

        private void RecalculateDemands()
        {
            View.LeftItem.forgeItemCost.CheckItems();
            View.RightItem.resourceCost.CheckResource();
        }

        public void SynthesisItem()
        {
            int createdCount = 0;
            RewardData reward = new RewardData();

            var requirementItems =
                new GameItem()
                {
                    Id = _currentItem.IDRequireItem,
                    Amount = _currentItem.countRequireItem
                };

            while (_resourceStorageController.CheckResource(_currentItem.requireResource) && _inventoryController.CheckItems(_currentItem.requireItem, _currentItem.countRequireItem))
            {
                _resourceStorageController.SubtractResource(_currentItem.requireResource);
                _inventoryController.Remove(_currentItem.requireItem);
                _onCraft.Execute(new BigDigit(1));
                createdCount += 1;
            }

            var newItem = new GameItem() { Amount = createdCount };
            //reward.Add(newItem);
            //MessageController.Instance.OpenSimpleRewardPanel(reward);

            CheckItemCount(_currentIndex);
            CheckItemCount(_currentIndex + 1);
            CheckDemands();
        }

        private void CheckItemCount(int index)
        {
            var item = _listPlace[index].Thing;
            var flag = _inventoryController.GameInventory.InventoryObjects[item.requireItem.Id].CheckCount(_currentItem.countRequireItem);

            _currentCell.UIItem.SwitchDoneForUse(flag);
        }
    }
}