using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelpFuction;
using UnityEngine.UI;
using TMPro;
using System;
using Fight.Grid;

public partial class FightControllerScript : MonoBehaviour{
	
	[Header("UI")]
	public Canvas canvasFightUI;
 	public TextMeshProUGUI textNumRound;
	
	[Header("Place heroes")]
	public GridController Grid;
	[SerializeField]private  List<Warrior> leftTeam  = new List<Warrior>(); 
	[SerializeField]private  List<Warrior> rightTeam = new List<Warrior>(); 
	[SerializeField] private List<HeroControllerScript> listInitiative  = new List<HeroControllerScript>();
 	[SerializeField] private List<HeroControllerScript> listHeroesWithAction = new List<HeroControllerScript>(); 

	[Header("Location")]
	public LocationControllerScript locationController;

	private bool isFightFinish = false;
 	private int currentHero = -1;
 	private  int round = 1;
 	public int MaxCountRound = 3;
 	private Mission mission;

	public List<Warrior> GetLeftTeam{get => leftTeam;}
	public List<Warrior> GetRightTeam{get => rightTeam;}

//Create Teams
	public void SetMission(Mission mission, List<WarriorPlaceScript> leftWarriorPlace, List<WarriorPlaceScript> rightWarriorPlace)
	{
		this.mission = mission;
		locationController.OpenLocation( mission.location );
		Grid.OpenGrid();
		AIController.Instance.StartAIFight();
		CreateTeams(leftWarriorPlace, rightWarriorPlace);
	}

    private void CreateTeams(List<WarriorPlaceScript> leftWarriorPlace, List<WarriorPlaceScript> rightWarriorPlace)
    {
    	canvasFightUI.enabled = true;
    	CreateTeam(GridController.Instance.GetLeftTeamPos,  leftWarriorPlace,  Side.Left );
    	CreateTeam(GridController.Instance.GetRightTeamPos, rightWarriorPlace, Side.Right );
    	listInitiative.Sort(new HeroInitiativeComparer());
    	StartCoroutine(StartFightCountdown());
    }

    IEnumerator StartFightCountdown()
    {
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

    private void CreateTeam(List<HexagonCellScript> teamPos, List<WarriorPlaceScript> team, Side side)
    {
    	HeroControllerScript heroScript;
    	GameObject currentHeroPrefab;
    	for(int i=0; i < team.Count; i++)
		{
			currentHeroPrefab = null;
			heroScript = null;
			
			if((side == Side.Left && (team[i].card != null)) || (team[i].Hero != null))
				currentHeroPrefab = Instantiate(team[i].Hero.generalInfo.Prefab, teamPos[i].Position, Quaternion.identity, GridController.Instance.ParentTemplateObjects);
			
			if(currentHeroPrefab != null){
				heroScript = currentHeroPrefab.GetComponent<HeroControllerScript>(); 
				List<Warrior> workTeam = (side == Side.Left) ? leftTeam : rightTeam;
				workTeam.Add(new Warrior(heroScript));
				heroScript.SetHero(team[i].Hero, teamPos[i], side);
				listInitiative.Add(heroScript);
			}
    	}
    }

//Fight loop    	
 	public void StartFight()
 	{
 		NextHero();
 	} 
 	
 	public void AddHeroWithAction(HeroControllerScript newHero)
 	{
 		listHeroesWithAction.Add(newHero);
 	}

 	public void RemoveHeroWithAction(HeroControllerScript removeHero)
 	{
 		listHeroesWithAction.Remove(removeHero);
 		if(listHeroesWithAction.Count == 0) NextHero();
 	}

 	public void RemoveHeroWithActionAll(HeroControllerScript removeHero)
 	{
 		listHeroesWithAction.RemoveAll(x => x == removeHero);
 		if(listHeroesWithAction.Count == 0) NextHero();
 	}
 	
 	private void NextHero()
 	{
 		if((currentHero + 1) < listInitiative.Count){
 			currentHero++;
 		}else{
 			NewRound();
 		} 
 		if(isFightFinish == false){
			listInitiative[currentHero].DoTurn();
 		}
 	}

 	private void NewRound()
 	{
 		UpdateListInitiative();
 		currentHero = 0;
		round++;
		textNumRound.text = string.Concat("Round ", round.ToString()); 
		PlayDelegateEndRound();
		if(round == MaxCountRound){
			Win(Side.Right);
		}
 	}

 	private void UpdateListInitiative()
 	{
 		listInitiative.Clear();
 		for(int i = 0; i < leftTeam.Count; i++){
 			if(leftTeam[i].heroController != null){
	 			listInitiative.Add(leftTeam[i].heroController);
 			}else{
 				Debug.Log("left team hero null");
 			}
 		}
 		for(int i = 0; i < rightTeam.Count; i++){
 			if(rightTeam[i].heroController != null){
	 			listInitiative.Add(rightTeam[i].heroController);
 			}else{
 				Debug.Log("Right team hero null");
 			}
 		}
 		listInitiative.Sort(new HeroInitiativeComparer());
 	}

 	public void WaitTurn()
 	{
 		HeroControllerScript hero = GetCurrentHero();
 		listInitiative.Remove(hero);
 		currentHero -= 1;
 		listInitiative.Add(hero);
 		hero.StartWait(); 
 	}

//Result fight
 	private void CheckFinishFight()
 	{
 		if((leftTeam.Count == 0) || (rightTeam.Count == 0))
 			Win(leftTeam.Count > 0 ? Side.Left : Side.Right);
 	}

 	void Win(Side side)
 	{
 		isFightFinish = true;
 		PlayDelegateOnFinishFight();
 		if(side == Side.Left){
 			MessageControllerScript.Instance.AddMessage("Ты выиграл!");
 		}else{
 			MessageControllerScript.Instance.AddMessage("Ты проиграл!");
 			
 		}
    	StartCoroutine(FinishFightCountdown(side));

 	}

 	IEnumerator FinishFightCountdown(Side side)
 	{
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

    private void CloseFigthUI()
    {
 		canvasFightUI.enabled = false;
    }

 	void ClearAll()
 	{
 		DeleteTeam(rightTeam);
 		DeleteTeam(leftTeam);
		listInitiative.Clear();
		listHeroesWithAction.Clear();
		currentHero = -1; 
		round = 1;
 	}

 	void DeleteTeam(List<Warrior> team)
 	{
 		for (int i = 0; i < team.Count;  i++)
 		{
 			team[i].heroController.DeleteHero();
		}
		team.Clear();
 	}

 	void CheckSaveResult()
 	{ 
 		if((mission is BossMission)) 
 			(mission as BossMission).SaveResult(); 
 	}

 	private static FightControllerScript instance;
	public  static FightControllerScript Instance{get => instance;}

	void Awake()
	{
		instance = this;
	}

//API
 	public void MessageAboutDamageForAttacker(float damage){
 		listInitiative[currentHero].MessageDamageAfterStrike(damage);
 	}
 	public void DeleteHero(HeroControllerScript heroForDelete)
 	{
		RemoveHeroWithActionAll(heroForDelete);
 		Warrior warrior = leftTeam.Find(x => x.heroController == heroForDelete);
 		if(warrior != null){
 			leftTeam.Remove(warrior);
 		}else{
	 		warrior = rightTeam.Find(x => x.heroController == heroForDelete);
 			rightTeam.Remove(warrior);
 		}
 		listInitiative.Remove(heroForDelete);
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
	
	public void OnFightResult(FightResult result)
	{
		if(delsFightResult != null){
			delsFightResult(result);
			delsFightResult = null;
		}
	}

}

public enum FightResult{
	Defeat = 0,
	Win = 1
}