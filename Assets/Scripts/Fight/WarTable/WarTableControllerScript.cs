using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class WarTableControllerScript : MonoBehaviour{
	public List<WarriorPlaceScript>  leftTeam = new List<WarriorPlaceScript>();
	public List<WarriorPlaceScript> rightTeam = new List<WarriorPlaceScript>();
	public Canvas warTableCanvas;
	public ListCardOnWarTableScript listCardPanel;
	private Mission mission;

	[Header("UI")]
	public Button btnStartFight;
	public TextMeshProUGUI textStrengthLeftTeam, textStrengthRightTeam;
	

	void Start(){
		warTableCanvas = GetComponent<Canvas>();
		listCardPanel.RegisterOnSelect(SelectCard);
		listCardPanel.RegisterOnUnSelect(UnSelectCard);
		textStrengthLeftTeam.text = string.Empty;
		textStrengthRightTeam.text = string.Empty;
	}

	public void SelectCard(CardScript card){ AddHero(card); }
	public void UnSelectCard(CardScript card){ RemoveHero(card);}

	private bool AddHero(CardScript card){
		bool success = false;
		foreach (WarriorPlaceScript place in leftTeam){
			if(place.IsEmpty()){
				place.SetHero(card, card.hero);
				UpdateStrengthTeam(leftTeam, textStrengthLeftTeam);
				break;
			}
		}
		CheckTeam(leftTeam);
		return success;
	}

	private void RemoveHero(CardScript card)
	{
		foreach(WarriorPlaceScript place in leftTeam){
			if(place.IsEmpty() == false){
				if(place.card == card){
					place.ClearPlace();
				}
			}
		}
		CheckTeam(leftTeam);
		UpdateStrengthTeam(leftTeam, textStrengthLeftTeam);
	}

	private void ClearRightTeam()
	{
		for (int i = 0; i < rightTeam.Count; i++) {
			rightTeam[i].ClearPlace();
		}
	}

	private void ClearLeftTeam()
	{
		for (int i = 0; i < leftTeam.Count; i++) {
			leftTeam[i].ClearPlace();
		}
	}

	private void UpdateStrengthTeam(List<WarriorPlaceScript> team, TextMeshProUGUI textComponent)
	{
		float strengthTeam = 0f;
		for(int i = 0; i < team.Count; i++)
			if(team[i].Hero != null)
				strengthTeam  += team[i].Hero.GetStrength;
		textComponent.text = strengthTeam.ToString();
	}

	private void CheckTeam(List<WarriorPlaceScript> team)
	{
		team = team.FindAll(x => x.Hero != null);
		int racePeople  = team.FindAll(x => x.Hero.generalInfo.race == Race.People ).Count;
		int raceElf     = team.FindAll(x => x.Hero.generalInfo.race == Race.Elf    ).Count;
		int raceUndead  = team.FindAll(x => x.Hero.generalInfo.race == Race.Undead ).Count;
		int raceDaemon  = team.FindAll(x => x.Hero.generalInfo.race == Race.Daemon ).Count;
		int raceGod     = team.FindAll(x => x.Hero.generalInfo.race == Race.God    ).Count;
		int raceDarkGod = team.FindAll(x => x.Hero.generalInfo.race == Race.Elemental).Count;
		switch(team.Count){
			case 1:
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
		btnStartFight.interactable = (leftTeam.Count > 0);
	}	
//API
	public void OpenMission(Mission mission, List<InfoHero> listHeroes)
	{
		ClearLeftTeam();
		ClearRightTeam();
		this.mission = mission;
		for (int i = 0; i < mission.listEnemy.Count; i++) {
			rightTeam[i].SetEnemy(mission.listEnemy[i]);
		}
		UpdateStrengthTeam(rightTeam, textStrengthRightTeam);
		CheckTeam(rightTeam);
		FillListHeroes(listHeroes);
		Open();
	}

	public void OpenMission(Mission mission, Action<bool> del)
	{
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
		ClearRightTeam();
		ClearLeftTeam();
		Close();
	}
	private void Close(){
		listCardPanel.EventClose();
		warTableCanvas.enabled = false;
	}
	public void Open(){

		OnOpenMission(true);
		MenuControllerScript.Instance.CloseMainPage();
		warTableCanvas.enabled = true;
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
