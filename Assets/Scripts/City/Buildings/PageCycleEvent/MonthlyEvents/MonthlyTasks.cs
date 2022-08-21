using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthlyTasks : RequirementMenuScript{
	public MonthlyEvents monthlyEventsParent;
	public TypeMonthlyTasks type;
	[Header("Last task")]
	protected override void OnLoadGame(){}
	protected override void SaveData(){
		monthlyEventsParent.SaveData(type, listRequirement);
	}
	protected override void OnAfterLoadData(){
		for(int i = 0; i < requirementControllers.Count - 1; i++){
			if(requirementControllers[i].IsEmpty == false){
				requirementControllers[i].RegisterOnComplete(OnCompleteTask);
			}
		}
		OnCompleteTask();
	}
	private void OnCompleteTask(){
		bool result = true;
		for(int i = 0; i < requirementControllers.Count - 1; i++){
			if(requirementControllers[i].IsComplete == false){
				result = false;
				break;
			}
		}
		if(result && (requirementControllers[requirementControllers.Count - 1].IsComplete == false)){
			requirementControllers[requirementControllers.Count - 1].SetProgress(new BigDigit(1));
		}
	}
}