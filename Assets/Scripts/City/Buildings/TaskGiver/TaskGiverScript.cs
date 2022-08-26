using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using UnityEngine.UI;
using System;
public class TaskGiverScript : Building{

	[HideInInspector] public List<Task> tasks = new List<Task>();
	public RectTransform taskboard;
	private List<TaskControllerScript> taskControllers = new List<TaskControllerScript>(); 
	public GameObject prefabTask;
	public PatternTask patternTasks;
	[Header("UI controller")]
	public ButtonCostScript buttonBuySimpleTask;
	public ButtonCostScript buttonBuySpecialTask; 
	public ButtonCostScript buttonBuyReplacement; 
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
		taskGiverBuilding.tasks = tasks;
		SaveGame();
	}
	private static TaskGiverScript instance;
	public static TaskGiverScript Instance{get => instance;}	
	bool isLoadedTask = false;
	protected override void OnStart(){
		if(instance == null){
			instance = this;
			buttonBuySimpleTask.RegisterOnBuy(CreateSimpleTask);
			buttonBuySpecialTask.RegisterOnBuy(CreateSprecialTask);
		}
	}
	private TaskGiverBuilding taskGiverBuilding;
	protected override void OnLoadGame(){
		taskGiverBuilding = PlayerScript.GetCitySave.taskGiverBuilding;
		tasks = taskGiverBuilding.tasks;
		FirstCreateTasks();
		SetCostReplacement();
	}
	public void UpdateSave(){
		SaveGame();
	}
	public void FinishTask(TaskControllerScript taskController){
		Task workTask = taskController.GetTask;
		workTask.GetReward();
		OnDoneTask(workTask.rating);
		Destroy(taskController.gameObject);
		tasks.Remove(workTask);
		SaveGame();
	}
	Resource costReplacement = new Resource(TypeResource.Diamond, 10f);
	private void SetCostReplacement(){
		costReplacement.Count = 10f * tasks.FindAll(x => x.status == StatusTask.NotStart).Count;
		buttonBuyReplacement.UpdateCost(costReplacement, ReplacementNotWorkTask);
	}
	public void ReplacementNotWorkTask(int count = 1){
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
	private ObserverDoneTask observerDoneTasks = new ObserverDoneTask(); 
	public void RegisterOnDoneTask(Action<BigDigit> d, int rating){observerDoneTasks.Add(d, rating);}	
	public void UnregisterOnDoneTask(Action<BigDigit> d, int rating){observerDoneTasks.Remove(d, rating);}
	public void OnDoneTask(int rating){
		observerDoneTasks.OnDoneTask(rating);
	}


	public class ObserverDoneTask{
	private List<Observer> observers = new List<Observer>();
	public void Add(Action<BigDigit> del,int rating){
		Observer work = GetObserver(rating);
		if(work != null){
			work.Add(del);
		}else{
			observers.Add(new Observer(del, rating));
		}
	}
	public void Remove(Action<BigDigit> del, int rating){
		Observer work = GetObserver(rating);
		if(work != null){
			work.Remove(del);
			if(work.del == null){
				observers.Remove(work);
			}	
		}
	}
	public void OnDoneTask(int rating){
		Observer work = GetObserver(rating); 
		if(work != null){
			work.DoAction();
		}
	}
	private Observer GetObserver(int rating){
		return observers.Find(x => (x.rating == rating));
	}

	public class Observer{
		public Action<BigDigit> del;
		public int rating;
		public Observer(Action<BigDigit> d, int rating){
			del = d;
			this.rating = rating;
		}
		public void Add(Action<BigDigit> d){ del += d; }
		public void Remove(Action<BigDigit> d){ del -= d; }
		public void DoAction(){
			if(del != null) del(new BigDigit(1));
		}
	}
}
	
}
