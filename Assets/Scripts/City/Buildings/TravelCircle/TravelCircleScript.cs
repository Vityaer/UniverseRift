using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using UnityEngine.UI;
public class TravelCircleScript : Building{
    [SerializeField] private List<TravelCircleOnRace> travels = new List<TravelCircleOnRace>();
	private TravelCircleOnRace currentTravel;
	BuildingWithFightTeams travelCircleSave = null;
	[Header("UI")]
	public List<TravelCircleMissionControllerScript> missionsUI = new List<TravelCircleMissionControllerScript>();
	public RectTransform mainCircle;
	public PanelTravelListMissions panelListMissions;
	public MyScrollRect scrollRectController;
    protected override void OnLoadGame(){
		travelCircleSave = PlayerScript.GetCitySave.travelCircleBuilding;
		foreach(TravelCircleOnRace travel in travels)
			travel.CurrentMission = travelCircleSave.GetRecordInt(travel.GetNameRecord);

		ChangeTravel(travels[UnityEngine.Random.Range(0, travels.Count)].race);
	}
	protected override void OpenPage(){
		LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
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
	private void LoadMissions(List<MissionWithSmashReward> missions, int currentMission){
		for(int i = 0; i < currentMission - 1; i++){
			missionsUI[i].SetData(missions[i], i + 1);
			missionsUI[i].SetCanSmash();
		}
		for(int i = currentMission; ( i < missions.Count )&&( i < missionsUI.Count ); i++){
			missionsUI[i].SetData(missions[i], i + 1);
		}
		for(int i = missions.Count; i < missionsUI.Count; i++){
			missionsUI[i].Hide();
		}
		missionsUI[currentMission].OpenForFight();
	}
	public void OnResultFight(FightResult result){
		if(result == FightResult.Win){
			currentTravel.OpenNextMission();
			travelCircleSave.SetRecordInt(currentTravel.GetNameRecord, currentTravel.CurrentMission);
			SaveGame();
			LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
		}
	}
	public void ChangeTravel(Race newRace){
		if((currentTravel == null) || currentTravel.race != newRace){
			currentTravel = travels.Find(x => x.race == newRace);
			currentTravel.controllerUI.Select();
			LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
		}
	}
	public void OpenTravel(){
		Debug.Log("open travel");
		panelListMissions.Open();
	}
	void Awake(){ instance = this; }
	public static TravelCircleScript Instance{get => instance;} 
	private static TravelCircleScript instance; 
}
[System.Serializable]
public class TravelCircleOnRace{
	private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission"; 
	public Race race;
	public TravelSelectScript controllerUI;
	public List<MissionWithSmashReward> missions = new List<MissionWithSmashReward>();
	private int currentMission =  0;
	public int CurrentMission{get => currentMission; set => currentMission = value;}
	public string GetNameRecord{get => string.Concat(NAME_RECORD_NUM_CURRENT_MISSION, race.ToString());}

	public void OpenNextMission(){
		currentMission += 1;
	}

}