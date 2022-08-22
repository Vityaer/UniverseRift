using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationWithBuildings : MonoBehaviour{
	public GameObject location;
	public TypeLocation typeBackground;
	public Building buildingCanvas;
	public virtual void Open(){
		Debug.Log("location open");
		location.SetActive(true);
		LocationControllerScript.Instance.OpenLocation(typeBackground);
		buildingCanvas.Open();
		OnOpenLocation();
	}
	public virtual void Close(){
		Debug.Log("location close");
		buildingCanvas.Close();
		location.SetActive(false);
	}
	protected virtual void OnOpenLocation(){}
}
