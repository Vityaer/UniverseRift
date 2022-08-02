using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BossUIScript : MonoBehaviour, IWorkWithWarTable{
	[SerializeField] private BossControllerScript bossController;
	[SerializeField] private Canvas canvasBuild;
	[SerializeField] private SliderScript sliderHP;
	[SerializeField] private Text textHPBoss;

	public void UpdateUI(){
		MissionEnemy boss = bossController.GetBoss();
		textHPBoss.text =  boss.CurrentHP.ToString();
		sliderHP.ChangeValue(boss.CurrentHP/boss.HP);
	}
	public void Change(bool isOpen){
		if(!isOpen){ Open(); }else{ Close(); }
	}
	public void Open(){
		UpdateUI();
		UnregisterOnOpenCloseWarTable();
		canvasBuild.enabled = true;
	}
	public void Close(){
		canvasBuild.enabled = false;
	}
	public void OpenFightWithBoss(){
		RegisterOnOpenCloseWarTable();
		WarTableControllerScript.Instance.OpenMission(bossController.mission, PlayerScript.Instance.GetListHeroes);
	}
	public void RegisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.RegisterOnOpenCloseMission(this.Change);}
	public void UnregisterOnOpenCloseWarTable(){WarTableControllerScript.Instance.UnregisterOnOpenCloseMission(this.Change);}
}
