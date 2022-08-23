using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationWithBuildings : MonoBehaviour{
	public GameObject location;
	public TypeLocation typeBackground;
	public Building buildingCanvas;
	public virtual void Open(){
		Debug.Log("location open");
		MenuControllerScript.Instance.CloseMainPage();
		buildingCanvas.Open();
		OnOpenLocation();
		Show();
	}
	public virtual void Close(){
		LocationControllerScript.Instance.Close();
		MenuControllerScript.Instance.OpenCity();
		buildingCanvas.Close();
		Hide();
	}
	public void Show(){
		LocationControllerScript.Instance.OpenLocation(typeBackground);
		location.SetActive(true);
	}
	public void Hide(){
		location.SetActive(false);
	}
	protected virtual void OnOpenLocation(){}
}
