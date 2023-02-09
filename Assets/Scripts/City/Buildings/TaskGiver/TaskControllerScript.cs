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

	public Task GetTask => task;
	
	public void SetData(Task task)
	{
		this.task = task;
		name.text = task.name;
		ratingController.ShowRating(task.rating);
		rewardUIController.SetItem(task.Reward);
		UpdateStatus();
	}

	private void UpdateStatus()
	{
		buttonCostScript.Clear();
		switch(task.status){
			case StatusTask.NotStart:
				sliderTime.SetMaxValue(task.requireTime);
				buttonCostScript.UpdateLabel( StartTask, "Начать" );  
				break;
			case StatusTask.InWork:
				sliderTime.RegisterOnFinish(FinishFromSlider);
				buttonCostScript.UpdateCost(new Resource(TypeResource.Diamond, task.rating * 10, 0), BuyProgress );  
				sliderTime.SetData(task.timeStartTask, task.requireTime);
				break;
			case StatusTask.Done:
				sliderTime.SetData(task.timeStartTask, task.requireTime);
				sliderTime.UnregisterOnFinish(FinishFromSlider);
				buttonCostScript.UpdateLabel( GetReward, "Завершить" );  
				break;
		}
	}
	public void StopTimer()
	{
		sliderTime.StopTimer();
	}
	private void FinishFromSlider()
	{
		FinishTask();
	}

	private void FinishTask()
	{
		sliderTime.UnregisterOnFinish(FinishFromSlider);
		task.Finish();
		UpdateStatus();
	}

//Action button
	public void StartTask(int count)
	{
		task.Start();
		objectCurrentTime.SetActive(true);
		sliderTime?.SetData(task.timeStartTask, task.requireTime);
		UpdateStatus();
		TaskGiverScript.Instance.UpdateSave();
	}

	public void BuyProgress(int count = 1)
	{
		buttonCostScript.Clear();
		sliderTime.SetFinish();
	}

	public void GetReward(int count)
	{
		TaskGiverScript.Instance.FinishTask(this);
	}

}
