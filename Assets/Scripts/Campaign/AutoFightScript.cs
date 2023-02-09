using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HelpFuction;

using System;
public class AutoFightScript : MainPage
{
	public Canvas canvasAutoFight;
	public MissionControllerScript missionAutoFight;
	public GoldHeapScript heap;
	[Header("UI")]
	public GameObject background;
	private static AutoFightScript instance;
	public  static AutoFightScript Instance{get => instance;}

	protected override void Awake()
	{
		instance = this;
		canvasAutoFight = GetComponent<Canvas>();
		base.Awake();
	}

	public void SelectMissionAutoFight(MissionControllerScript infoMission)
	{
		missionAutoFight?.StopAutoFight();
		missionAutoFight = infoMission;
		missionAutoFight.StartAutoFight();
		heap.SetNewReward(missionAutoFight.mission.AutoFightReward);
	}

	public void SelectMissionAutoFight(CampaignMission mission)
	{
		heap.SetNewReward(mission.AutoFightReward);
	}

	public override void Open()
	{
		base.Open();
		canvasAutoFight.enabled = true;
		BackGroundControllerScript.Instance.OpenBackground(background);
		heap.OnOpenSheet();
	}
	public override void Close()
	{
		heap.OnCloseSheet();
		canvasAutoFight.enabled = false;
	}
}
