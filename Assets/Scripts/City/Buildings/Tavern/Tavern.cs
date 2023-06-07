using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IdleGame.AdvancedObservers;
using Cysharp.Threading.Tasks;
using Network.DataServer.Messages;
using Network.DataServer;
using Network.DataServer.Models;
using Models;
using Misc.Json;
using Misc.Json.Impl;

public class Tavern : Building
{
	[Header("All rating heroes")]
	[SerializeField] private List<HeroModel> listHeroes = new List<HeroModel>();
	public Button SpecialHire;
	public Button SimpleHire;
	public Button FriendHire;
	public ButtonWithObserverResource btnCostOneHire, btnCostManyHire;
	private Resource simpleHireCost = new Resource(TypeResource.SimpleHireCard, 1, 0);
	private Resource specialHireCost = new Resource(TypeResource.SpecialHireCard, 1, 0);
    private ObserverActionWithHero observersHireRace = new ObserverActionWithHero();
	private IJsonConverter _jsonConverter;
    public List<HeroModel> GetListHeroes => listHeroes;
	public static Tavern Instance { get; private set; }

	void Awake()
	{
		Instance = this;
		_jsonConverter = new JsonConverter();

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
		btnCostOneHire.ChangeCost(simpleHireCost, StartSimpleHire);
		btnCostManyHire.ChangeCost(simpleHireCost * 10f, _ => StartSimpleHire(10));
	}

	private void StartSimpleHire(int count = 1)
	{
		SimpleHireHero(count).Forget();
	}

	private async UniTaskVoid SimpleHireHero(int count = 1)
	{
		HeroModel hero = null;
		var message = new SimpleHire { PlayerId = GameController.GetPlayerInfo.PlayerId, Count = count };
		var result = await DataServer.PostData(message);
		var newHeroes = _jsonConverter.FromJson<DataHero[]>(result);

		for (int i = 0; i < newHeroes.Length; i++)
		{
			var heroSave = new HeroData(newHeroes[i]);
			hero = new HeroModel(heroSave);
			OnHireHeroes(hero);
			if (hero != null)
			{
				hero.General.Name = $"{hero.General.HeroId} #{UnityEngine.Random.Range(0, 1000)}";
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

	public void SpecialHireHero(int count = 1)
	{
		float rand = 0f;
		HeroModel hero = null;
		List<HeroModel> workList = new List<HeroModel>();
		
		OnSpecialHire(count);
	}

	//Friend hire
	private Resource simpleFriendCost = new Resource(TypeResource.FriendHeart, 10, 0);
	public void SelectFriendHire()
	{
		btnCostOneHire.ChangeCost(simpleFriendCost, StartSimpleHire);
		btnCostManyHire.ChangeCost(simpleFriendCost * 10f, StartSimpleHire);
	}

	public void AddNewHero(HeroModel hero)
	{
		MessageController.Instance.AddMessage($"Новый герой! Это - {hero.General.Name}");
		GameController.Instance.AddHero(hero);
	}


	private void CheckLoadedHeroes()
	{
		if (listHeroes.Count == 0)
		{
			//listHeroes = new List<InfoHero>(Resources.LoadAll("ScriptableObjects/HeroesData", typeof(InfoHero)) as InfoHero[]);
		}
	}
	//API

	public HeroModel GetInfoHero(string ID)
	{
		HeroModel hero = (HeroModel)listHeroes.Find(x => x.General.HeroId == ID)?.Clone();
		if (hero == null)
			Debug.Log(string.Concat("not exist hero with ID= ", ID.ToString()));
		return hero;
	}

	//Observers
	private Action<BigDigit> observerSimpleHire, observerSpecialHire, observerFriendHire;
	public void RegisterOnSimpleHire(Action<BigDigit> d) { observerSimpleHire += d; }
	public void RegisterOnSpecialHire(Action<BigDigit> d) { observerSpecialHire += d; }
	public void RegisterOnFriendHire(Action<BigDigit> d) { observerFriendHire += d; }
	private void OnSimpleHire(int amount)
	{
		if (observerSimpleHire != null)
			observerSimpleHire(new BigDigit(amount));
	}

	private void OnSpecialHire(int amount)
	{
		if (observerSpecialHire != null)
			observerSpecialHire(new BigDigit(amount));
	}

	private void OnFriendHire(int amount)
	{
		if (observerFriendHire != null)
			observerFriendHire(new BigDigit(amount));
	}

	public void RegisterOnHireHeroes(Action<BigDigit> d, string ID = "")
	{
		observersHireRace.Add(d, ID, 1);
	}

	private void OnHireHeroes(HeroModel hero)
	{
		//and Rarity
		observersHireRace.OnAction(string.Empty, 1);
		observersHireRace.OnAction(hero.General.ViewId, 1);
	}

}
