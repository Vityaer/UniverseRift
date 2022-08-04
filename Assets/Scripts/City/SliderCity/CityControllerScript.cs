using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityControllerScript : MonoBehaviour{

	public SliderCityScript sliderCity;

	[Header("UI")]
	private Canvas canvasCity;
	public FooterButtonScript btnOpenClose;
	public GameObject background, cityParent;
	[Header("UI Button")]
	[SerializeField] GameObject canvasButtonsUI; 
	void Awake(){
		canvasCity = GetComponent<Canvas>();
		btnOpenClose.RegisterOnChange(Change);
	}
	void Change(bool isOpen){
		if(isOpen){ Open(); }else{ Close(); }
	}
	public void Open(){
		Debug.Log("open city");
		canvasCity.enabled = true;
		sliderCity.enabled = true;
		canvasButtonsUI.SetActive(true);
		cityParent.SetActive(true);
		BackGroundControllerScript.Instance.OpenBackground(background);
	}
	public void Close(){
		canvasCity.enabled = false;
		sliderCity.enabled = false;	
		cityParent.SetActive(false);
		canvasButtonsUI.SetActive(false);
	}
}
