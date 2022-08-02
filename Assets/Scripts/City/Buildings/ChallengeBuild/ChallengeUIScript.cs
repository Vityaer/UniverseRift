using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class ChallengeUIScript : MonoBehaviour{
	[SerializeField] private Text textName;
	[SerializeField] private Image backgroundChallenge;
	[SerializeField] private GameObject btnOpen;
	[SerializeField] private GameObject imageIsDone;
	[SerializeField] private Challenge challenge;
	[SerializeField] private ChallengeBuildScript challengeBuild;
	public void SetData(Challenge challenge, ChallengeBuildScript challengeBuild){
		this.challengeBuild = challengeBuild;
		this.challenge = challenge;
		UpdateUI();
	}
	public void UpdateUI(){
		textName.text = challenge.Name;
    	backgroundChallenge.sprite = LocationControllerScript.Instance.GetBackgroundForMission(challenge.mission.location);
		UpdateControllersUI();
	}
	public void UpdateControllersUI(){
		imageIsDone.SetActive(challenge.IsDone);
		btnOpen.SetActive(!challenge.IsDone);
	}
	public void OpenChallenge(){
		challengeBuild.OpenChallenge(challenge);
	}
}
