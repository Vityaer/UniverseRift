using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationControllerScript : MonoBehaviour{
	public List<Location> locations = new List<Location>();
	private Location curLocation;
	void Awake(){
		instance = this;
	}
	public void OpenLocation(TypeLocation typeLocation){
		curLocation = locations.Find(x => x.type == typeLocation);
		BackGroundControllerScript.Instance.OpenBackground(curLocation.backgroundForFight);
	}
	// public void CloseLocation(){
	// 	curLocation.backgroundForFight.SetActive(false);
	// }
	public Sprite GetBackgroundForMission(TypeLocation typeLocation){
		return locations.Find(x => x.type == typeLocation).backgroundForMission;
	}
	private static LocationControllerScript instance;
	public static LocationControllerScript Instance{get => instance;}
}

public enum TypeLocation{
	Forest,
	NightForest,
	Desert
}
[System.Serializable]
public class Location{
	public TypeLocation type;
	public GameObject backgroundForFight;
	public Sprite backgroundForMission;
}