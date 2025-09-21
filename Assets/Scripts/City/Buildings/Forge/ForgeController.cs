using System;
using System.Collections.Generic;
using System.Linq;
using City.Buildings.Abstractions;
using City.Panels.Inventories;
using City.Panels.SubjectPanels.Items;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Models;
using Models.Common.BigDigits;
using Models.Data.Inventories;
using Models.Items;
using Network.DataServer;
using Network.DataServer.Messages.Items;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.ItemVisual.Forges;
using UIController.Rewards;
using UniRx;
using VContainer;

namespace City.Buildings.Forge
{
    public class ForgeController : BaseBuilding<ForgeView>
    {
        private const int ITEMS_COUNT = 16;

        private readonly List<string> m_setNames = new List<string>()
        {
            "Pupil", "Peasant", "Militiaman", "Monk", "Warrior", "Feller", "Soldier", "Minotaur", "Demon", "Druid",
            "Obedient", "Devil", "Destiny", "Archangel", "Titan", "God"
        };

        [Inject] private readonly InventoryPanelController m_inventoryPanelController;
        [Inject] private readonly ItemPanelController m_itemPanelController;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;

        private readonly List<ForgeItemVisual> m_listPlace = new List<ForgeItemVisual>();
        private List<GameResource> m_listCostItems = new List<GameResource>();

        private readonly List<GameItemRelation> m_weapons = new List<GameItemRelation>();
        private readonly List<GameItemRelation> m_armors = new List<GameItemRelation>();
        private readonly List<GameItemRelation> m_necklaces = new List<GameItemRelation>();
        private readonly List<GameItemRelation> m_boots = new List<GameItemRelation>();
        private List<GameItemRelation> m_currentItems;

        private int m_currentIndex;
        private GameItemRelation m_currentItem;
        private ForgeItemVisual m_currentCell = null;
        private readonly ReactiveCommand<BigDigit> m_onCraft = new ReactiveCommand<BigDigit>();
        private IDisposable m_onGetReward;

        public IObservable<BigDigit> OnCraft => m_onCraft;

        protected override void OnStart()
        {
            View.WeaponPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(m_weapons)).AddTo(Disposables);
            View.ArmorPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(m_armors)).AddTo(Disposables);
            View.BootsPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(m_boots)).AddTo(Disposables);
            View.AmuletPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(m_necklaces)).AddTo(Disposables);
            View.ButtonSynthesis.OnClickAsObservable().Subscribe(_ => SynthesisItem().Forget()).AddTo(Disposables);

            for (int i = 0; i < ITEMS_COUNT; i++)
            {
                var itemVisual = UnityEngine.Object.Instantiate(View.ForgeItemVisualPrefab, View.Content);
                itemVisual.OnSelected.Subscribe(SelectItem).AddTo(Disposables);
                m_listPlace.Add(itemVisual);
            }

            View.LeftItem.SubjectCell.OnSelect.Subscribe(OpenDetails);
            View.RightItem.SubjectCell.OnSelect.Subscribe(OpenDetails);

            m_currentCell = m_listPlace[0];
            base.OnStart();
        }

        private void OpenDetails(SubjectCell cell)
        {
            m_itemPanelController.ShowData(cell.Subject as GameItem);
        }

        protected override void OnLoadGame()
        {
            LoadItemRelations(m_weapons,
                m_commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Weapon).ToList());
            LoadItemRelations(m_armors,
                m_commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Armor).ToList());
            LoadItemRelations(m_necklaces,
                m_commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Amulet).ToList());
            LoadItemRelations(m_boots,
                m_commonDictionaries.Items.Values.Where(item => item.Type == ItemType.Boots).ToList());
            OpenList(m_weapons);
        }

        public override void OnShow()
        {
            base.OnShow();
            View.GridOverrider.RecalculateGridSize();
        }

        private void LoadItemRelations(List<GameItemRelation> relations, List<ItemModel> itemModels)
        {
            foreach (string name in m_setNames)
            {
                var item = itemModels.Find(item => item.SetName == name);

                if (m_commonDictionaries.ItemRelations.TryGetValue($"Recipe{item.Id}", out var relationModel))
                {
                    var result = new GameItem(m_commonDictionaries.Items[relationModel.ResultItemName], 0);
                    var ingredient = new GameItem(m_commonDictionaries.Items[relationModel.ItemIngredientName], 0);
                    var relation = new GameItemRelation(relationModel, ingredient, result);
                    relations.Add(relation);
                }
            }
        }

        private void OpenList(List<GameItemRelation> items)
        {
            m_currentItems = items;
            for (int i = 0; i < items.Count; i++)
            {
                m_listPlace[i].SetItem(items[i]);
                CheckItemCount(i);
            }

            if (m_currentCell != null)
                SelectItem(m_currentCell);
        }

        private void SelectItem(ForgeItemVisual selectedCell)
        {
            m_currentCell?.Diselect();
            m_currentItem = selectedCell.Thing;
            m_currentIndex = m_listPlace.FindIndex(place => place == selectedCell);
            m_currentCell = selectedCell;
            m_currentCell.Select();
            View.LeftItem.SetInfo(m_currentItems[m_currentIndex]);
            View.RightItem.SetInfo(m_currentItems[m_currentIndex]);
            CheckResources();
        }

        private void CheckResources()
        {
            View.ButtonSynthesis.interactable =
                m_resourceStorageController.CheckResource(m_currentItem.Cost)
                &&
                View.LeftItem.IsEnough;
        }

        private async UniTaskVoid SynthesisItem()
        {
            int createdCount = 0;
            var rewardModel = new RewardModel();
            int currentCount = HowManyThisItems(m_currentItem.Ingredient);
            var cost = m_currentItem.Cost;
            int maxCanCreateCount = currentCount / m_currentItem.Model.RequireCount;

            for (int i = 1; i <= maxCanCreateCount; i++)
                if (m_resourceStorageController.CheckResource(cost * i))
                    createdCount = i;

            var message = new SynthesisItemMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id, ItemId = m_currentItem.Model.ResultItemName,
                Count = createdCount
            };
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var ingredients = new GameItem(m_commonDictionaries.Items[m_currentItem.Model.ItemIngredientName],
                    createdCount * m_currentItem.Model.RequireCount);
                m_resourceStorageController.SubtractResource(cost * createdCount);
                m_inventoryPanelController.Remove(ingredients);
                m_onCraft.Execute(new BigDigit(createdCount));

                var newItem = new ItemData() { Id = m_currentItem.Model.ResultItemName, Amount = createdCount };
                rewardModel.Items.Add(newItem);
                var gameReward = new GameReward(rewardModel, m_commonDictionaries);
                m_clientRewardService.ShowReward(gameReward);
                m_onGetReward = m_clientRewardService.OnGetReward.Subscribe(_ => Refresh());
            }
        }

        private void Refresh()
        {
            m_onGetReward.Dispose();
            CheckItemCount(m_currentIndex);
            CheckItemCount(m_currentIndex + 1);
            CheckResources();
            SelectItem(m_currentCell);
        }

        private void CheckItemCount(int index)
        {
            var item = m_listPlace[index].Thing;

            //var flag = _inventoryController.GameInventory
            //    .InventoryObjects[item.Ingredient.Id]
            //    .CheckCount(_currentItem.Model.RequireCount);

            //_currentCell.Cell.SwitchDoneForUse(flag);
        }

        private int HowManyThisItems(GameItem item)
        {
            return m_inventoryPanelController.GameInventory.InventoryObjects.TryGetValue(item.Id, out var value)
                ? value.Amount
                : 0;
        }
    }
}