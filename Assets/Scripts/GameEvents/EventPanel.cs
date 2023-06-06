using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EventPanel : MonoBehaviour{
	public BasePanelScript panelEvent;
	public SliderTime timerToEnd;
	public void Open(){
		panelEvent.Open();
	}
	public void Show(DateTime startTime, TimeSpan requireTime){
		gameObject.SetActive(true);
		timerToEnd.SetData(startTime, requireTime);
	}
	public void Hide(){
		gameObject.SetActive(false);
	}
}
