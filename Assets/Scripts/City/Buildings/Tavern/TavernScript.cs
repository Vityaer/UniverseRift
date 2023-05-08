using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IdleGame.AdvancedObservers;
public class TavernScript : Building{

	[Header("All rating heroes")]
	[SerializeField] private List<InfoHero> listHeroes = new List<InfoHero>();
	public Button SpecialHire;
	public Button SimpleHire;
	public Button FriendHire;
	public ButtonWithObserverResource btnCostOneHire, btnCostManyHire;
	private Resource simpleHireCost = new Resource(TypeResource.SimpleHireCard, 1, 0);
	private Resource specialHireCost = new Resource(TypeResource.SpecialHireCard, 1, 0);

	public List<InfoHero> GetListHeroes => listHeroes; 
	public static TavernScript Instance{get; private set;}

	void Awake()
	{
		Instance = this;
	}

	protected override void OnStart()
	{
		CheckLoadedHeroes();
		SelectSpecialHire();
		SimpleHire.onClick.AddListener(SelectSimpleHire);
		SpecialHire.onClick.AddListener(SelectSpecialHire);
	}

//Simple hire

	public void SelectSimpleHire()
	{
		btnCostOneHire.ChangeCost(simpleHireCost, SimpleHireHero);
		btnCostManyHire.ChangeCost(simpleHireCost * 10f, _ => SimpleHireHero(10));
	}

	private void SimpleHireHero(int count = 1)
	{
		float rand = 0f;
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for(int i = 1; i <= count; i++){
			rand = UnityEngine.Random.Range(0f, 100f);
			if(rand < 56f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.C));
			} else if(rand < 90f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.UC));
			} else if(rand < 98.5f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.R));
			} else if(rand < 99.95f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SR));
			} else if(rand <= 100f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SSR));
			}
			if(workList.Count > 0){
				hero = (InfoHero) (workList[ UnityEngine.Random.Range(0, workList.Count) ]).Clone();
			}else{
				hero = (InfoHero) (listHeroes[ UnityEngine.Random.Range(0, listHeroes.Count) ]).Clone();
			}
			OnHireHeroes(hero);
			if(hero != null){
				hero.generalInfo.Name = $"{hero.generalInfo.Name} № {UnityEngine.Random.Range(0, 1000)}";
				AddNewHero(hero);
			}
		}
		OnSimpleHire(count);
	}

//Special hire	

	
	public void SelectSpecialHire()
	{
		btnCostOneHire.ChangeCost(specialHireCost, SpecialHireHero);
		btnCostManyHire.ChangeCost(specialHireCost * 10f, _ => SpecialHireHero(10));
	}

	public void SpecialHireHero(int count = 1){
		float rand = 0f;
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for(int i = 0; i < count; i++){
			rand    = UnityEngine.Random.Range(0f, 100f);
			if(rand < 60f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.R));
			}else if(rand < 88.42f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SR));
			} else if(rand < 98.42f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SSR));
			} else if(rand <= 100f){
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.UR));
			}
			hero = (InfoHero) (workList[ UnityEngine.Random.Range(0, workList.Count) ]).Clone();
			if(hero != null){
				hero.generalInfo.Name = $"{hero.generalInfo.Name} № {UnityEngine.Random.Range(0, 1000)}";
				AddNewHero(hero);			
			}
			OnHireHeroes(hero);
		}
		OnSpecialHire(count);
	}

//Friend hire
	private Resource simpleFriendCost = new Resource(TypeResource.FriendHeart, 10, 0);
	public void SelectFriendHire(){
		btnCostOneHire.ChangeCost(simpleFriendCost, SimpleHireHero);
		btnCostManyHire.ChangeCost(simpleFriendCost * 10f, SimpleHireHero);
	}	

	public void AddNewHero(InfoHero hero)
	{
		MessageController.Instance.AddMessage($"Новый герой! Это - {hero.generalInfo.Name}");
		GameController.Instance.AddHero(hero);
	}


	private void CheckLoadedHeroes()
	{
		if(listHeroes.Count == 0)
		{
			listHeroes = new List<InfoHero>(Resources.LoadAll("ScriptableObjects/HeroesData", typeof(InfoHero)) as InfoHero[]);
		}
	}
//API

	public InfoHero GetInfoHero(string ID)
	{
		InfoHero hero =  (InfoHero) listHeroes.Find(x => x.generalInfo.ViewId == ID)?.Clone();
		if(hero == null)
			Debug.Log(string.Concat("not exist hero with ID= ", ID.ToString()));
		return hero;
	}

//Observers
	private Action<BigDigit> observerSimpleHire, observerSpecialHire, observerFriendHire;
	public void RegisterOnSimpleHire(Action<BigDigit> d){observerSimpleHire += d;}	 
	public void RegisterOnSpecialHire(Action<BigDigit> d){observerSpecialHire += d;}	 
	public void RegisterOnFriendHire(Action<BigDigit> d){observerFriendHire += d;}
	private void OnSimpleHire(int amount)
	{
		if(observerSimpleHire != null) 
			observerSimpleHire(new BigDigit(amount));
	}	

	private void OnSpecialHire(int amount)
	{
		if(observerSpecialHire != null)
			observerSpecialHire(new BigDigit(amount));
	}	

	private void OnFriendHire(int amount)
	{
		if(observerFriendHire != null)
			observerFriendHire(new BigDigit(amount));
	}	 
	
	private ObserverActionWithHero observersHireRace = new ObserverActionWithHero();
	public void RegisterOnHireHeroes(Action<BigDigit> d, Rare rare, string ID = "")
	{
		observersHireRace.Add(d, ID, (int) rare);
	}
	
	private void OnHireHeroes(InfoHero hero){
		observersHireRace.OnAction(string.Empty, (int) hero.generalInfo.Rare);
		observersHireRace.OnAction(hero.generalInfo.ViewId, (int) hero.generalInfo.Rare);
	}

}
