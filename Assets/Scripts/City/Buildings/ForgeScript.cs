using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif
public class ForgeScript : Building{

	[Header("General")]
	public ForgeItemVisual leftItem, rightItem;
	[SerializeField] private List<ForgeItemVisual> listPlace = new List<ForgeItemVisual>();
	private List<ItemSynthesis> workList;
	[Header("Data")]
	[SerializeField] private List<ItemSynthesis> weapons, armors, necklace, shield, boots, helmets, mittens;
    private ItemsList itemsList;
    [Header("Costs")]
    [SerializeField] private List<Resource> listCostItems = new List<Resource>();


	protected override void OpenPage(){
		OpenList(TypeSynthesis.Weapon);
	}
	public void OpenList(int numList){
		OpenList((TypeSynthesis) numList);
	}
	public void OpenList(TypeSynthesis type){
		switch(type){
			case TypeSynthesis.Weapon:
				workList = weapons;
				break;
			case TypeSynthesis.Armor:
				workList = armors;
				break;
			case TypeSynthesis.Necklace:
				workList = necklace;
				break;
			case TypeSynthesis.Shield:
				workList = shield;
				break;
			case TypeSynthesis.Boots:
				workList = boots;
				break;
			case TypeSynthesis.Mittens:
				workList = mittens;
				break;
			case TypeSynthesis.Helmet:
				workList = helmets;
				break;						
		}
		for(int i=0; i < workList.Count; i++){
			listPlace[i].SetItem(workList[i]);
			// listPlace[i].UIItem.SwitchDoneForUse( InventoryControllerScript.Instance.HowManyThisItems( workList[i].requireItem) >= workList[i].countRequireItem );
		}
		if(currentCell != null) SelectItem(currentCell, currentCell.Thing);
	}

	private ForgeItemVisual currentCell = null;
	private ItemSynthesis currentItem;
	public void SelectItem(ForgeItemVisual selectedCell, ItemSynthesis item){
		currentItem = item;
		if(currentCell != null) currentCell.UIItem.Diselect();
		currentCell = selectedCell;
		leftItem.SetItem(item.requireItem, item.countRequireItem);
		rightItem.SetResource(item.requireResource);
		CheckDemands();
	}

	public Button btnSynthesis;
	public void CheckDemands(){
		if(currentItem != null){
			btnSynthesis.interactable =  PlayerScript.Instance.CheckResource(currentItem.requireResource) &&  InventoryControllerScript.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem);
			RecalculateDemands();
		}
	}
	private void RecalculateDemands(){
		leftItem.forgeItemCost.CheckItems();
		rightItem.resourceCost.CheckResource();
	}

	public void MakeItem(){
		if(currentItem != null){
			while(PlayerScript.Instance.CheckResource(currentItem.requireResource) && InventoryControllerScript.Instance.CheckItems(currentItem.requireItem, currentItem.countRequireItem) ){
				PlayerScript.Instance.SubtractResource(currentItem.requireResource);
				InventoryControllerScript.Instance.RemoveItems(currentItem.requireItem, currentItem.countRequireItem);
				InventoryControllerScript.Instance.AddItem(currentItem.reward);
				OnCraft(1);
			}	
		}
		currentCell.UIItem.SwitchDoneForUse( InventoryControllerScript.Instance.HowManyThisItems( currentItem.requireItem) >= currentItem.countRequireItem );
		CheckNextItem();
		CheckDemands();
	}
	private void CheckNextItem(){
		int num = listPlace.FindIndex(x => x == currentCell) + 1;
		if(num < workList.Count)
			listPlace[num].UIItem.SwitchDoneForUse( InventoryControllerScript.Instance.HowManyThisItems( workList[num].requireItem) >= workList[num].countRequireItem );
	}

	private static ForgeScript instance;
	public static ForgeScript Instance{get => instance;}
	void Awake(){
		instance = this;
	}



//Observers
	private Action<BigDigit> observerCraft;
	public void RegisterOnCraft(Action<BigDigit> d){observerCraft += d;}	 
	private void OnCraft(int amount) {if(observerCraft != null) observerCraft(new BigDigit(amount));}	 
//Editor
	#if UNITY_EDITOR_WIN
	[ContextMenu("Set All Costs")]
	public void SetAllCosts(){
		Undo.RecordObject(this, "forgeUpdateCosts");
		SetCosts(weapons);
		SetCosts(armors);
		SetCosts(necklace);
		SetCosts(shield);
		SetCosts(boots);
		SetCosts(helmets);
		SetCosts(mittens);
	}
	void SetCosts(List<ItemSynthesis> listItems){
		for(int i = 0; i < listItems.Count; i++){
			listItems[i].requireResource = listCostItems[i];
		}
	}
	#endif
}

[System.Serializable]
public class ItemSynthesis{
	[Header("Require")]
	public int IDRequireItem;
	public int countRequireItem;
	public Resource requireResource;
	[Header("Reward")]
	public int IDReward;

    private Item _reward = null; 
    public Item reward{ 
    	get {
    		if(_reward == null) _reward = GetItem(IDReward);
    		return _reward;
    	}
    }
    private Item _requireItem = null; 
    public Item requireItem{
    	get{
    		if(_requireItem == null) _requireItem = GetItem(IDRequireItem);
    		return _requireItem;
    	}
    }
    private Item GetItem(int ID){
		return Item.GetItem(ID);
    }

}
public enum TypeSynthesis{
	Weapon = 1,
	Boots = 2,
	Armor = 3,
	Necklace = 4,
	Shield = 5,
	Helmet = 6,
	Mittens = 7
}