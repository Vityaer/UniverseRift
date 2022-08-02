using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanelScript : MonoBehaviour{
	public GameObject panel;
	public virtual void Open(){
		panel.SetActive(true);
		OnOpen();
	}
	public virtual void Close(){
		OnClose();
		panel.SetActive(false);
	}
	protected virtual void OnOpen(){}
	protected virtual void OnClose(){}
}
