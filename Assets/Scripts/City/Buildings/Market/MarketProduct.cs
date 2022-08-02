using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[System.Serializable]
public class MarketProduct{
	public Resource cost;
	[Min(1)][SerializeField] private int id;
	[Min(1)][SerializeField] private  int countMaxProduct = 1;
	private int countLeftProduct = 0;


	public int ID{get => id;} 
	public int CountMaxProduct{get => countMaxProduct;}
	public int CountLeftProduct{get => countLeftProduct;}
	protected void AddCountLeftProduct(int count = 1){ countLeftProduct += count; }
	public void UpdateData(int newCountLeftProduct = 1){ this.countLeftProduct = newCountLeftProduct; }
	public void Recovery(){ countLeftProduct = 0; }
	public virtual void GetProduct(int count){}
}

[System.Serializable]
public class MarketProduct<T>: MarketProduct where T: BaseObject{
	[OdinSerialize] public T subject;
	public override void GetProduct(int count){
		AddCountLeftProduct(count);
		switch(subject){
			case Resource product:
				PlayerScript.Instance.AddResource(product * count);
				break;
			case Item product:
				InventoryControllerScript.Instance.AddItem(product);
				break;
			case Splinter product:
				Debug.Log("write add splinter here");
				// InventoryControllerScript.Instance.AddSplinter(product);
				break;
		}
	}
	
}
public enum CycleRecover{
	Day,
	Week,
	Month,
	Never
}