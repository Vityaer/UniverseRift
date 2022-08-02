using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRoomScript : Building, IWorkWithWarTable{
	[SerializeField] private Mission mission;
	[SerializeField] private List<WarriorPlaceScript>  leftTeam = new List<WarriorPlaceScript>();
	[SerializeField] private List<WarriorPlaceScript> rightTeam = new List<WarriorPlaceScript>();
	protected override void OpenPage(){
		UnregisterOnOpenCloseWarTable();
	}
	public void ChangeTeamForFill(bool isLeft){

	}
	public void OpenFight(){
		WarTableControllerScript.Instance.OpenMission(mission, TavernScript.Instance.GetListHeroes);
		RegisterOnOpenCloseWarTable();
	}
	public void Change(bool isOpen){
		if(!isOpen){ Open(); }else{  Close(); }
	}
	public void RegisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.RegisterOnOpenCloseMission(this.Change);}
	public void UnregisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(this.Change);} 
}
