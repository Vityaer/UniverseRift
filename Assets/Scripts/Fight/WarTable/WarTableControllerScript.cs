using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WarTableControllerScript : MonoBehaviour{
	public List<WarriorPlaceScript>  leftTeam = new List<WarriorPlaceScript>();
	public List<WarriorPlaceScript> rightTeam = new List<WarriorPlaceScript>();
	public Canvas warTableCanvas;
	public ListCardOnWarTableScript listCardPanel;
	private Mission mission;

	[Header("UI")]
	public Button btnStartFight;
	[Header("Strengths Teams")]
	public Text textStrengthLeftTeam;
	public Text textStrengthRightTeam;
	private float strengthLeftTeam = 0, strengthRightTeam = 0;
	

	void Start(){
		warTableCanvas = GetComponent<Canvas>();
		listCardPanel.RegisterOnSelect(SelectCard);
		listCardPanel.RegisterOnUnSelect(UnSelectCard);
	}

	public void SelectCard(CardScript card){ AddHero(card); }
	public void UnSelectCard(CardScript card){ RemoveHero(card);}
	private bool AddHero(CardScript card){
		bool success = false;
		foreach (WarriorPlaceScript place in leftTeam){
			if(place.IsEmpty()){
				place.SetHero(card, card.hero);
				UpdateStrengthTeam(isLeft: true);
				break;
			}
		}
		return success;
	}
	private void RemoveHero(CardScript card){
		foreach(WarriorPlaceScript place in leftTeam){
			if(place.IsEmpty() == false){
				if(place.card == card){
					place.ClearPlace();
				}
			}
		}
		UpdateStrengthTeam(isLeft: true);
	}
	private void ClearRightTeam(){
		for (int i = 0; i < rightTeam.Count; i++) {
			rightTeam[i].hero = null;
			rightTeam[i].ClearUI(); 
		}
	}
	private void ClearLeftTeam(){
		for (int i = 0; i < leftTeam.Count; i++) {
			leftTeam[i].hero = null;
			leftTeam[i].ClearUI(); 
		}
	}

	private void UpdateStrengthTeam(bool isLeft){
		CheckTeam(isLeft);
		if(isLeft){ strengthLeftTeam = 0; }else{ strengthRightTeam = 0; }
		List<WarriorPlaceScript> heroes = (isLeft ? leftTeam : rightTeam).FindAll(x => x.hero != null);
		for(int i = 0; i < heroes.Count; i++){
			if(isLeft){
				strengthLeftTeam  += heroes[i].hero.GetStrength;
			}else{
				strengthRightTeam += heroes[i].hero.GetStrength;
			}
		}
		textStrengthLeftTeam.text  = strengthLeftTeam.ToString();
		textStrengthRightTeam.text = strengthRightTeam.ToString();
	}
	private void CheckTeam(bool isLeft){
		List<WarriorPlaceScript> team = (isLeft ? leftTeam : rightTeam).FindAll(x => x.hero != null);
		int racePeople  = team.FindAll(x => x.hero.generalInfo.race == Race.People ).Count;
		int raceElf     = team.FindAll(x => x.hero.generalInfo.race == Race.Elf    ).Count;
		int raceUndead  = team.FindAll(x => x.hero.generalInfo.race == Race.Undead ).Count;
		int raceDaemon  = team.FindAll(x => x.hero.generalInfo.race == Race.Daemon ).Count;
		int raceGod     = team.FindAll(x => x.hero.generalInfo.race == Race.God    ).Count;
		int raceDarkGod = team.FindAll(x => x.hero.generalInfo.race == Race.DarkGod).Count;
		switch(team.Count){
			case 1:
				if(racePeople == 1)
					Debug.Log("one people");
				break;
			case 6:
				if(racePeople == 6){
					Debug.Log("all people");
				}else if(raceElf == 6){
					Debug.Log("all elf");
				}else if(raceUndead == 6){
					Debug.Log("all undead");
				}else if(raceDaemon == 6){
					Debug.Log("all daemon");
				}else if(raceGod == 6){
					Debug.Log("all god");
				}else if(raceDarkGod == 6){
					Debug.Log("all dark god");
				}else if((raceElf == 3) && (racePeople == 3)){
					Debug.Log("3 elf and 3 people");
				}else if((raceUndead == 3) && (raceDaemon == 3)){
					Debug.Log("3 undead and 3 Daemon");
				}else if((raceGod == 3) && (raceDarkGod == 3)){
					Debug.Log("3 god and 3 darkgod");
				}else if((racePeople == 2) && (raceElf == 2) && (raceGod == 2)){
					Debug.Log("this is Good");
				}else if((raceUndead == 2) && (raceDaemon == 2) && (raceDarkGod == 2)){
					Debug.Log("this is Evil");
				}else if((racePeople == 1) && (raceElf == 1) && (raceDaemon == 1) && (raceUndead == 1) && (raceGod == 1) && (raceDarkGod == 1)){
					Debug.Log("all race");
				}
				break;
		}
		btnStartFight.interactable = isLeft && (team.Count > 0);
	}	
//API
	public void OpenMission(Mission mission, List<InfoHero> listHeroes){
		ClearRightTeam();
		this.mission = mission;
		List<MissionEnemy> listEnemy = mission.listEnemy;
		InfoHero[] heroes = new InfoHero[listEnemy.Count];
		for (int i = 0; i < listEnemy.Count; i++) {
			if(listEnemy[i].enemyPrefab != null){
				rightTeam[i].SetEnemy(listEnemy[i]);
				heroes[i] = listEnemy[i].enemyPrefab;
			}
		}
		UpdateStrengthTeam(isLeft: false);
		FillListHeroes(listHeroes);
		Open();
	}
	public void OpenMission(Mission mission, Action<bool> del){
		RegisterOnOpenCloseMission(del);
		OpenMission(mission, PlayerScript.Instance.GetListHeroes);
	}
	public void OpenMission(Mission mission, Action<bool> actionOnCloseWarTable, Action<FightResult> actionOnResultFight){
		RegisterOnOpenCloseMission(actionOnCloseWarTable);
		FightControllerScript.Instance.RegisterOnFightResult(actionOnResultFight);
		OpenMission(mission, PlayerScript.Instance.GetListHeroes);
	}
	public void ReturnBack(){
		OnOpenMission(false);
		Close();
	}
	private void Close(){
		listCardPanel.EventClose();
		warTableCanvas.enabled = false;
		strengthLeftTeam  = 0;
		strengthRightTeam = 0;
	}
	public void Open(){
		OnOpenMission(true);
		MenuControllerScript.Instance.CloseMainPage();
		warTableCanvas.enabled = true;
		ClearLeftTeam();
		listCardPanel.EventOpen();
	}
	private void FillListHeroes(List<InfoHero> listHeroes){
		listCardPanel.Clear();
		listCardPanel.SetList(listHeroes);
	}
	public void StartFight(){
		Close();
		FightControllerScript.Instance.SetMission(mission, leftTeam, rightTeam);
	}

	public void FinishMission(){Debug.Log("war table finish mission");OnOpenMission(isOpen: false);}
	private Action<bool> observerOpenCloseMission;
	public void RegisterOnOpenCloseMission(Action<bool> d){Debug.Log("war table register on close"); observerOpenCloseMission += d;}
	public void UnregisterOnOpenCloseMission(Action<bool> d){ observerOpenCloseMission -= d;}
	private void OnOpenMission(bool isOpen){
		if(observerOpenCloseMission != null){
			observerOpenCloseMission(isOpen);
		}
	}
	private static WarTableControllerScript instance;
	public  static WarTableControllerScript Instance{get => instance;}
	void Awake(){instance = this;}
}
