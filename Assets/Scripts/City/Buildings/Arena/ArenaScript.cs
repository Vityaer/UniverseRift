using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
public class ArenaScript : BuildingWithFight{
    ArenaBuildingSave arenaBuildingSave = null;
	private const string RECORD_ = "CurrentMission"; 
	protected override void OnLoadGame(){
		arenaBuildingSave = PlayerScript.GetCitySave.arenaBuilding;
	}

    public void FightWithOpponentUseAI(ArenaOpponent opponent){
    	OpenMission(opponent.mission);
    }
	protected override void OnResultFight(FightResult result){
		if(result == FightResult.Win){
			OnWinFight(1);
			SaveGame();
		}
		OnTryFight();
	}
	private static ArenaScript instance;
	public static ArenaScript Instance{get => instance;}
	void Awake(){ instance = this; }
}
