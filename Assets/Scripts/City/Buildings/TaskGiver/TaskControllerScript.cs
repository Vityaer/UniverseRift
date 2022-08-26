using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class TaskControllerScript : MonoBehaviour{
	[SerializeField]private Task task;
	[Header("UI")]
	public TextMeshProUGUI name;
	public GameObject objectCurrentTime;
	public SliderTimeScript sliderTime;
	public ButtonCostScript buttonCostScript;
	public RatingHeroScript ratingController;
	public SubjectCellControllerScript rewardUIController;

	public Task GetTask{get => task;}
	public void SetData(Task task){
		this.task = task;
		name.text = task.name;
		ratingController.ShowRating(task.rating);
		rewardUIController.SetItem(task.Reward);
		UpdateStatus();
	}

	private void UpdateStatus(){
		buttonCostScript.Clear();
		switch(task.status){
			case StatusTask.NotStart:
				sliderTime.SetMaxValue(task.requireTime);
				buttonCostScript.UpdateLabel( () => StartTask(), "Начать" );  
				break;
			case StatusTask.InWork:
				sliderTime.RegisterOnFinish(FinishTask);
				buttonCostScript.UpdateCost(new Resource(TypeResource.Diamond, task.rating * 10, 0), BuyProgress );  
				sliderTime.SetData(task.timeStartTask, task.requireTime);
				break;
			case StatusTask.Done:
				sliderTime.UnregisterOnFinish(FinishTask);
				buttonCostScript.Clear();
				buttonCostScript.UpdateLabel( () => GetReward(), "Завершить" );  
				break;
		}
	}
	public void StopTimer(){
		sliderTime.StopTimer();
	}
	private void FinishTask(){
		task.Finish();
		UpdateStatus();
	}

//Action button
	public void StartTask(){
				Debug.Log("start");
		task.Start();
		objectCurrentTime.SetActive(true);
		sliderTime?.SetData(task.timeStartTask, task.requireTime);
		UpdateStatus();
		TaskGiverScript.Instance.UpdateSave();
	}

	public void BuyProgress(int count = 1){
				Debug.Log("BuyProgress");
		sliderTime.UnregisterOnFinish(FinishTask);
		sliderTime.SetFinish();
		FinishTask();
	}
	public void GetReward(){
				Debug.Log("GetReward");
		TaskGiverScript.Instance.FinishTask(this);
	}

}
