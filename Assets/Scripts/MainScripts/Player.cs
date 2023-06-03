using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;
[System.Serializable]
public class Player{
	

	[SerializeField] private Game playerGame;
	public PlayerInfoModel GetPlayerInfo{get => playerGame.playerInfo;}
	public Game PlayerGame{get => playerGame;}
	public void LevelUP(){
		GetPlayerInfo.LevelUP();
		OnLevelUP();
	}

//Observers
	private Action<BigDigit> observerOnLevelUP;
	public void RegisterOnLevelUP(Action<BigDigit> d){observerOnLevelUP += d;}
	private void OnLevelUP(){ if(observerOnLevelUP != null) observerOnLevelUP(new BigDigit(GetPlayerInfo.Level, 0)); }	 
}
