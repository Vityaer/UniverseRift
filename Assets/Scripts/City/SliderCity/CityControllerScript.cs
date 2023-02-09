using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityControllerScript : MainPage{

	public SliderCityScript sliderCity;

	[Header("UI")]
	private Canvas canvasCity;
	public GameObject background, cityParent;
	[Header("UI Button")]
	[SerializeField] GameObject canvasButtonsUI; 

	protected override void Awake()
	{
		canvasCity = GetComponent<Canvas>();
		base.Awake();
	}

	public override void Open()
	{
		Debug.Log("city open");

		base.Open();
		sliderCity.enabled = true;
		canvasButtonsUI.SetActive(true);
		cityParent.SetActive(true);
		BackGroundControllerScript.Instance.OpenBackground(background);
	}

	public override void Close()
	{
		Debug.Log("city close");
		sliderCity.enabled = false;	
		cityParent.SetActive(false);
		canvasButtonsUI.SetActive(false);
	}
}
