using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif

public class Forge : Building
{
    [Header("General")]
    public ForgeItemVisual leftItem, rightItem;
    [SerializeField] private List<ForgeItemVisual> listPlace = new List<ForgeItemVisual>();
    private List<ItemSynthesis> workList;
    [Header("Data")]
    [SerializeField] private List<ItemSynthesis> weapons, armors, necklace, boots;
    private ItemsList itemsList;
    [Header("Costs")]
    [SerializeField] private List<Resource> listCostItems = new List<Resource>();

    public Button WeaponPanelButton;
    public Button ArmorPanelButton;
    public Button BootsPanelButton;
    public Button AmuletPanelButton;
    public Button btnSynthesis;

    private ItemSynthesis currentItem;
    private ForgeItemVisual currentCell = null;

    private static Forge instance;
    public static Forge Instance { get => instance; }

    void Awake()
    {
        instance = this;
    }

    protected override void OnStart()
    {
        WeaponPanelButton.onClick.AddListener(() => OpenList(TypeSynthesis.Weapon));
        ArmorPanelButton.onClick.AddListener(() => OpenList(TypeSynthesis.Armor));
        BootsPanelButton.onClick.AddListener(() => OpenList(TypeSynthesis.Boots));
        AmuletPanelButton.onClick.AddListener(() => OpenList(TypeSynthesis.Amulet));
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
        leftItem.SetItem(item.requireItem, item.countRequireItem);
        rightItem.SetResource(item.requireResource);
        CheckDemands();
    }

    public void CheckDemands()
    {
        btnSynthesis.interactable = GameController.Instance.CheckResource(currentItem.requireResource) && InventoryController.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem);
        RecalculateDemands();
    }

    private void RecalculateDemands()
    {
        leftItem.forgeItemCost.CheckItems();
        rightItem.resourceCost.CheckResource();
    }

    public void MakeItem()
    {
        int createdCount = 0;
        Reward reward = new Reward();
        if (currentItem != null)
        {
            while (GameController.Instance.CheckResource(currentItem.requireResource) && InventoryController.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem))
            {
                GameController.Instance.SubtractResource(currentItem.requireResource);
                InventoryController.Instance.RemoveItems(currentItem.requireItem, currentItem.countRequireItem);
                OnCraft(1);
                createdCount += 1;
            }
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

[System.Serializable]
public class ItemSynthesis
{
    [Header("Require")]
    public string IDRequireItem;
    public int countRequireItem;
    public Resource requireResource;

    [Header("Reward")]
    public string IDReward;

    private Item _reward = null;
    public Item reward
    {
        get
        {
            if (_reward == null) _reward = GetItem(IDReward);
            return _reward;
        }
    }
    private Item _requireItem = null;
    public Item requireItem
    {
        get
        {
            if (_requireItem == null) _requireItem = GetItem(IDRequireItem);
            return _requireItem;
        }
    }
    private Item GetItem(string ID)
    {
        return Item.GetItem(ID);
    }

}
public enum TypeSynthesis
{
    Weapon = 1,
    Boots = 2,
    Armor = 3,
    Amulet = 4,
    Shield = 5,
    Helmet = 6,
    Mittens = 7
}