﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class Effect{
	

	[Header("Data")]
	public TypePerformance performance    = TypePerformance.All;
	public TypeEvent  typeEvent           = TypeEvent.OnStartFight;
	public int        countExecutions     = -1;
	private HeroControllerScript master; 

	[Header("Conditions")]
	public List<ConditionEffect> conditions = new List<ConditionEffect>();

	[Header("Actions")]
	public  List<ActionEffect> listAction = new List<ActionEffect>();
	private List<HeroControllerScript> listTarget = new List<HeroControllerScript>();


//API
	public void CreateEffect(HeroControllerScript master){
		this.master = master;
		foreach (ActionEffect action in listAction) {
			action.Master = master;
		}
		RegisterOnEvent(master);
	}

	public void ExecuteEffect(){
		if(performance == TypePerformance.Random){
			int rand = UnityEngine.Random.Range(0, listAction.Count);
			listAction[ rand ].SetNewTarget(this.listTarget);
			listAction[ rand ].ExecuteAction();
		}else{
			foreach (ActionEffect action in listAction) {
				action.SetNewTarget(this.listTarget);
				action.ExecuteAction();
			}	
		}
	}

	public void ExecuteSpell(List<HeroControllerScript> listTarget){
		if(performance == TypePerformance.Random){
			int rand = UnityEngine.Random.Range(0, listAction.Count);
			listAction[ rand ].SetNewTarget(listTarget);
			listAction[ rand ].ExecuteAction();
		}else{
			foreach (ActionEffect action in listAction) {
				action.SetNewTarget(listTarget);
				action.ExecuteAction();
			}	
		}
	}

	public void RegisterOnEvent(HeroControllerScript master){
		this.master = master;
		switch (typeEvent) {
			case TypeEvent.OnStartFight:
				master.RegisterOnStartFight(ExecuteEffect);
				break;
			case TypeEvent.OnStrike:
				master.RegisterOnStrike(ExecuteSpell);
				break;
			case TypeEvent.OnTakingDamage:
				master.RegisterOnTakingDamage(ExecuteEffect);
				break;
			case TypeEvent.OnDeathHero:
				master.RegisterOnDeathHero(ExecuteEffect);
				break;
			case TypeEvent.OnHPLess50:
				master.RegisterOnHPLess50(ExecuteEffect);
				break;
			case TypeEvent.OnHPLess30:
				master.RegisterOnHPLess30(ExecuteEffect);
				break;
			case TypeEvent.OnHeal:
				master.RegisterOnHeal(ExecuteEffect);
				break;
			case TypeEvent.OnSpell:
				master.RegisterOnSpell(ExecuteSpell);
				break;
			case TypeEvent.OnDeathFriend:
				break;		
			case TypeEvent.OnDeathEnemy:
				break;
			case TypeEvent.OnEndRound:
				break;					
		}
	}
}


public enum TypeEvent{
	OnStartFight,
	OnStrike,
	OnTakingDamage,
	OnHPLess50,
	OnHPLess30,
	OnHeal,
	OnSpell,
	OnEndRound,
	OnDeathHero,
	OnDeathFriend,
	OnDeathEnemy}

public enum TypeNumber{
	Percent,
	Num}

public enum TypePerformance{
	All,
	Random
}