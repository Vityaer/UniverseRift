using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketResourceScript : MonoBehaviour
{
	[Header("Resource")]
	public List<MarketProduct<Resource>> resources = new List<MarketProduct<Resource>>();
	private static MarketResourceScript instance; 
	public static MarketResourceScript Instance{get => instance;} 

	public MarketProduct<Resource> GetProductFromTypeResource(TypeResource name)
	{
		return resources.Find(x => (x.subject as Resource).Name == name);
	}

	public bool GetCanSellThisResource(TypeResource name)
	{
		return (resources.Find(x => (x.subject as Resource).Name == name) != null);
	}

	void Awake()
	{ 
		if(instance == null){
			instance = this;
		}else{
			Debug.Log("MarketResourceScript twice");
		}	 
	}
}
