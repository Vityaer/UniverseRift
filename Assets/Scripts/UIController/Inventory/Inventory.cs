using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;

[System.Serializable]
public class Inventory{
   	public List<ItemController> items = new List<ItemController>();
	public List<SplinterController> splinters = new List<SplinterController>();

	public void GetAll(List<VisualAPI> list){
		list.Clear();
		foreach(ItemController controller in items) list.Add(controller as VisualAPI);
		foreach(SplinterController controller in splinters) list.Add(controller as VisualAPI);
	}
	public void GetItemAtType(TypeItem typeItems, List<VisualAPI> list){
		list.Clear();
		foreach(ItemController controller in items) if(controller.item.Type == typeItems) list.Add(controller as VisualAPI);
	}
	public void Add(ItemController itemController){
		Debug.Log("add item");
		ItemController workItem = items.Find(x => (x.item.ID == itemController.item.ID));
		if(workItem != null){
			workItem.IncreaseAmount(itemController.Amount);
		}else{
			items.Add((ItemController)itemController.Clone());
		}
	}
	public void Add(SplinterController splinterController){
		SplinterController workSplinter = splinters.Find(x => (x.splinter.ID == splinterController.splinter.ID));
		if(workSplinter != null){
			workSplinter.IncreaseAmount(splinterController.splinter.Amount);
		}else{
			splinters.Add((SplinterController) splinterController.Clone());
		}
	}
	public void Add(List<ItemController> items){
		for(int i = 0; i < items.Count; i++) Add(items[i]);
	}
	public void Add(List<SplinterController> splinters){
		for(int i = 0; i < splinters.Count; i++) Add(splinters[i]);
	}
	public void RemoveSplinter(Splinter splinerForDelete){
		SplinterController controller = splinters.Find(x => x.splinter == splinerForDelete);
		if(controller == null) Debug.Log("not found splinter for delete");
		splinters.Remove(controller);
	}

	public Inventory(InventorySave inventorySave){
		Item _item = null; Splinter _splinter = null;
		ItemsList itemsList = Resources.Load<ItemsList>("Items/ListItems");
		foreach(ItemSave item in inventorySave.listItem){
			_item = itemsList?.GetItem(item.ID);
			if(_item != null){
				items.Add(new ItemController(_item, item.Amount));
			}			
		}
		foreach(SplinterSave item in inventorySave.listSplinter){
			_splinter = SplinterSystem.Instance.GetSplinter(item.ID);
			if(_splinter != null){
				splinters.Add(new SplinterController(_splinter, item.Amount));
			}
		}
	}
	public Inventory(){}
}
