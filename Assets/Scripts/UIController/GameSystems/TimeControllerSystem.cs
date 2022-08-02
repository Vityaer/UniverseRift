using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimeControllerSystem : MonoBehaviour{
	private TimeSpan day = new TimeSpan(24, 0, 0);
	private TimeSpan week = new TimeSpan(7, 0, 0, 0);
	private TimeSpan month = new TimeSpan(30, 0, 0, 0);
    private DateTime dayRecover, weekRecover, monthRecover;
    private DateTime currentTime;
    [SerializeField] private bool flagNewDay = false, flagNewWeek = false, flagNewMonth = false;
    void Start(){
    	currentTime = Client.Instance.GetServerTime();
    	UpdateDay(currentTime);
    	UpdateWeek(currentTime);
    	UpdateMonth(currentTime);
    }
    public void UpdateDay(DateTime newDay){
    	TimeSpan deltaTime = newDay - dayRecover;
    	if(deltaTime > day){
    		dayRecover = newDay.Date;
    		flagNewDay = true;
    		OnNewDay();
    	}else{
    		Debug.Log("this equals day");
    	}
    }
    public void UpdateWeek(DateTime newWeek){
    	TimeSpan deltaTime = newWeek - weekRecover;
    	if(deltaTime > week){
    		weekRecover = newWeek.Date;
    		flagNewWeek = true;
    		OnNewWeek();
    	}else{
    		Debug.Log("this equals week");
    	}
    }
    public void UpdateMonth(DateTime newMonth){
    	TimeSpan deltaTime = newMonth - monthRecover;
    	if(deltaTime > month){
    		monthRecover = newMonth.Date;
    		flagNewMonth = true;
    		OnNewMonth();
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