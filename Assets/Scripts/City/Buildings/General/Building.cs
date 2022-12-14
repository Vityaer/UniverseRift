using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using System;
public class Building : SerializedMonoBehaviour{

	[SerializeField] protected GameObject building;
	[SerializeField] protected Button buttonOpenBuilding, buttonCloseBuilding;  
	[Header("Main settings")]
	[SerializeField] private int levelForAvailableBuilding = 0; 
	
	protected virtual void Start(){
		OnStart();	
		if(building != null){
			PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
			building.SetActive(false); 
			buttonOpenBuilding?.onClick.RemoveAllListeners();
			buttonCloseBuilding?.onClick.RemoveAllListeners();
			buttonOpenBuilding?.onClick.AddListener(() => Open());
			buttonCloseBuilding?.onClick.AddListener(() => Close());
		}
	}
	public virtual void Open(){
		if(AvailableFromLevel()){
			if(building != null){ CanvasBuildingsUI.Instance.OpenBuilding(building); }
			OpenPage();
		}
	}
	protected bool AvailableFromLevel(){
		bool result = (PlayerScript.GetPlayerInfo.Level >= levelForAvailableBuilding);
		if(result == false)  
 			MessageControllerScript.Instance.ShowErrorMessage(String.Format("Откроется на {0} уровне", levelForAvailableBuilding));
		
		return result;
	}
	public virtual void Close(){
		ClosePage();
		if(building != null){ CanvasBuildingsUI.Instance.CloseBuilding(building); }
	}
	virtual protected void OnStart(){}
	virtual protected void OpenPage(){}
	virtual protected void ClosePage(){}
	virtual protected void OnLoadGame(){ Debug.Log("load game empty: " + gameObject.name);}
	protected void SaveGame(){ PlayerScript.Instance.SaveGame(); }
}
