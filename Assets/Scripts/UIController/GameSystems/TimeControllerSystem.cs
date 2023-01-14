using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
public class TimeControllerSystem : MonoBehaviour{
	private TimeSpan day = new TimeSpan(24, 0, 0);
	private TimeSpan week = new TimeSpan(7, 0, 0, 0);
	private TimeSpan month = new TimeSpan(30, 0, 0, 0);
    private DateTime dayCycle, weekCycle, monthCycle;
    private DateTime currentTime;
    [SerializeField] private bool flagNewDay = false, flagNewWeek = false, flagNewMonth = false;

	private const string NAME_RECORD_DAY   = "CurrentDay"; 
	private const string NAME_RECORD_WEEK  = "CurrentWeek"; 
	private const string NAME_RECORD_MONTH = "CurrentMonth"; 

    public DateTime GetDayCycle{get => dayCycle;}
    public DateTime GetWeekCycle{get => weekCycle;}
    public DateTime GetMonthCycle{get => monthCycle;}
    void Start(){
    	PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
    }
    TimeManagement timeControllerSave = null;
    void OnLoadGame(){
    	currentTime = Client.Instance.GetServerTime();
    	timeControllerSave = PlayerScript.GetCitySave.timeManagement;
        if(timeControllerSave.CheckRecordDate(NAME_RECORD_DAY)){
        	dayCycle   = timeControllerSave.GetRecordDate(NAME_RECORD_DAY);
        	weekCycle  = timeControllerSave.GetRecordDate(NAME_RECORD_WEEK);
        	monthCycle = timeControllerSave.GetRecordDate(NAME_RECORD_MONTH);
        }
    	UpdateDay(currentTime);
    	UpdateWeek(currentTime);
    	UpdateMonth(currentTime);
    }
    public void UpdateDay(DateTime newDay){
    	TimeSpan deltaTime = newDay - dayCycle;
    	if(deltaTime > day){
            Debug.Log("this new day");
    		dayCycle = newDay.Date;
    		flagNewDay = true;
    		timeControllerSave.SetRecordDate(NAME_RECORD_DAY, dayCycle);
    		OnNewDay();
    	}else{
    		Debug.Log("this equals day");
    	}
    }
    public void UpdateWeek(DateTime newWeek){
    	TimeSpan deltaTime = newWeek - weekCycle;
    	if(deltaTime > week){
    		weekCycle = newWeek.Date;
    		flagNewWeek = true;
    		OnNewWeek();
    		timeControllerSave.SetRecordDate(NAME_RECORD_WEEK, weekCycle);
    	}else{
    		Debug.Log("this equals week");
    	}
    }
    public void UpdateMonth(DateTime newMonth){
    	TimeSpan deltaTime = newMonth - monthCycle;
    	if(deltaTime > month){
    		monthCycle = newMonth.Date;
    		flagNewMonth = true;
    		OnNewMonth();
    		timeControllerSave.SetRecordDate(NAME_RECORD_MONTH, monthCycle);
    	}else{
    		Debug.Log("this equals month");
    	}
    }
    private Action observerDayChange, observerWeekChange, observerMonthChange;
    public void RegisterOnNewCycle(Action d, CycleRecover cycle){
    	switch(cycle){
    		case CycleRecover.Day:
    			RegisterOnNewDay(d);
    			break;
    		case CycleRecover.Week:
    			RegisterOnNewWeek(d);
    			break;
    		case CycleRecover.Month:
    			RegisterOnNewMonth(d);
    			break;	
    	}
    }
    public void RegisterOnNewDay(Action d)  {observerDayChange   += d; if(flagNewDay)   d();} 
    public void RegisterOnNewWeek(Action d) {observerWeekChange  += d; if(flagNewWeek)  d();} 
    public void RegisterOnNewMonth(Action d){observerMonthChange += d; if(flagNewMonth) d();} 
    private void OnNewDay()  {if(observerDayChange   != null) observerDayChange();}
    private void OnNewWeek() {if(observerWeekChange  != null) observerWeekChange();}
    private void OnNewMonth(){if(observerMonthChange != null) observerMonthChange();}

    [ContextMenu("Change Cycle day")]
    private void ChangeDay(){ OnNewDay(); }
	void Awake(){ instance = this; }
	private static TimeControllerSystem instance;
	public static TimeControllerSystem Instance{get => instance;}
}

public enum TypeDateRecord{
    DayRecover  = 0,
    WeekRecover = 1,
    monthRecover = 2,
    AutoFightReward = 3
}