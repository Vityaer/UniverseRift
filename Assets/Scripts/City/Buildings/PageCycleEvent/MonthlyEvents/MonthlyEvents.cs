using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
public class MonthlyEvents : Building
{
	[SerializeField] private MonthlyTasks arenaTasks, travelTasks, evolutionTasks, taskBoardsTasks;
	MonthlyRequirements monthlyRequirements = null;
	protected override void OnLoadGame(){
		monthlyRequirements = PlayerScript.GetCitySave.cycleEvents.monthlyRequirements;
		LoadTasks();
	}
	private void LoadTasks(){
		this.monthlyRequirements = monthlyRequirements;
		arenaTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Arena));
		travelTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Travel));
		evolutionTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Evolution));
		taskBoardsTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.TaskBoard));
	}
	
	public void SaveData(TypeMonthlyTasks type, List<Requirement> tasks){
		List<RequirementSave> RequirementSaves = monthlyRequirements.GetTasks(type);
		PlayerScript.GetPlayerGame.SaveRequirement(RequirementSaves, tasks);
		SaveGame();
	}
}
public enum TypeMonthlyTasks{
	Arena     = 0,
	Travel    = 1,
	Evolution = 2,
	TaskBoard = 3
}