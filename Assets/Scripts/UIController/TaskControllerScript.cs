using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TaskControllerScript : MonoBehaviour{
	private Task task;
	[Header("UI")]
	public Text name;
	public GameObject objectCurrentTime;
	public SliderTimeScript sliderTime;
	public Button btnTaskController;
	public Text textBtnTaskController;
	public RatingHeroScript ratingController;
	public SubjectCellControllerScript rewardUIController;
	public void SetData(Task task){
		this.task = task;
		name.text = task.name;
		ratingController.ShowRating(task.rating);
		rewardUIController.SetItem(task.Reward);
		UpdateStatus();
	}

	private void UpdateStatus(){
		btnTaskController.onClick.RemoveAllListeners();
		DateTime localDate = DateTime.Now;
		switch(task.status){
			case StatusTask.NotStart:
				sliderTime.SetMaxValue(task.requireTime);
				btnTaskController.onClick.AddListener( () => StartTask() );  
				break;
			case StatusTask.InWork:
				TimeSpan interval = localDate - task.timeStartTask;
				sliderTime.SetData(task.timeStartTask, task.requireTime);
				textBtnTaskController.text = (task.rating * 10).ToString();
				btnTaskController.gameObject.GetComponent<ButtonCostScript>().UpdateCost(new Resource(TypeResource.Diamond, task.rating * 10, 0), BuyProgress );  
				break;
			case StatusTask.Done:
				textBtnTaskController.text = "Done";
				sliderTime.SetFinish();
				btnTaskController.onClick.AddListener( () => GetReward() );  
				break;
		}
	}
	public void StopTimer(){
		sliderTime.StopTimer();
	}


//Action button
	public void StartTask(){
		task.Start();
		objectCurrentTime.SetActive(true);
		sliderTime?.SetData(task.timeStartTask, task.requireTime);
		UpdateStatus();
	}

	public void BuyProgress(int count = 1){
		task.status = StatusTask.Done;
		UpdateStatus();
	}
	public void GetReward(){
		task.GetReward();
	}

}
