using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using UnityEngine.UI;
using System;
using Utils.Observer;

public class TaskGiverScript : Building
{
	public int countFreeTaskOnDay = 6;
	Resource costReplacement = new Resource(TypeResource.Diamond, 10f);
	[HideInInspector] public List<Task> tasks = new List<Task>();
	public RectTransform taskboard;
	private List<TaskControllerScript> taskControllers = new List<TaskControllerScript>(); 
	public GameObject prefabTask;
	public PatternTask patternTasks;
	[Header("UI controller")]
	public ButtonCostScript buttonBuySimpleTask;
	public ButtonCostScript buttonBuySpecialTask; 
	public ButtonCostScript buttonBuyReplacement; 
	private static TaskGiverScript instance;
	public static TaskGiverScript Instance{get => instance;}	
	bool isLoadedTask = false;
	private TaskGiverBuilding taskGiverBuilding;
	private ObserverDoneTask observerDoneTasks = new ObserverDoneTask(); 

	protected override void ClosePage()
	{
		foreach(TaskControllerScript task in taskControllers)
		{
			task.StopTimer();
		}
	}

	private void FirstCreateTasks()
	{
		for(int i=0; i < tasks.Count; i++)
		{
			taskControllers.Add(Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskControllerScript>());
			taskControllers[i].SetData(tasks[i]);
		}
	}

	private void GetDailyTask(){
		var taskInNotWork = tasks.FindAll(x => x.status == StatusTask.NotStart);

		for(int i=0; i < countFreeTaskOnDay; i++)
		{
			if(i < taskInNotWork.Count)
			{
				tasks.Remove(taskInNotWork[i]);
				Destroy(taskControllers.Find(x => x.GetTask == taskInNotWork[i])?.gameObject);
			}

			var newTask = patternTasks.GetSimpleTask();
			tasks.Add(newTask);
			var newTaskController = Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskControllerScript>();
			taskControllers.Add(newTaskController);
			newTaskController.SetData(newTask);
			taskGiverBuilding.tasks = tasks;
		}
		SetCostReplacement();
		SaveGame();
	}
	public void CreateSimpleTask(int count = 1)
	{
		CreateTask(patternTasks.GetSimpleTask());
	}

	public void CreateSprecialTask(int count = 1)
	{
		CreateTask(patternTasks.GetSpecialTask());
	}

	private void CreateTask(Task newTask)
	{
		tasks.Add(newTask);
		TaskControllerScript newTaskController = Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskControllerScript>();
		taskControllers.Add(newTaskController);
		newTaskController.SetData(newTask);
		taskGiverBuilding.tasks = tasks;
		SetCostReplacement();
		SaveGame();
	}

	protected override void OnStart()
	{
		if(instance == null)
		{
			instance = this;
			buttonBuySimpleTask.RegisterOnBuy(CreateSimpleTask);
			buttonBuySpecialTask.RegisterOnBuy(CreateSprecialTask);
		}
	}

	protected override void OnLoadGame()
	{
		taskGiverBuilding = PlayerScript.GetCitySave.taskGiverBuilding;
		tasks = taskGiverBuilding.tasks;
		FirstCreateTasks();
		TimeControllerSystem.Instance.RegisterOnNewCycle(GetDailyTask, CycleRecover.Day);
		SetCostReplacement();
	}

	public void UpdateSave()
	{
		SaveGame();
		SetCostReplacement();
	}

	public void FinishTask(TaskControllerScript taskController){
		Task workTask = taskController.GetTask;
		workTask.GetReward();
		OnDoneTask(workTask.rating);
		Destroy(taskController.gameObject);
		tasks.Remove(workTask);
		SaveGame();
	}

	private void SetCostReplacement()
	{
		costReplacement.Count = 10f * tasks.FindAll(x => x.status == StatusTask.NotStart).Count;
		buttonBuyReplacement.UpdateCost(costReplacement, ReplacementNotWorkTask);
	}

	public void ReplacementNotWorkTask(int count = 1)
	{
		List<Task> tasksForReplacement = tasks.FindAll(x => x.status == StatusTask.NotStart);
		for(int i = 0; i < tasksForReplacement.Count; i++){
			tasks.Remove(tasksForReplacement[i]);
			Destroy(taskControllers.Find(x => x.GetTask == tasksForReplacement[i])?.gameObject);
		}
		for(int i = 0; i < tasksForReplacement.Count; i++){
			CreateTask(patternTasks.GetSimpleTask());
		}
		SaveGame();
	}

	public void RegisterOnDoneTask(Action<BigDigit> d, int rating)
	{
		observerDoneTasks.Add(d, rating);
	}	

	public void UnregisterOnDoneTask(Action<BigDigit> d, int rating)
	{
		observerDoneTasks.Remove(d, rating);
	}

	public void OnDoneTask(int rating)
	{
		observerDoneTasks.OnDoneTask(rating);
	}
}
