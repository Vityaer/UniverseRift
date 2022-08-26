using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MainEventController : MonoBehaviour{
	public List<EventPanel> listSimpleEvent = new List<EventPanel>();
	public StageCycleEvent stageCycleEvent;

	public void Open(DateTime startTime, TimeSpan requireTime){
		gameObject.SetActive(true);
		foreach(EventPanel panel in listSimpleEvent){
			panel.Show(startTime, requireTime);
		}
	}
	[ContextMenu("Close")]
	private void Close(){
		foreach(EventPanel panel in listSimpleEvent){
			panel.Hide();
		}	
		gameObject.SetActive(false);
	}
}