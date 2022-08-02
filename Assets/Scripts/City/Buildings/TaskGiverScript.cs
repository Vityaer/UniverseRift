using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using UnityEngine.UI;
public class TaskGiverScript : Building{

	[HideInInspector] public List<Task> tasks = new List<Task>();
	public RectTransform taskboard;
	private List<TaskControllerScript> taskControllers = new List<TaskControllerScript>(); 
	public GameObject prefabTask;
	public PatternTask patternTasks;
	[Header("UI controller")]
	public ButtonCostScript buttonBuySimpleTask;
	public ButtonCostScript buttonBuySpecialTask; 
	protected override void OpenPage(){
		LoadTasks();
		if((taskControllers.Count == 0) && (tasks.Count > 0)) FirstCreateTasks();
	}

	protected override void ClosePage(){
		foreach(TaskControllerScript task in taskControllers){
			task.StopTimer();
		}
	}
	private void FirstCreateTasks(){
		for(int i=0; i < tasks.Count; i++){
			taskControllers.Add(Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskControllerScript>());
			taskControllers[i].SetData(tasks[i]);
		}
	}
	public void CreateSimpleTask(int count = 1){ CreateTask(patternTasks.GetSimpleTask()); }
	public void CreateSprecialTask(int count = 1){ CreateTask(patternTasks.GetSpecialTask()); }
	private void CreateTask(Task newTask){
		tasks.Add(newTask);
		TaskControllerScript newTaskController = Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskControllerScript>();
		taskControllers.Add(newTaskController);
		newTaskController.SetData(newTask);
	}
	private static TaskGiverScript instance;
	public static TaskGiverScript Istance{get => instance;}	
	bool isLoadedTask = false;
	void Start(){
		if(instance == null){
			instance = this;
			buttonBuySimpleTask.RegisterOnBuy(CreateSimpleTask);
			buttonBuySpecialTask.RegisterOnBuy(CreateSprecialTask);
		}else{
			Debug.Log("double task giver");
		}
	}
	void LoadTasks(){
		if(isLoadedTask == false){
			foreach(Task task in tasks) if(task.Reward == null) task.Reward = patternTasks.GetRandomReward(task.rating); 
			isLoadedTask = true;
		}

	}
	private TaskGiverBuilding taskGiverBuilding;
	protected override void OnLoadGame(){
		taskGiverBuilding = PlayerScript.GetCitySave.taskGiverBuilding;
		tasks = taskGiverBuilding.tasks;
	}	
}
