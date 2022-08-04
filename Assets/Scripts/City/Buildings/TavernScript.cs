using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TavernScript : Building{

	[Header("All rating heroes")]
	[SerializeField] private List<InfoHero> listHeroes = new List<InfoHero>();
	public List<InfoHero> GetListHeroes{get => listHeroes;} 
	public ButtonWithObserverResource btnCostOneHire, btnCostManyHire;

//Simple hire
	private Resource simpleHireCost = new Resource(TypeResource.SimpleHireCard, 1, 0);
	public void SelectSimpleHire(){
		btnCostOneHire.ChangeCost(simpleHireCost, SimpleHireHero);
		btnCostManyHire.ChangeCost(simpleHireCost * 10f, SimpleHireHero);
	}

	private void SimpleHireHero(int count = 1){
		float rand = 0f;
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for(int i = 0; i < count; i++){
			rand          = UnityEngine.Random.Range(0f, 100f);
			if(rand < 56f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 1));
			} else if(rand < 90f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 2));
			} else if(rand < 98.5f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 3));
			} else if(rand < 99.95f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 4));
			} else if(rand <= 100f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 5));
			}
			hero = (InfoHero) (workList[ UnityEngine.Random.Range(0, workList.Count) ]).Clone();

			if(hero != null){
				hero.generalInfo.Name = hero.generalInfo.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
				AddNewHero(hero);
			}
		}
		OnSimpleHire(count);
	}
//Special hire	
	private Resource specialHireCost = new Resource(TypeResource.SpecialHireCard, 1, 0);

	
	public void SelectSpecialHire(){
		btnCostOneHire.ChangeCost(specialHireCost, SpecialHireHero);
		btnCostManyHire.ChangeCost(specialHireCost * 10f, SpecialHireHero);
	}
	public void SpecialHireHero(int count = 1){
		float rand = 0f;
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for(int i = 0; i < count; i++){
			rand    = UnityEngine.Random.Range(0f, 100f);
			if(rand < 78.42f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 3));
			} else if(rand < 98.42f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 4));
			} else if(rand <= 100f){
				workList = listHeroes.FindAll(x => (x.generalInfo.ratingHero == 5));
			}
			hero = (InfoHero) (workList[ UnityEngine.Random.Range(0, workList.Count) ]).Clone();
			if(hero != null){
				hero.generalInfo.Name = hero.generalInfo.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
				AddNewHero(hero);			
			}
		}
		OnSpecialHire(count);
	}

//Friend hire
	private Resource simpleFriendCost = new Resource(TypeResource.FriendHeart, 10, 0);
	public void SelectFriendHire(){
		btnCostOneHire.ChangeCost(simpleFriendCost, SimpleHireHero);
		btnCostManyHire.ChangeCost(simpleFriendCost * 10f, SimpleHireHero);
	}	

	public void AddNewHero(InfoHero hero){
		MessageControllerScript.Instance.AddMessage("Новый герой! Это - " + hero.generalInfo.Name);
		PlayerScript.Instance.AddHero(hero);
	}

	private static TavernScript instance;
	public static TavernScript Instance{get => instance;}
	void Awake(){ instance = this; }
	void Start(){
		CheckLoadedHeroes();
		SelectSpecialHire();
	}
	private void CheckLoadedHeroes(){
		if(listHeroes.Count == 0){
			listHeroes = new List<InfoHero>(Resources.LoadAll("ScriptableObjects/HeroesData", typeof(InfoHero)) as InfoHero[]);
		}
	}
//API
	public InfoHero GetInfoHero(int ID){
		InfoHero hero =  (InfoHero) listHeroes.Find(x => x.generalInfo.idHero == ID)?.Clone();
		if(hero == null) Debug.Log(string.Concat("not exist hero with ID= ", ID.ToString()));
		return hero;
	}
//Observers
	private Action<BigDigit> observerSimpleHire, observerSpecialHire, observerFriendHire;
	public void RegisterOnSimpleHire(Action<BigDigit> d){observerSimpleHire += d;}	 
	public void RegisterOnSpecialHire(Action<BigDigit> d){observerSpecialHire += d;}	 
	public void RegisterOnFriendHire(Action<BigDigit> d){observerFriendHire += d;}
	private void OnSimpleHire(int amount) {if(observerSimpleHire != null) observerSimpleHire(new BigDigit(amount));}	 
	private void OnSpecialHire(int amount){if(observerSpecialHire != null) observerSpecialHire(new BigDigit(amount));}	 
	private void OnFriendHire(int amount) {if(observerFriendHire != null) observerFriendHire(new BigDigit(amount));}	 
}
