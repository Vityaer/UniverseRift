using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class ResourceObjectCost : MonoBehaviour{
    public Image image;
    public TextMeshProUGUI textAmount;
	private Resource costResource = null;
	private Resource storeResource; 

	public void SetData(Resource res){
		if(costResource != null)
			PlayerScript.Instance.UnRegisterOnChangeResource( CheckResource, costResource.Name );

		this.costResource = res;
		CheckResource();
		image.sprite = this.costResource.Image;
		gameObject.SetActive(true);
		PlayerScript.Instance.RegisterOnChangeResource( CheckResource, costResource.Name );
	}

	public void CheckResource(Resource res){
		CheckResource();
	}

	public bool CheckResource(){
		storeResource = PlayerScript.Instance.GetResource(costResource.Name);
		bool flag     = PlayerScript.Instance.CheckResource(costResource); 
		string result = flag ? "<color=green>" : "<color=red>";
		result = string.Concat(result, costResource.ToString(), "</color>/", storeResource.ToString());
		textAmount.text = result;
		OnCheckResource(flag);
		return flag;
	}
	public void Hide(){
		gameObject.SetActive(false);
	}
	private Action<bool> observerCanBuy;
	public void RegisterOnCanBuy(Action<bool> d){observerCanBuy += d;} 
	public void UnregisterOnCanBuy(Action<bool> d){observerCanBuy -= d;} 
	private void OnCheckResource(bool check){
		if(observerCanBuy != null)
			observerCanBuy(check);
	}
}
