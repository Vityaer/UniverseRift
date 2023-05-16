using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using UnityEngine.UI;
using Models.Requiremets;

public class MonthlyEvents : Building
{
	[SerializeField] private MonthlyTasks arenaTasks, travelTasks, evolutionTasks, taskBoardsTasks;
	public Button ArenaOpenButton;
	public Button TravelOpenButton;
	public Button EvolutionOpenButton;
	public Button TaskBoardsOpenButton;

	private MonthlyTasks _currentPage;

	MonthlyRequirements monthlyRequirements = null;

	protected override void OnStart()
	{
		ArenaOpenButton.onClick.AddListener(() => OpenPageEvents(arenaTasks));
		TravelOpenButton.onClick.AddListener(() => OpenPageEvents(travelTasks));
		EvolutionOpenButton.onClick.AddListener(() => OpenPageEvents(evolutionTasks));
		TaskBoardsOpenButton.onClick.AddListener(() => OpenPageEvents(taskBoardsTasks));
	}

	protected override void OnLoadGame()
	{
		monthlyRequirements = GameController.GetCitySave.cycleEvents.monthlyRequirements;
		LoadTasks();
	}

	private void LoadTasks()
	{
		this.monthlyRequirements = monthlyRequirements;
		arenaTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Arena));
		travelTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Travel));
		evolutionTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.Evolution));
		taskBoardsTasks.LoadData(monthlyRequirements.GetTasks(TypeMonthlyTasks.TaskBoard));
	}
	
	public void SaveData(TypeMonthlyTasks type, List<Achievement> tasks)
	{
		List<AchievementSave> RequirementSaves = monthlyRequirements.GetTasks(type);
		GameController.GetPlayerGame.SaveRequirement(RequirementSaves, tasks);
		SaveGame();
	}

	private void OpenPageEvents(MonthlyTasks currentPage)
	{
		Open();
		_currentPage = currentPage;
		_currentPage.MainPanel.SetActive(true);
	}

	protected override void ClosePage()
	{
		_currentPage.MainPanel.SetActive(false);
		_currentPage = null;
	}
}
public enum TypeMonthlyTasks{
	Arena     = 0,
	Travel    = 1,
	Evolution = 2,
	TaskBoard = 3
}