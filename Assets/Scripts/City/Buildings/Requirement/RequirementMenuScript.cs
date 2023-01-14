using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
public class RequirementMenuScript : Building{
	[SerializeField] private Transform  taskboard;
	[SerializeField] private GameObject prefabRequirement;
	[SerializeField] protected List<Requirement> listRequirement = new List<Requirement>(); 
	[SerializeField] protected List<RequirementUI> requirementControllers = new List<RequirementUI>();

	protected List<RequirementUI> listTaskUI = new List<RequirementUI>();
	List<RequirementSave> RequirementSaves = new List<RequirementSave>();
	public MyScrollRect scrollRectController;
	protected override void OnLoadGame(){
		LoadData(PlayerScript.GetPlayerGame.saveMainRequirements);
	}
	public void LoadData(List<RequirementSave> RequirementSaves){
		Requirement currentTask = null;
		for(int i = 0; i < RequirementSaves.Count; i++){
			currentTask = listRequirement.Find(x => x.ID == RequirementSaves[i].ID);
			if(currentTask != null) {currentTask.SetProgress(RequirementSaves[i].currentStage, RequirementSaves[i].progress);}else{
				RequirementSaves.Remove(RequirementSaves[i]);
			}
		}
		CreateRequrements();
		OnAfterLoadData();
	}
	protected virtual void SaveData(){
		PlayerScript.GetPlayerGame.SaveMainRequirements(listRequirement);
		SaveGame();
	}
	protected void CreateRequrements(){
		RequirementUI currentTask = null;
		foreach(Requirement task in listRequirement){
			currentTask = GetRequirementUI();
			currentTask.SetData(task as Requirement);
			currentTask.RegisterOnChange(SaveData);
			currentTask.SetScroll(scrollRectController);
			listTaskUI.Add(currentTask);
		}
	}
	protected virtual void OnAfterLoadData(){}
	private RequirementUI GetRequirementUI(){
		RequirementUI result = requirementControllers.Find(x => x.IsEmpty);
		if(result == null){
			result = Instantiate(prefabRequirement, taskboard).GetComponent<RequirementUI>();
			requirementControllers.Add(result);
		}
		return result;
	}
	[ContextMenu("Clear all task")]
	public void ClearAllTask(){
		for(int i = 0; i < listTaskUI.Count; i++){
			listTaskUI[i].Restart();
		}
	}
//Test and check
	[ContextMenu("Check all")]
	public void CheckAll(){
		for(int i = 0; i < listRequirement.Count - 1; i++){
			for(int j = i + 1; j < listRequirement.Count; j++){
				if(listRequirement[i].ID == listRequirement[j].ID){
					Debug.Log(string.Concat("two Requirement with equals ID: ", listRequirement[i].ID.ToString(), " ,i: ", i.ToString(), " ,j: ", j.ToString()));
				}
			}
		}
	}

}
