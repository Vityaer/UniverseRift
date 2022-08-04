using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingWithFight : Building{
	protected override void OpenPage(){
		WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
	}
	public void OpenMission(Mission mission){
    	FightControllerScript.Instance.RegisterOnFightResult(OnResultFight);
		WarTableControllerScript.Instance.OpenMission(mission, OnAfterFight);
    }
	public void OnAfterFight(bool isOpen){
		if(!isOpen){ Open(); UnregisterFight(); }else{ Close(); }
	} 
	protected virtual void OnResultFight(FightResult result){Debug.Log("not override result fight");}
	private void UnregisterFight(){
    	FightControllerScript.Instance.UnregisterOnFightResult(OnResultFight);
    	WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
	}
	private Action<BigDigit> observerTryFight, observerWinFight;
	public void RegisterOnTryFight(Action<BigDigit> d){observerTryFight += d;}
	public void UnregisterOnTryFight(Action<BigDigit> d){observerTryFight -= d;}
	protected void OnTryFight(){
		if(observerTryFight != null)
			observerTryFight(new BigDigit(1));
	}
	public void RegisterOnWinFight(Action<BigDigit> d){observerWinFight += d;}
	public void UnregisterOnWinFight(Action<BigDigit> d){observerWinFight -= d;}
	protected void OnWinFight(int num){
		if(observerWinFight != null)
			observerWinFight(new BigDigit(num));
	}
}