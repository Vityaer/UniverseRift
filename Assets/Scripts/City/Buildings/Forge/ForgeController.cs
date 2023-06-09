using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using City.Buildings.General;
using Common.Resourses;
using Common;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards;
using MainScripts;
using UniRx;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif

namespace City.Buildings.Forge
{
    public class ForgeController : Building
    {
        public ForgeItemVisual LeftItem, RightItem;
        private List<ItemSynthesis> workList;

        [SerializeField] private List<ItemSynthesis> weapons, armors, necklace, boots;
        [SerializeField] private List<ForgeItemVisual> listPlace = new List<ForgeItemVisual>();
        [SerializeField] private List<Resource> listCostItems = new List<Resource>();

        [SerializeField] private Button _weaponPanelButton;
        [SerializeField] private Button _armorPanelButton;
        [SerializeField] private Button _bootsPanelButton;
        [SerializeField] private Button _amuletPanelButton;
        [SerializeField] private Button _buttonSynthesis;

        private ItemsList itemsList;
        private ItemSynthesis currentItem;
        private ForgeItemVisual currentCell = null;

        public static ForgeController Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        protected override void OnStart()
        {
            _weaponPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(TypeSynthesis.Weapon));
            _armorPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(TypeSynthesis.Armor));
            _bootsPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(TypeSynthesis.Boots));
            _amuletPanelButton.OnClickAsObservable().Subscribe(_ => OpenList(TypeSynthesis.Amulet));
            _buttonSynthesis.OnClickAsObservable().Subscribe(_ => SynthesisItem());
            OpenList(TypeSynthesis.Weapon);
            SelectItem(listPlace[0], listPlace[0].Thing);
        }

        public void OpenList(TypeSynthesis type)
        {
            switch (type)
            {
                case TypeSynthesis.Weapon:
                    workList = weapons;
                    break;
                case TypeSynthesis.Armor:
                    workList = armors;
                    break;
                case TypeSynthesis.Amulet:
                    workList = necklace;
                    break;
                case TypeSynthesis.Boots:
                    workList = boots;
                    break;
            }

            for (int i = 0; i < workList.Count; i++)
            {
                listPlace[i].SetItem(workList[i]);
                // listPlace[i].UIItem.SwitchDoneForUse( InventoryControllerScript.Instance.HowManyThisItems( workList[i].requireItem) >= workList[i].countRequireItem );
            }
            if (currentCell != null)
                SelectItem(currentCell, currentCell.Thing);

            currentCell?.UIItem.Select();
        }


        public void SelectItem(ForgeItemVisual selectedCell, ItemSynthesis item)
        {
            currentCell?.UIItem.Diselect();
            currentItem = item;
            currentCell = selectedCell;
            LeftItem.SetItem(item.requireItem, item.countRequireItem);
            RightItem.SetResource(item.requireResource);
            CheckDemands();
        }

        public void CheckDemands()
        {
            _buttonSynthesis.interactable = GameController.Instance.CheckResource(currentItem.requireResource) && InventoryController.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem);
            RecalculateDemands();
        }

        private void RecalculateDemands()
        {
            LeftItem.forgeItemCost.CheckItems();
            RightItem.resourceCost.CheckResource();
        }

        public void SynthesisItem()
        {
            int createdCount = 0;
            Reward reward = new Reward();

            while (GameController.Instance.CheckResource(currentItem.requireResource) && InventoryController.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem))
            {
                GameController.Instance.SubtractResource(currentItem.requireResource);
                InventoryController.Instance.RemoveItems(currentItem.requireItem, currentItem.countRequireItem);
                OnCraft(1);
                createdCount += 1;
            }

            Item item = (Item)currentItem.reward.Clone();
            item.Amount = createdCount;
            reward.AddItem(item);
            MessageController.Instance.OpenSimpleRewardPanel(reward);
            currentCell.UIItem.SwitchDoneForUse(InventoryController.Instance.HowManyThisItems(currentItem.requireItem) >= currentItem.countRequireItem);
            CheckNextItem();
            CheckDemands();
        }

        private void CheckNextItem()
        {
            int num = listPlace.FindIndex(x => x == currentCell) + 1;
            if (num < workList.Count)
                listPlace[num].UIItem.SwitchDoneForUse(InventoryController.Instance.HowManyThisItems(workList[num].requireItem) >= workList[num].countRequireItem);
        }

        //Observers
        private Action<BigDigit> observerCraft;
        public void RegisterOnCraft(Action<BigDigit> d) { observerCraft += d; }
        private void OnCraft(int amount) { if (observerCraft != null) observerCraft(new BigDigit(amount)); }
        //Editor
#if UNITY_EDITOR_WIN
        [ContextMenu("Set All Costs")]
        public void SetAllCosts()
        {
            Undo.RecordObject(this, "forgeUpdateCosts");
            SetCosts(weapons);
            SetCosts(armors);
            SetCosts(necklace);
            SetCosts(boots);
        }
        void SetCosts(List<ItemSynthesis> listItems)
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                listItems[i].requireResource = listCostItems[i];
            }
        }
#endif
    }
}