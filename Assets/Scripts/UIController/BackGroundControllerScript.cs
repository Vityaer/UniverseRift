using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControllerScript : MonoBehaviour{

	private static BackGroundControllerScript instance;
	public  static BackGroundControllerScript Instance{get => instance;}
	public GameObject currentBackground;

	[Header("List background")]
	public GameObject cityBackground;
	void Awake(){
		instance = this;
	}
   	public void OpenBackground(GameObject newBackground){
		if(currentBackground != null) currentBackground.SetActive(false);
		currentBackground = newBackground;
		currentBackground.SetActive(true);
	}
	public void OpenCityBackground(){
		OpenBackground(cityBackground);
	}
}
