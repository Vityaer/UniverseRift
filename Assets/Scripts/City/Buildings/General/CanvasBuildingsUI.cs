using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBuildingsUI : MonoBehaviour{
	public Canvas canvas;
	private Stack<GameObject> listOpenBuilding = new Stack<GameObject>();
	public int countOpenBuildng;

	private static CanvasBuildingsUI instance;
	public static CanvasBuildingsUI Instance{get => instance;}

	public void OpenBuilding(GameObject newBuilding)
	{
		if(listOpenBuilding.Count > 0){
			listOpenBuilding.Peek().SetActive(false);
		}else{
			canvas.enabled = true;
			MenuController.Instance.CloseMainPage();
		}

		newBuilding.SetActive(true);
		listOpenBuilding.Push(newBuilding);
		countOpenBuildng = listOpenBuilding.Count;
	}

	public void CloseBuilding(GameObject building)
	{
		if(listOpenBuilding.Count > 0){
			GameObject buildingFromStack = listOpenBuilding.Pop();
			if(building != buildingFromStack) Debug.Log("close other building");
			buildingFromStack.SetActive(false);
		}

		countOpenBuildng = listOpenBuilding.Count;
		if(listOpenBuilding.Count == 0){
			MenuController.Instance.OpenMainPage();
			canvas.enabled = false;
		}else{
			listOpenBuilding.Peek().SetActive(true);
		}
	}

	void Awake(){
		instance = this;
	}

}
