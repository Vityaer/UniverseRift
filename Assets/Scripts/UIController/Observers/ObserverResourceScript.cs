using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ObserverResourceScript : MonoBehaviour{
	[Header("General")]
	public TypeResource typeResource;
	private bool         isMyabeBuy;
	public int          cost;
	private Resource resource;
	[Header("UI")]
	public GameObject btnAddResource;
	public Image imageResource;
	public TextMeshProUGUI countResource;
	void Start(){
		isMyabeBuy = MarketResourceScript.Instance.GetCanSellThisResource(typeResource);
		resource             = new Resource(typeResource);
		imageResource.sprite = resource.Image;
		btnAddResource.SetActive(isMyabeBuy);
		PlayerScript.Instance.RegisterOnChangeResource(UpdateUI, typeResource);
		UpdateUI(PlayerScript.Instance.GetResource(typeResource));
	}

	public void UpdateUI(Resource res){
		resource = res;
		countResource.text = resource.ToString();
	}
	public void OpenPanelForBuyResource(){
		MarketProduct<Resource> product = null;
		product = MarketResourceScript.Instance.GetProductFromTypeResource(resource.Name);
		if(product != null)
			PanelBuyResourceScript.StandartPanelBuyResource.Open(
				product.subject, product.cost
				);
	}
	void OnDestroy(){
		PlayerScript.Instance.UnRegisterOnChangeResource(UpdateUI, typeResource);
	}
}
