using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelBuyResourceScript : MonoBehaviour{
	[SerializeField] private GameObject panel;
	public SubjectCellControllerScript cellProduct;
	public CountControllerScript countController;
	public ButtonWithObserverResource buttonBuy;
	public bool standartPanel = true;
	void Awake(){
		if(standartPanel) standartPanelBuyResource = this;
	}
	void Start(){
		countController.RegisterOnChangeCount(ChangeCount);
	}
	private Resource product, cost;
	public void Open(Resource product, Resource cost){
		this.cost    = cost;
		this.product = product;
		ChangeCount(count: countController.MinCount);
		cellProduct.SetItem(product);
		panel.SetActive(true);
	}
	public void Close(){	
		panel.SetActive(false);
	}
	private void ChangeCount(int count){
		buttonBuy.ChangeCost(cost * count);
	}
	public void Buy(){
		int count = countController.Count;
		if(PlayerScript.Instance.CheckResource(cost * count)){
			PlayerScript.Instance.SubtractResource(cost * count);
			PlayerScript.Instance.AddResource(product * count);
		}
	}
	private static PanelBuyResourceScript standartPanelBuyResource;
	public static PanelBuyResourceScript StandartPanelBuyResource{get => standartPanelBuyResource;}
}