using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelpFuction;
using UnityEngine.UI;
using TMPro;
using System;
public partial class FightControllerScript : MonoBehaviour{
	public Canvas canvasFightUI;
	[Header("Place heros")]
	public HexagonGridScript hexagonGrid;
	[SerializeField]private  List<Warrior> leftTeam  = new List<Warrior>(); 
	[SerializeField]private  List<Warrior> rightTeam = new List<Warrior>(); 

	private List<HeroControllerScript> listInitiative  = new List<HeroControllerScript>();

	[Header("Location")]
	public LocationControllerScript locationController;


	public List<Warrior> GetLeftTeam{get => leftTeam;}
	public List<Warrior> GetRightTeam{get => rightTeam;}
//Create Teams
	public void SetMission(Mission mission, List<WarriorPlaceScript> leftWarriorPlace, List<WarriorPlaceScript> rightWarriorPlace){
		this.mission = mission;
		locationController.OpenLocation( mission.location );
		HexagonGridScript.Instance.OpenGrid();
		AIController.Instance.StartAIFight();
		CreateTeams(leftWarriorPlace, rightWarriorPlace);
	}
    private void CreateTeams(List<WarriorPlaceScript> leftWarriorPlace, List<WarriorPlaceScript> rightWarriorPlace){
    	Screen.orientation = ScreenOrientation.LandscapeRight;
    	canvasFightUI.enabled = true;
    	CreateTeam(hexagonGrid.GetLeftTeamPos,  leftWarriorPlace,  Side.Left );
    	CreateTeam(hexagonGrid.GetRightTeamPos, rightWarriorPlace, Side.Right );
    	listInitiative.Sort(new HeroInitiativeComparer());
    	StartCoroutine(StartFightCountdown());
    }
    IEnumerator StartFightCountdown(){
    	for (int i = 3; i>0; i--){
	    	textNumRound.text = i.ToString();
			yield return new WaitForSeconds(0.75f);
		}
		textNumRound.text = "Fight!";
		yield return new WaitForSeconds(0.5f);
		textNumRound.text = string.Concat("Round 1"); 
 		isFightFinish = false;
 		PlayDelegateOnStartFight();
		StartFight();

    }
    private void CreateTeam(List<HexagonCellScript> teamPos, List<WarriorPlaceScript> team, Side side){
    	HeroControllerScript heroScript;
    	GameObject hero;
    	for(int i=0; i < team.Count; i++){
    			hero = null;
    			heroScript = null;
    			if((side == Side.Left && (team[i].card != null)) || (team[i].hero != null))
		    		hero = Instantiate(team[i].hero.generalInfo.Prefab, teamPos[i].Position, Quaternion.identity);
    			if(hero != null){
	    			heroScript = hero.GetComponent<HeroControllerScript>(); 
	    			List<Warrior> workTeam = (side == Side.Left) ? leftTeam : rightTeam;
	    			workTeam.Add(new Warrior(heroScript));
	    			heroScript.SetHero(team[i].hero, teamPos[i], side);
	    			listInitiative.Add(heroScript);
    			}

    	}
    }
//Fight loop    	
 	public void StartFight(){ NextHero(); } 
 	private bool isFightFinish = false;
 	private int currentHero = -1;
 	private  int round = 1;
 	public TextMeshProUGUI textNumRound;
 	public int MaxCountRound = 3;
 	private List<HeroControllerScript> listHeroesWithAction = new List<HeroControllerScript>(); 
 	public void AddHeroWithAction(HeroControllerScript newHero){listHeroesWithAction.Add(newHero);}
 	public void RemoveHeroWithAction(HeroControllerScript removeHero){
 		listHeroesWithAction.Remove(removeHero);
 		if(listHeroesWithAction.Count == 0) NextHero();
 	}
 	private void NextHero(){
 		if((currentHero + 1) < listInitiative.Count){
 			currentHero++;
 		}else{
 			NewRound();
 		} 
 		if(isFightFinish == false){
			listInitiative[currentHero].DoTurn();
 		}
 	}
 	private void NewRound(){
 		currentHero = 0;
		round++;
		textNumRound.text = string.Concat("Round ",round.ToString()); 
		PlayDelegateEndRound();
		if(round == MaxCountRound){
			Win(Side.Right);
		}
 	}
 	public void WaitTurn(){
 		HeroControllerScript hero = GetCurrentHero();
 		listInitiative.Remove(hero);
 		currentHero -= 1;
 		listInitiative.Add(hero);
 		hero.EndTurn(); 
 	}
//Result fight
 	private void CheckFinishFight(){
 		if((leftTeam.Count == 0) || (rightTeam.Count == 0)) Win(leftTeam.Count > 0 ? Side.Left : Side.Right);
 	}
 	void Win(Side side){
    	Screen.orientation = ScreenOrientation.Portrait;
 		isFightFinish = true;
 		PlayDelegateOnFinishFight();
 		if(side == Side.Left){
 			MessageControllerScript.Instance.AddMessage("Ты выиграл!");
 		}else{
 			MessageControllerScript.Instance.AddMessage("Ты проиграл!");
 			
 		}
    	StartCoroutine(FinishFightCountdown(side));

 	}
 	Mission mission;
 	IEnumerator FinishFightCountdown(Side side){
		textNumRound.text = "Конец боя!";
		if(side == Side.Right) CheckSaveResult();
		yield return new WaitForSeconds(2.5f);
 		FightResult fightResult = (side == Side.Left) ? FightResult.Win : FightResult.Defeat;
 		mission.OnFinishFight(fightResult); 
 		OnFightResult(result: fightResult);
		textNumRound.text = string.Empty;
    	CloseFigthUI();
 		ClearAll();
    }
    private void CloseFigthUI(){
 		HexagonGridScript.Instance.CloseGrid();
 		canvasFightUI.enabled = false;
    }
 	void ClearAll(){
 		Debug.Log("clear all");
 		DeleteTeam(rightTeam);
 		DeleteTeam(leftTeam);
		listInitiative.Clear();
		currentHero = -1; 
		round = 1;
 	}
 	void DeleteTeam(List<Warrior> team){
 		Debug.Log("delete team");
 		for (int i = 0; i < team.Count;  i++){
 			Debug.Log(i);
 			Debug.Log(team[i].heroController == null);
 			team[i].heroController.DeleteHero();
		}
		team.Clear();
 	}
 	void CheckSaveResult(){ if((mission is BossMission)) (mission as BossMission).SaveResult(); }
 	private static FightControllerScript instance;
	public  static FightControllerScript Instance{get => instance;}
	void Awake(){
		instance = this;
	}

//API
 	public void MessageAboutDamageForAttacker(float damage){
 		listInitiative[currentHero].MessageDamageAfterStrike(damage);
 	}
 	public void DeleteHero(HeroControllerScript heroForDelete){
 		Debug.Log("delete hero");
 		Warrior warrior = leftTeam.Find(x => x.heroController == heroForDelete);
 		if(warrior != null){
 			leftTeam.Remove(warrior);
 		}else{
	 		warrior = rightTeam.Find(x => x.heroController == heroForDelete);
 			rightTeam.Remove(warrior);
 		}
 		CheckFinishFight();
 	}
 	public HeroControllerScript GetCurrentHero(){ return listInitiative[currentHero]; }

//Listeners
	private Action delsOnEndRound, delsOnStartFight, delsOnFinishFight;
	public void RegisterOnStartFight(Action d){ delsOnStartFight += d;}	 	
	public void UnregisterOnStartFight(Action d){ delsOnStartFight -= d; }
	private void PlayDelegateOnStartFight(){ if(delsOnStartFight != null) delsOnStartFight(); }

	public void RegisterOnEndRound(Action d){ delsOnEndRound += d;}	 	
	public void UnregisterOnEndRound(Action d){ delsOnEndRound -= d; }
	private void PlayDelegateEndRound(){ if(delsOnEndRound != null) delsOnEndRound(); }

	public void RegisterOnFinishFight(Action d){ delsOnFinishFight += d;}	 	
	public void UnregisterOnFinishFight(Action d){ delsOnFinishFight -= d; }
	private void PlayDelegateOnFinishFight(){ if(delsOnFinishFight != null) delsOnFinishFight(); }

	private Action<FightResult> delsFightResult;
	public void RegisterOnFightResult(Action<FightResult> d){Debug.Log("register on finish fight result"); delsFightResult += d;}
	public void UnregisterOnFightResult(Action<FightResult> d){ delsFightResult -= d;}
	public void OnFightResult(FightResult result){
		if(delsFightResult != null){
			Debug.Log("finish fight subscribe exist");
			delsFightResult(result);
			delsFightResult = null;
		}
	}

}

public enum FightResult{
	Defeat = 0,
	Win = 1
}