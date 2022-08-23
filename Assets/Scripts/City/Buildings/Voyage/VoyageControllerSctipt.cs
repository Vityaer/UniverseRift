using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using System;
public class VoyageControllerSctipt : BuildingWithFight{
	[SerializeField] private List<Mission> missions = new List<Mission>();
	[SerializeField] private List<VoyageMissionController> missionsUI = new List<VoyageMissionController>(); 
	[SerializeField] private PanelVoyageMission panelVoyageMission;
	private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission"; 
	VoyageBuildingSave voyageBuildingSave = null;
	int currentMission = 0;
	protected override void OnLoadGame(){
		voyageBuildingSave = PlayerScript.GetCitySave.voyageBuildingSave;
		currentMission = voyageBuildingSave.GetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION);
		LoadMissions();
	}
	private void LoadMissions(){
		StatusMission statusMission = StatusMission.NotOpen;
		for(int i = 0; i < missions.Count; i++){
			if(i < currentMission){
				statusMission = StatusMission.Complete;
			}else if(i == currentMission){
				statusMission = StatusMission.Open;
			}else{statusMission = StatusMission.NotOpen;}
			missionsUI[i].SetData(missions[i], i + 1, statusMission);
		}
	}
	protected override void OnResultFight(FightResult result){
		if(result == FightResult.Win){
			OnWinFight(currentMission);
			missionsUI[currentMission].SetStatus(StatusMission.Complete);
			currentMission += 1;
			if(currentMission < missions.Count) missionsUI[currentMission].SetStatus(StatusMission.Open);
			voyageBuildingSave.SetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, currentMission);
			SaveGame();
			if(currentMission == missions.Count) OnDoneTravel(1);
		}
	}
	public void ShowInfo(VoyageMissionController controller, Reward winReward, StatusMission status){
		panelVoyageMission.ShowInfo(controller, winReward, status);
	}
	private Action<BigDigit> observerDoneTravel;
	public void RegisterOnDoneTravel(Action<BigDigit> d){observerDoneTravel += d;}
	public void UnregisterOnDoneTravel(Action<BigDigit> d){observerDoneTravel -= d;}
	protected void OnDoneTravel(int num){
		if(observerDoneTravel != null)
			observerDoneTravel(new BigDigit(num));
	}
	public LocationWithBuildings locationController;
	protected override void OpenPage(){
		WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
		locationController.Show();
	}
	protected override void ClosePage(){
		Debug.Log("close page");
		locationController.Hide();
	}
	private static VoyageControllerSctipt instance;
	public static VoyageControllerSctipt Instance{get => instance;}
	void Awake(){ instance = this; }
}
