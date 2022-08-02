using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;

public class TutorialScript : MonoBehaviour{
	void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
	}
	SimpleBuildingSave tutorial = null;
	private const string NAME_RECORD_STAGE = "Stage"; 
	int stage = 0;
	void OnLoadGame(){
		tutorial = PlayerScript.GetCitySave.tutorial;
		stage    = tutorial.GetRecordInt(NAME_RECORD_STAGE);  
		switch(stage){
			case 0:
				StartTutorial();
				break;
		}
	}
	private void StartTutorial(){
		stage += 1;
		PlayerScript.Instance.AddResource(new Resource(TypeResource.SimpleHireCard, 50, 0));
		PlayerScript.Instance.AddResource(new Resource(TypeResource.SimpleTask, 50, 0));
		PlayerScript.Instance.AddResource(new Resource(TypeResource.SpecialTask, 50, 0));
		PlayerScript.Instance.AddResource(new Resource(TypeResource.SpecialHireCard, 50, 0));
		SaveStage();
	}
	private void SaveStage(){tutorial.SetRecordInt(NAME_RECORD_STAGE, stage);}
}
