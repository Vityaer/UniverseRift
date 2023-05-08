using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingWithFight : Building{

	private Action<BigDigit> observerTryFight, observerWinFight;

	protected override void OpenPage(){
		WarTableController.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
	}

	public void OpenMission(Mission mission){
    	FightController.Instance.RegisterOnFightResult(OnResultFight);
		WarTableController.Instance.OpenMission(mission, OnAfterFight);
    }

	public void OnAfterFight(bool isOpen){
		if(isOpen == false)
		{
			Open();
			UnregisterFight();
		}
		else
		{ 
			Close(); 
		}
	} 

	protected virtual void OnResultFight(FightResult result)
	{
		Debug.Log("not override result fight");
	}

	private void UnregisterFight()
	{
    	FightController.Instance.UnregisterOnFightResult(OnResultFight);
    	WarTableController.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
	}

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