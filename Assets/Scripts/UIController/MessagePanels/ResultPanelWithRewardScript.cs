using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanelWithRewardScript : MonoBehaviour{
	public GameObject panel;
	public virtual void Open(){
		panel.SetActive(true);
	}
	public void Close(){
		panel.SetActive(false);
		MessageControllerScript.Instance.Close();
		OnClose();
	}
	protected virtual void OnClose(){}
}
