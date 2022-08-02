using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelpFuction;

using System;
public class AutoFightScript : MonoBehaviour{
	public Canvas canvasAutoFight;
	public MissionControllerScript missionAutoFight;
	public GoldHeapScript heap;
	[Header("UI")]
	public FooterButtonScript btnOpenClose;
	public GameObject background;
	public void SelectMissionAutoFight(MissionControllerScript infoMission){
		missionAutoFight?.StopAutoFight();
		missionAutoFight = infoMission;
		missionAutoFight.StartAutoFight();
		heap.SetNewReward(missionAutoFight.mission.AutoFightReward);
	}
	public void Open(){
		Debug.Log("auto fight open");
		canvasAutoFight.enabled = true;
		BackGroundControllerScript.Instance.OpenBackground(background);
		heap.OnOpenSheet();
	}
	public void Close(){
		heap.OnCloseSheet();
		canvasAutoFight.enabled = false;
	}

	private static AutoFightScript instance;
	public  static AutoFightScript Instance{get => instance;}
	void Awake(){
		instance = this;
		canvasAutoFight = GetComponent<Canvas>();
		btnOpenClose.RegisterOnChange(Change);
	}


	void Change(bool isOpen){
		if(isOpen){ Open(); }else{ Close(); }
	}

}
