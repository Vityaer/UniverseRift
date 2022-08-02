using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CalculatedReward{
	[SerializeField] private ListResource listResource = new ListResource();
	public ListResource GetListResource{get => listResource;}
	[SerializeField] private List<ItemController> items = new List<ItemController>();
	public List<ItemController> GetItems{get => items;}
	[SerializeField] private List<SplinterController> splinters = new List<SplinterController>();
	public List<SplinterController> GetSplinters{get => splinters;}
	public int AllCount{get => listResource.Count + GetItems.Count + GetSplinters.Count;}
	public CalculatedReward Clone(){
		ListResource listRes = (ListResource) listResource.Clone();
		List<ItemController> items = new List<ItemController>();
		List<SplinterController> splinters = new List<SplinterController>();
		foreach(ItemController item in this.items){ items.Add((ItemController) item.Clone());}
		foreach(SplinterController splinter in this.splinters){ splinters.Add((SplinterController) splinter.Clone());}
		return new CalculatedReward(listRes, items, splinters);
	}
	public CalculatedReward(){}
	public CalculatedReward(ListResource listRes, List<ItemController> items, List<SplinterController> splinters){
		this.listResource = listRes;
		this.items = items;
		this.splinters = splinters;
	}
}