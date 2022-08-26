using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
using IdleGame.MultiplayerData;
using HelpFuction;
public class CycleEventsController : MonoBehaviour{
	public List<MainEventController> listEvents = new List<MainEventController>();
	public int cycleTimeDay = 7;
	private DateTime dateMonthlyEvent, dateLastChangeCycle;
	CycleEventsSave сycleEventsSave = null;
	private const string CURRENT_CYCLE_STAGE = "CycleStage",
						 START_DATETIME_CYCLE = "DateTimeCycle";
	private StageCycleEvent currentStageCycle;
	private MainEventController currentEventController = null; 
	DateTime startTime;
	TimeSpan requireTime;
	void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
		// TimeControllerSystem.Instance.RegisterOnNewMonth(UpdateMonthlyEvent);
		// TimeControllerSystem.Instance.RegisterOnNewWeek(UpdateMonthlyEvent);
	}
	GameTimer timerForNextStageCycle = null;
	private void OnLoadGame(){
		сycleEventsSave = PlayerScript.GetCitySave.cycleEvents;
		if(сycleEventsSave.CheckRecordDate(START_DATETIME_CYCLE)){
			currentStageCycle = (StageCycleEvent) сycleEventsSave.GetRecordInt(CURRENT_CYCLE_STAGE);
			startTime = сycleEventsSave.GetRecordDate(START_DATETIME_CYCLE);
		}else{
			DataCycleEvent data = Client.Instance.GetDataCurrentCycleEvent();
			currentStageCycle = (StageCycleEvent) data.stage;
			startTime = FunctionHelp.StringToDateTime(data.startTime);
		}
		OpenCurrentCycleStage();
		timerForNextStageCycle = TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
	}

	[ContextMenu("NextCycleStage")]
	private void NextCycleStage(){
		int current = (int) currentStageCycle;
		current =  (current < listEvents.Count - 1) ? current + 1 : 0;
		currentStageCycle = (StageCycleEvent) current;
		сycleEventsSave.SetRecordDate(START_DATETIME_CYCLE, DateTime.Today);
		сycleEventsSave.SetRecordInt(CURRENT_CYCLE_STAGE, (int) currentStageCycle);
		OpenCurrentCycleStage();
		timerForNextStageCycle = TimerScript.Timer.StartTimer(FunctionHelp.GetLeftSecondsToEnd(startTime, requireTime), NextCycleStage);
	}
	private void OpenCurrentCycleStage(){
		requireTime = new TimeSpan(cycleTimeDay, 0, 0, 0);
		currentEventController = listEvents.Find(x => x.stageCycleEvent == currentStageCycle);
		currentEventController.Open(startTime, requireTime);
	}
}
public enum StageCycleEvent{
	Tavern = 0,
	Agent = 1,
	Arena = 2,
	MagicRound = 3,
	Fortune = 4
}
