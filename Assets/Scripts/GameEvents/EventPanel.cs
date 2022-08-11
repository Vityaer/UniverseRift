using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPanel : MonoBehaviour{
	public BasePanelScript panelEvent;
	public void OpenEventPanel(){
		panelEvent.Open();
	}
	public void Show(){
		gameObject.SetActive(true);
	}
	public void Hide(){
		gameObject.SetActive(false);
	}
}
