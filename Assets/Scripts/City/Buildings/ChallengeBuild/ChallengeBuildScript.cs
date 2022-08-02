using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeBuildScript : Building, IWorkWithWarTable{
	[Header("UI")]
	private List<ChallengeUIScript> listChallengeUI = new List<ChallengeUIScript>();
	[SerializeField] private bool isFillList = false;

	[Header("Data")]
	[SerializeField] private List<Challenge> challenges = new List<Challenge>();
	[SerializeField] private GameObject prefabChallengeUI;

	[SerializeField] private Transform transformList;
	protected override void OpenPage(){
		UnregisterOnOpenCloseWarTable();
		if(isFillList == false) FillListChallenge();
	} 
	public void OpenChallenge(Challenge challenge){
		RegisterOnOpenCloseWarTable();
		WarTableControllerScript.Instance.OpenMission(challenge.mission, PlayerScript.Instance.GetListHeroes);
	}
	public void Change(bool isOpen){
		if(!isOpen){ UpdateAllUI(); Open(); }else{ Close(); }
	}
	private void FillListChallenge(){
		isFillList  = true;
		for(int i = 0; i < challenges.Count; i++){
			ChallengeUIScript curChallengeUI = Instantiate(prefabChallengeUI, transformList).GetComponent<ChallengeUIScript>();
			curChallengeUI.SetData(challenges[i], this);
			listChallengeUI.Add(curChallengeUI);
		}
	}
	private void UpdateAllUI(){
		for(int i = 0;  i < listChallengeUI.Count; i++)
			listChallengeUI[i].UpdateControllersUI();
	}
	public void RegisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.RegisterOnOpenCloseMission(this.Change);}
	public void UnregisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(this.Change);}
}
