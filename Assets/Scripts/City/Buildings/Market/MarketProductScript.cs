using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public class MarketProductScript : MonoBehaviour{
	public ButtonCostScript buttonCost;
	[OdinSerialize] private MarketProduct marketProduct;
	public SubjectCellControllerScript cellProduct;
	[SerializeField] private GameObject soldOutPanel;
	public void SetData(MarketProduct<Resource> product){
		marketProduct = product;
		buttonCost.UpdateCost(product.cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
	}
	public void SetData(MarketProduct<Item> product){
		marketProduct = product;
		buttonCost.UpdateCost(product.cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
	}
	public void SetData(MarketProduct<Splinter> product){
		marketProduct = product;
		buttonCost.UpdateCost(product.cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
	}
	public void UpdateUI(){
		if(marketProduct.CountLeftProduct == marketProduct.CountMaxProduct){
			buttonCost.Disable();
			soldOutPanel.SetActive(true);
		}else{
			soldOutPanel.SetActive(false);
			buttonCost.EnableButton();
		}
	}
	public void Recovery(){
		marketProduct.Recovery();
		UpdateUI();
	}
    public void Buy(int count = 1){
		if((count + marketProduct.CountLeftProduct) > marketProduct.CountMaxProduct) count = marketProduct.CountMaxProduct - marketProduct.CountLeftProduct;
		marketProduct.GetProduct(count);
		UpdateUI();
	}
	public void Hide(){
		gameObject.SetActive(false);
	}
}
