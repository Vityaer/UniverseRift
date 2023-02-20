using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class InventoryControllerScript : MonoBehaviour{

	[SerializeField] private Canvas canvasInventory;
	private TypeItem typeItems;
	public Transform grid;
	private CellItemHeroScript cellItem      = null;
	private ItemController selectItem = null;
	[Header("Panel Inventory")]
	public GameObject panelInventory;
	private bool isOpenInventory = false;
	public GameObject panelController;
	[Header("Info panel")]
	public PanelInfoItemScript panelInfoItem;
	public PanelInfoSplinterScript panelInfoSplinter;

	private Inventory inventory = new Inventory();
	private List<VisualAPI> listForVisual = new List<VisualAPI>();

	[SerializeField]private List<SubjectCellControllerScript> cells = new List<SubjectCellControllerScript>();
	private static InventoryControllerScript instance;
	public static InventoryControllerScript Instance{get => instance;}
	
	void Awake()
	{
		instance = this;
		GetCells();
	}

	private void GetCells()
	{
		if(cells.Count == 0)
			foreach(Transform cell in grid)
				cells.Add(cell.GetComponent<SubjectCellControllerScript>());
	}

	private void FillGrid(List<VisualAPI> list)
	{
		for(int i = 0; i < list.Count; i++){
			cells[i].SetItem(list[i]);
		}
		for(int i = list.Count; i < cells.Count; i++){
			cells[i].Clear();
		}
	}

	private void FillGrid(List<ItemController> filterItems)
	{
		for(int i = 0; i < filterItems.Count; i++){
			cells[i].SetItem(filterItems[i]);
		}
		for(int i = filterItems.Count; i < cells.Count; i++){
			cells[i].Clear();
		}
	}

//API
	//Invenotory
	public int HowManyThisItems(Item item)
	{
		ItemController workItem = inventory.items.Find(x => (x.item.ID == item.ID));
		return (workItem != null) ? workItem.Amount : 0;
	}

	public bool CheckItems(Item item, int count = 1)
	{
		bool result = false;
		ItemController workItem = inventory.items.Find(x => (x.item.ID == item.ID));
		if(workItem != null)
			if(workItem.Amount >= count) result = true;
		return result;
	}

	public void RemoveItems(Item item, int count = 1)
	{
		ItemController workItem = inventory.items.Find(x => (x?.item.ID == item.ID));
		if(workItem != null){
			workItem.DecreaseAmount(count);
			if(workItem.Amount == 0) inventory.items.Remove(workItem);
		}
	}

	public void AddItem(Item item)
	{
		inventory.Add(new ItemController(item, 1));
	}
	
	public void AddItem(ItemController itemController){ inventory.Add(itemController); }
	public void AddItems(List<ItemController> Items){ inventory.Add(Items); }
	public void AddItems(List<Item> Items){ 
		foreach(Item item in Items) AddItem(new ItemController(item)); }
	public void AddSplinter(SplinterController splinterController){ inventory.Add(splinterController); }
	public void AddSplinters(List<SplinterController> splinters){ inventory.Add(splinters); }
	public void AddSplinters(List<Splinter> splinters){
		foreach(Splinter splinter in splinters) AddSplinter(new SplinterController(splinter));}
	public void RemoveSplinter(Splinter splinterForDelete){ inventory.RemoveSplinter(splinterForDelete); }	
	public void SelectItem(){
		if(cellItem != null){
			cellItem.SetItem(selectItem.item);
			selectItem.DecreaseAmount( 1 );
			if(selectItem.Amount <= 0) inventory.items.Remove(selectItem);
			cellItem = null;
			selectItem = null;
		}
		CloseAll();
	}
	public void DropItem(){
		panelInfoItem.Close();
		inventory.items.Remove(selectItem);
		inventory.GetAll(listForVisual);
		FillGrid(listForVisual);
	}
	public void DropSplinter(SplinterController splinter){
		panelInfoItem.Close();
		inventory.splinters.Remove(splinter);
		inventory.GetAll(listForVisual);
		FillGrid(listForVisual);
	}
	public void Open(){
		OpenAllItem();
		canvasInventory.enabled = true;
		panelInventory.SetActive(true);
		isOpenInventory = true;
	}
	public void OpenAllItem(){
		inventory.GetAll(listForVisual);
		FillGrid(listForVisual);
		panelController.SetActive(true);
	}
	public void Refresh(){
		inventory.GetAll(listForVisual);
		FillGrid(listForVisual);
	}
	public void Open(string str){
		TypeItem curType = (TypeItem) Enum.Parse(typeof(TypeItem), str); 
		Open(curType, cellItem: null);
	}
	public void Open(TypeItem typeItems, CellItemHeroScript cellItem = null){
		this.cellItem = cellItem;
		inventory.GetItemAtType(typeItems, listForVisual);
		FillGrid(listForVisual);
		canvasInventory.enabled = true;
		panelController.SetActive( (this.cellItem == null) ? true : false );
		panelInventory.SetActive(true);
		isOpenInventory = true;
	}
	public void OpenInfoItem(ItemController itemController){
		selectItem = itemController;
		panelInfoItem.OpenInfoAboutItem(itemController.item, this.cellItem);
	}
	public void OpenInfoItem(Item item){
		panelInfoItem.OpenInfoAboutItem(item, this.cellItem);
	}
	public void OpenInfoItem(Resource res){
		panelInfoItem.OpenInfoAboutItem(res);
	}
	public void OpenInfoItem(Item item, TypeItem typeItems, CellItemHeroScript cellItem){
		// canvasInventory.enabled = true;
		this.cellItem = cellItem;
		panelInfoItem.OpenInfoAboutItem(item, this.cellItem, onHero: true);
	}
	SplinterController selectSplinter;
	public void OpenInfoItem(Splinter splinter){ OpenInfoItem(new SplinterController(splinter), withControl : false); }
	public void OpenInfoItem(SplinterController splinterController, bool withControl){
		selectSplinter = splinterController;
		panelInfoSplinter.OpenInfoAboutSplinter(splinterController, withControl);
	}
	[ContextMenu("Close")]
	public void Close(){
		panelInfoItem.Close();
		panelInfoSplinter.Close();
		if(isOpenInventory){
			panelInventory.SetActive(false);
			isOpenInventory = false;
		}
		canvasInventory.enabled = false;
		cellItem = null;
	}
	public void CloseAll(){
		panelInfoItem.Close();
		panelInventory.SetActive(false);
		isOpenInventory = false;
		canvasInventory.enabled = false;
	}

	void Start(){
		LoadInformation();
		inventory.RegisterOnChange(Refresh);
	}

	void OnApplicationPause(bool pauseStatus){
		#if UNITY_ANDROID && !UNITY_EDITOR
		SaveLoadControllerScript.SaveInventory(inventory);
		#endif
	}
	void OnDestroy(){
		SaveLoadControllerScript.SaveInventory(inventory);
		inventory.UnregisterOnChange(Refresh);
	}
	void LoadInformation(){
		inventory = SaveLoadControllerScript.LoadInventory();
	}
}
