using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationWithBuildings : MonoBehaviour
{
	public GameObject location;
	public TypeLocation typeBackground;
	public Building buildingCanvas;

	public virtual void Open()
	{
		Debug.Log("location open");
		MenuController.Instance.CloseMainPage();
		buildingCanvas.Open();
		OnOpenLocation();
		Show();
	}

	public virtual void Close()
	{
		LocationController.Instance.Close();
		buildingCanvas.Close();
		MenuController.Instance.OpenMainPage();
		Hide();
	}

	public void Show()
	{
		LocationController.Instance.OpenLocation(typeBackground);
		location.SetActive(true);
	}

	public void Hide()
	{
		location.SetActive(false);
	}
	
	protected virtual void OnOpenLocation(){}
}
