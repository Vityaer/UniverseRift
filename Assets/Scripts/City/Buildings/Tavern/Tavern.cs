using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IdleGame.AdvancedObservers;
using Cysharp.Threading.Tasks;
using Network.DataServer.Messages;
using Network.DataServer;
using Network.DataServer.Models;
using ObjectSave;
using Misc.Json;
using Misc.Json.Impl;

public class Tavern : Building
{
	[Header("All rating heroes")]
	[SerializeField] private List<InfoHero> listHeroes = new List<InfoHero>();
	public Button SpecialHire;
	public Button SimpleHire;
	public Button FriendHire;
	public ButtonWithObserverResource btnCostOneHire, btnCostManyHire;
	private Resource simpleHireCost = new Resource(TypeResource.SimpleHireCard, 1, 0);
	private Resource specialHireCost = new Resource(TypeResource.SpecialHireCard, 1, 0);
    private ObserverActionWithHero observersHireRace = new ObserverActionWithHero();
	private IJsonConverter _jsonConverter;
    public List<InfoHero> GetListHeroes => listHeroes;
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
		InfoHero hero = null;
		var message = new SimpleHire { PlayerId = GameController.GetPlayerInfo.PlayerId, Count = count };
		var result = await DataServer.PostData(message);
		var newHeroes = _jsonConverter.FromJson<DataHero[]>(result);

		for (int i = 0; i < newHeroes.Length; i++)
		{
			var heroSave = new HeroSave(newHeroes[i]);
			hero = new InfoHero(heroSave);
			OnHireHeroes(hero);
			if (hero != null)
			{
				hero.generalInfo.Name = $"{hero.generalInfo.HeroId} #{UnityEngine.Random.Range(0, 1000)}";
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
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for (int i = 0; i < count; i++)
		{
			rand = UnityEngine.Random.Range(0f, 100f);
			if (rand < 60f)
			{
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.R));
			}
			else if (rand < 88.42f)
			{
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SR));
			}
			else if (rand < 98.42f)
			{
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.SSR));
			}
			else if (rand <= 100f)
			{
				workList = listHeroes.FindAll(x => (x.generalInfo.Rare == Rare.UR));
			}
			hero = (InfoHero)(workList[UnityEngine.Random.Range(0, workList.Count)]).Clone();
			if (hero != null)
			{
				hero.generalInfo.Name = $"{hero.generalInfo.Name} № {UnityEngine.Random.Range(0, 1000)}";
				AddNewHero(hero);
			}
			OnHireHeroes(hero);
		}
		OnSpecialHire(count);
	}

	//Friend hire
	private Resource simpleFriendCost = new Resource(TypeResource.FriendHeart, 10, 0);
	public void SelectFriendHire()
	{
		btnCostOneHire.ChangeCost(simpleFriendCost, StartSimpleHire);
		btnCostManyHire.ChangeCost(simpleFriendCost * 10f, StartSimpleHire);
	}

	public void AddNewHero(InfoHero hero)
	{
		MessageController.Instance.AddMessage($"Новый герой! Это - {hero.generalInfo.Name}");
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

	public InfoHero GetInfoHero(string ID)
	{
		InfoHero hero = (InfoHero)listHeroes.Find(x => x.generalInfo.HeroId == ID)?.Clone();
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

	public void RegisterOnHireHeroes(Action<BigDigit> d, Rare rare, string ID = "")
	{
		observersHireRace.Add(d, ID, (int)rare);
	}

	private void OnHireHeroes(InfoHero hero)
	{
		observersHireRace.OnAction(string.Empty, (int)hero.generalInfo.Rare);
		observersHireRace.OnAction(hero.generalInfo.ViewId, (int)hero.generalInfo.Rare);
	}

}
