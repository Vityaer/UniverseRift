using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ObjectSave;
public class ChallengeTowerScript : Building{
	public int countShowMission = 8;
	public ListMissions listMissions;
	public List<TowerMissionCotrollerScript> missionsUI = new List<TowerMissionCotrollerScript>();
	public List<TowerMissionCotrollerScript> bottomMissionsUI = new List<TowerMissionCotrollerScript>();
	public List<TowerMissionCotrollerScript> topMissionsUI = new List<TowerMissionCotrollerScript>();
	public MyScrollRect scrollRectController;
	List<InfoHero> listHeroes = new List<InfoHero>();
	int currentMission = 0;
	List<Mission> workMission = new List<Mission>();
	BuildingWithFightTeams challengeTower = null;
	private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission"; 
	protected override void OnLoadGame(){
		challengeTower = PlayerScript.GetCitySave.challengeTowerBuilding;
		currentMission = challengeTower.GetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION);
		LoadMissions();
	}
	protected override void OpenPage(){
		WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
	}
	int skipStart = 2;
	private void LoadMissions(){
		skipStart = currentMission > bottomMissionsUI.Count ? bottomMissionsUI.Count : 0;
		for(int i = currentMission; ((i < currentMission + countShowMission) && (i < listMissions.Count)); i++)
			workMission.Add(listMissions.missions[i]);
		FillData();
	}
	private void FillData(){
		// for(int i= 0; (i < skipStart) && (i < bottomMissionsUI.Count); i++){
		// 	bottomMissionUI[i].SetData(null, currentMission - (i + 1));
		// }

		for(int i = 0; ((i < missionsUI.Count) && (i < workMission.Count)); i++){
			missionsUI[i].SetData(workMission[i], currentMission + i + 1, (i == 0) );
		}
	}
	void Awake(){ instance = this; }
	public static ChallengeTowerScript Instance{get => instance;} 
	private static ChallengeTowerScript instance; 



//After fight
    public void OpenMission(Mission mission){
    	FightControllerScript.Instance.RegisterOnFightResult(OnResultFight);
		WarTableControllerScript.Instance.OpenMission(mission, OnAfterFight);
    }
	public void OnAfterFight(bool isOpen){
		if(!isOpen){ Open(); }else{ Close(); }
	} 

	public void OnResultFight(FightResult result){
		if(result == FightResult.Win){
			OnWinMission(currentMission);
			currentMission += 1;
			challengeTower.SetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, currentMission);
			LoadMissions();
			SaveGame();
		}
		OnTryCompleteMission();
	}
	private Action<BigDigit> observerTryCompleteMission, observerCompleteMission;
	public void RegisterOnTryCompleteMision(Action<BigDigit> d){observerTryCompleteMission += d;}
	public void UnregisterOnTryCompleteMision(Action<BigDigit> d){observerTryCompleteMission -= d;}
	private void OnTryCompleteMission(){
		if(observerTryCompleteMission != null)
			observerTryCompleteMission(new BigDigit(1));
	}
	public void RegisterOnCompleteMision(Action<BigDigit> d){observerCompleteMission += d;}
	public void UnregisterOnCompleteMision(Action<BigDigit> d){observerCompleteMission -= d;}
	private void OnWinMission(int num){
		if(observerCompleteMission != null)
			observerCompleteMission(new BigDigit(num));
	}

}