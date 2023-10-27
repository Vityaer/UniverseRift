using City.Buildings.Abstractions;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models;
using Models.Common.BigDigits;
using Models.Data.Inventories;
using Models.Items;
using Network.DataServer;
using Network.DataServer.Messages.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UIController.Inventory;
using UIController.ItemVisual.Forges;
using UIController.Rewards;
using UniRx;
using VContainer;

namespace City.Buildings.Forge
{
    public class ForgeController : BaseBuilding<ForgeView>
    {
        private const int ITEMS_COUNT = 16;

        private List<string> SetNames = new List<string>() { "Pupil", "Peasant", "Militiaman", "Monk", "Warrior", "Feller", "Soldier", "Minotaur", "Demon", "Druid", "Obedient", "Devil", "Destiny", "Archangel", "Titan", "God" };

        [Inject] private readonly InventoryController _inventoryController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private List<ForgeItemVisual> _listPlace = new List<ForgeItemVisual>();
        private List<GameResource> _listCostItems = new List<GameResource>();

        private List<GameItemRelation> _weapons = new List<GameItemRelation>();
        private List<GameItemRelation> _armors = new List<GameItemRelation>();
        private List<GameItemRelation> _necklaces = new List<GameItemRelation>();
        private List<GameItemRelation> _boots = new List<GameItemRelation>();
        private List<GameItemRelation> _currentItems;

        private int _currentIndex;
        private GameItemRelation _currentItem;
        private ForgeItemVisual _currentCell = null;
        private ReactiveCommand<BigDigit> _onCraft = new ReactiveCommand<BigDigit>();
        private IDisposable _onGetReward;

        public IObservable<BigDigit> OnCraft => _onCraft;

        protected override void OnStart()
        {
            View.WeaponPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_weapons)).AddTo(Disposables);
            View.ArmorPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_armors)).AddTo(Disposables);
            View.BootsPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_boots)).AddTo(Disposables);
            View.AmuletPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(_necklaces)).AddTo(Disposables);
            View.ButtonSynthesis.OnClickAsObservable().Subscribe(_ => SynthesisItem().Forget()).AddTo(Disposables);

            for (var i = 0; i < ITEMS_COUNT; i++)
            {
                var itemVisual = UnityEngine.Object.Instantiate(View.ForgeItemVisualPrefab, View.Content);
                itemVisual.OnSelected.Subscribe(SelectItem).AddTo(Disposables);
                _listPlace.Add(itemVisual);
            }
            _currentCell = _listPlace[0];
        }

        protected override void OnLoadGame()
        {
            LoadItemRelations(_weapons, _commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Weapon).ToList());
            LoadItemRelations(_armors, _commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Armor).ToList());
            LoadItemRelations(_necklaces, _commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Amulet).ToList());
            LoadItemRelations(_boots, _commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Boots).ToList());
            OpenList(_weapons);
        }

        private void LoadItemRelations(List<GameItemRelation> relations, List<ItemModel> itemModels)
        {
            foreach (var name in SetNames)
            {
                var item = itemModels.Find(item => item.SetName == name);

                if (_commonDictionaries.ItemRelations.TryGetValue($"Recipe{item.Id}", out var relationModel))
                {
                    var result = new GameItem(_commonDictionaries.Items[relationModel.ResultItemName], 0);
                    var ingredient = new GameItem(_commonDictionaries.Items[relationModel.ItemIngredientName], 0);
                    var relation = new GameItemRelation(relationModel, ingredient, result);
                    relations.Add(relation);
                }
            }
        }

        public void OpenList(List<GameItemRelation> items)
        {
            _currentItems = items;
            for (int i = 0; i < items.Count; i++)
            {
                _listPlace[i].SetItem(items[i]);
                CheckItemCount(i);
            }

            if (_currentCell != null)
                SelectItem(_currentCell);
        }

        public void SelectItem(ForgeItemVisual selectedCell)
        {
            _currentCell?.Diselect();
            _currentItem = selectedCell.Thing;
            _currentIndex = _listPlace.FindIndex(place => place == selectedCell);
            _currentCell = selectedCell;
            _currentCell.Select();
            View.LeftItem.SetInfo(_currentItems[_currentIndex]);
            View.RightItem.SetInfo(_currentItems[_currentIndex]);
            CheckResources();
        }

        public void CheckResources()
        {
            View.ButtonSynthesis.interactable =
                _resourceStorageController.CheckResource(_currentItem.Cost)
                &&
                View.LeftItem.IsEnough;
        }

        public async UniTaskVoid SynthesisItem()
        {
            var createdCount = 0;
            var rewardModel = new RewardModel();
            var currentCount = HowManyThisItems(_currentItem.Ingredient);
            var cost = _currentItem.Cost;
            var maxCanCreateCount = currentCount / _currentItem.Model.RequireCount;

            for (var i = 1; i <= maxCanCreateCount; i++)
            {
                if (_resourceStorageController.CheckResource(cost * i))
                {
                    createdCount = i;
                }
            }

            var message = new SynthesisItemMessage { PlayerId = CommonGameData.PlayerInfoData.Id, ItemId = _currentItem.Model.ResultItemName, Count = createdCount };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var ingredients = new GameItem(_commonDictionaries.Items[_currentItem.Model.ItemIngredientName], createdCount * _currentItem.Model.RequireCount);
                _resourceStorageController.SubtractResource(cost * createdCount);
                _inventoryController.Remove(ingredients);
                _onCraft.Execute(new BigDigit(createdCount));

                var newItem = new ItemData() { Id = _currentItem.Model.ResultItemName, Amount = createdCount };
                rewardModel.Items.Add(newItem);
                var gameReward = new GameReward(rewardModel);
                _clientRewardService.ShowReward(gameReward);
                _onGetReward = _clientRewardService.OnGetReward.Subscribe(_ => Refresh());
            }
        }

        private void Refresh()
        {
            _onGetReward.Dispose();
            CheckItemCount(_currentIndex);
            CheckItemCount(_currentIndex + 1);
            CheckResources();
            SelectItem(_currentCell);
        }

        private void CheckItemCount(int index)
        {
            var item = _listPlace[index].Thing;

            //var flag = _inventoryController.GameInventory
            //    .InventoryObjects[item.Ingredient.Id]
            //    .CheckCount(_currentItem.Model.RequireCount);

            //_currentCell.Cell.SwitchDoneForUse(flag);
        }

        private int HowManyThisItems(GameItem item)
        {
            if (_inventoryController.GameInventory.InventoryObjects.TryGetValue(item.Id, out var value))
            {
                return value.Amount;
            }
            else
            {
                return 0;
            }
        }
    }
}