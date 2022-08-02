using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
public class TravelCircleScript : Building{
    [SerializeField] private List<TravelCircleOnRace> travels = new List<TravelCircleOnRace>();
	private TravelCircleOnRace currentTravel;
	BuildingWithFightTeams travelCircleSave = null;
	[Header("UI")]
	public List<TravelCircleMissionControllerScript> missionsUI = new List<TravelCircleMissionControllerScript>();
	public MyScrollRect scrollRectController;
    protected override void OnLoadGame(){
		travelCircleSave = PlayerScript.GetCitySave.travelCircleBuilding;
		foreach(TravelCircleOnRace travel in travels)
			travel.currentMission = travelCircleSave.GetRecordInt(travel.GetNameRecord);
		currentTravel = travels[UnityEngine.Random.Range(0, travels.Count)];
	}
	protected override void OpenPage(){
		LoadMissions(currentTravel.missions, currentTravel.currentMission);
	}
	public void OpenMission(Mission mission){
    	FightControllerScript.Instance.RegisterOnFightResult(OnResultFight);
		WarTableControllerScript.Instance.OpenMission(mission, OnAfterFight);
    }
	public void OnAfterFight(bool isOpen){
		if(!isOpen){
			WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
			Open();
		}else{
			Close();
		}
	} 
	private void LoadMissions(List<MissionWithSmashReward> missions, int startMission){
		for(int i = 0; i < startMission - 1; i++){
			missionsUI[i].Hide();
		}
		for(int i = startMission - 1; i < missions.Count; i++){
			missionsUI[i].SetData(missions[i], i + 1);
		}
		missionsUI[startMission - 1].SetCanSmash();
	}
	public void OnResultFight(FightResult result){
		if(result == FightResult.Win){
			currentTravel.currentMission += 1;
			travelCircleSave.SetRecordInt(currentTravel.GetNameRecord, currentTravel.currentMission);
			SaveGame();
			LoadMissions(currentTravel.missions, currentTravel.currentMission);
		}
	}
	public void OpenTravel(Race race){
		currentTravel = travels.Find(x => x.race == race);
		if(currentTravel != null){
			LoadMissions(currentTravel.missions, currentTravel.currentMission);
		}
	}
	void Awake(){ instance = this; }
	public static TravelCircleScript Instance{get => instance;} 
	private static TravelCircleScript instance; 
}
[System.Serializable]
public class TravelCircleOnRace{
	private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission"; 
	public Race race;
	public List<MissionWithSmashReward> missions = new List<MissionWithSmashReward>();
	public int currentMission =  0;
	public string GetNameRecord{get => string.Concat(NAME_RECORD_NUM_CURRENT_MISSION, race.ToString());}
}