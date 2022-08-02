using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using ObjectSave;
public class MarketScript : Building{
	public TypeMarket typeMarket;
	public Transform showcase;
	[Header("Products")]
	[OdinSerialize] private List<MarketProduct> productsForSale = new List<MarketProduct>();
	[SerializeField] private List<MarketProductScript> productControllers = new List<MarketProductScript>(); 
	public CycleRecover cycle;
	protected override void OnStart(){
		if(productControllers.Count == 0) GetCells();
		TimeControllerSystem.Instance.RegisterOnNewCycle(RecoveryAllProducts, cycle);
	}
	protected override void OnLoadGame(){
		List<MarketProductSave> saveProducts = PlayerScript.GetPlayerGame.GetProductForMarket(typeMarket);
		MarketProduct currentProduct = null;
		foreach(MarketProductSave product in saveProducts){
			currentProduct = productsForSale.Find(x => x.ID == product.ID);
			if(currentProduct != null) currentProduct.UpdateData(product.countSell);
		}
		if(productFill) UpdateUIProducts();
	}
	private void UpdateUIProducts(){
		foreach(MarketProductScript product in productControllers){ product.UpdateUI(); } 
	}
	private bool productFill = false;
	protected override void OpenPage(){
		for(int i=0; i < productsForSale.Count; i++){
			switch(productsForSale[i]){
				case MarketProduct<Resource> product:
					productControllers[i].SetData(product);
					break;
				case MarketProduct<Item> product:
					productControllers[i].SetData(product);
					break;
				case MarketProduct<Splinter> product:
					productControllers[i].SetData(product);
					break;		

			}
		}
		for(int i = productsForSale.Count; i < productControllers.Count; i++){
			productControllers[i].Hide();
		}
		productFill = true;
	}
	private void GetCells(){
		foreach(Transform child in showcase)
			productControllers.Add(child.GetComponent<MarketProductScript>());
	}
	private void RecoveryAllProducts(){if(productFill) foreach(MarketProductScript product in productControllers){ product.Recovery(); } }
	
	public void NewSellProduct(int IDproduct, int newCountSell){ PlayerScript.GetPlayerGame.NewDataAboutSellProduct(typeMarket, IDproduct, newCountSell); }

	[Button] public void AddResource(){ productsForSale.Add(new MarketProduct<Resource>()); }
	[Button] public void AddSplinter(){ productsForSale.Add(new MarketProduct<Splinter>()); }
	[Button] public void AddItem(){ productsForSale.Add(new MarketProduct<Item>()); }
}

public enum TypeMarket{
	MainMarket = 0
}
