using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;

public class EveryDayTaskControllerScript : RequirementMenuScript{
	protected override void OnLoadGame(){
		LoadData(PlayerScript.GetPlayerGame.saveEveryTimeTasks);
	}
	protected override void SaveData(){
		PlayerScript.GetPlayerGame.SaveEveryTimeTask(listRequirement);
		SaveGame();
	}
}
