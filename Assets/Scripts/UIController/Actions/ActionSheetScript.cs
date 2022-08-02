using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionSheetScript : MonoBehaviour{
	[SerializeField] private FooterButtonScript btnOpenClose;
	[SerializeField] private Canvas canvasActionUI;
	[SerializeField] private GameObject background;
	void Awake(){
		btnOpenClose.RegisterOnChange(Change);
	}
	void Change(bool isOpen){
		if(isOpen){ Open(); }else{ Close(); }
	}
	public void Open(){
		canvasActionUI.enabled = true;
		BackGroundControllerScript.Instance.OpenBackground(background);
	}
	public void Close(){
		canvasActionUI.enabled = false;
	}
}
