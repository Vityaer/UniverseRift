using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceForMineScript : MonoBehaviour{
	public int ID;
	public Transform point;
	public List<TypeMine> types = new List<TypeMine>();
	public MineControllerScript mineController = null;
	void Awake(){ point = base.transform; }
	public void OpenPanelForCreateMine(){
		if(mineController == null){
			MinesScript.Instance.panelNewMineCreate.Open(this);
		}else{
			MinesScript.Instance.panelMineInfo.SetData(mineController);
		}
	}
}
