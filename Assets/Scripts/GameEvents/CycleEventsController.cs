using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
public class CycleEventsController : MonoBehaviour{
	public List<MainEventController> listEvents = new List<MainEventController>();
	public MonthlyEvents monthlyEvents;
	[SerializeField] private int cicleTimeDay;
	private DateTime dateMonthlyEvent, dateLastChangeCycle;
	CycleEventsSave сycleEventsSave = null;
	void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
		TimeControllerSystem.Instance.RegisterOnNewMonth(UpdateMonthlyEvent);
		TimeControllerSystem.Instance.RegisterOnNewWeek(UpdateMonthlyEvent);
	}

	private void OnLoadGame(){
		сycleEventsSave = PlayerScript.GetCitySave.cycleEvents;
	}
	private void UpdateMonthlyEvent(){

	}
}