using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BossUIScript : MonoBehaviour, IWorkWithWarTable{
	[SerializeField] private BossControllerScript bossController;
	[SerializeField] private Canvas canvasBuild;
	[SerializeField] private TimeSlider sliderHP;
	[SerializeField] private Text textHPBoss;

	public void UpdateUI(){
		// MissionEnemy boss = bossController.GetBoss();
		// textHPBoss.text =  boss.CurrentHP.ToString();
		// sliderHP.ChangeValue(boss.CurrentHP/boss.HP);
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
		WarTableController.Instance.OpenMission(bossController.mission, GameController.Instance.GetListHeroes);
	}
	public void RegisterOnOpenCloseWarTable(){WarTableController.Instance.RegisterOnOpenCloseMission(this.Change);}
	public void UnregisterOnOpenCloseWarTable(){WarTableController.Instance.UnregisterOnOpenCloseMission(this.Change);}
}
