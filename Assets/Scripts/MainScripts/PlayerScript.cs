using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using System;
public class PlayerScript : MonoBehaviour{
	[SerializeField] private List<InfoHero> listHeroes = new List<InfoHero>();
	public Player player;
	public bool flagLoadedGame = false;
	public Action<InfoHero> observerChangeListHeroes;
	public Action observerChangeCountHeroes;

	private static PlayerScript instance;
	public  static PlayerScript Instance{get => instance;}

	public List<InfoHero> GetListHeroes{get => listHeroes;}
	public int GetMaxCountHeroes{get => player.PlayerGame.maxCountHeroes;}
	public int GetCurrentCountHeroes{get => listHeroes.Count;}

	void Awake(){ 
		instance = this;
		SaveLoadControllerScript.LoadGame(player.PlayerGame);
	}

	void Start(){
		SaveLoadControllerScript.LoadListHero(listHeroes);
		UpdateAllResource();
		flagLoadedGame = true;
		OnLoadedGame();
	}

	public void SaveGame(){
		Debug.Log("Save game");
		if(flagLoadedGame){
			SaveLoadControllerScript.SaveGame(player.PlayerGame);
			SaveLoadControllerScript.SaveListHero(listHeroes);
		}
	}
	void OnApplicationPause(bool pauseStatus){
		#if UNITY_ANDROID && !UNITY_EDITOR
		// SaveGame();
		#endif
	}

//API List Heroes

	public void RegisterOnChangeCountHeroes(Action d){observerChangeCountHeroes += d;}

	public void AddMaxCountHeroes(int amount){
		player.PlayerGame.maxCountHeroes += amount;
		if(observerChangeCountHeroes != null) observerChangeCountHeroes();
	}



	public void GetListHeroesWithObserver(ref List<InfoHero> listHeroes, Action<InfoHero> d){
		observerChangeListHeroes += d;
		listHeroes = this.listHeroes;
	}

	public void AddHero(InfoHero newHero){
		newHero.PrepareLocalization();
		listHeroes.Add(newHero);
		OnChangeListHeroes(newHero);
		SaveGame();
	}

	public void RemoveHero(InfoHero removeHero){
		bool flag = listHeroes.Remove(removeHero);
		if(flag) Debug.Log("герой успешно удалён!");
		OnChangeListHeroes(removeHero);
		SaveGame();
	}	

	private void OnChangeListHeroes(InfoHero hero){
		if(observerChangeListHeroes != null) observerChangeListHeroes(hero);
		if(observerChangeCountHeroes != null) observerChangeCountHeroes();
	}

//API resources	
	public void AddReward(Reward reward){
		PlayerScript.Instance.AddResource(reward.GetListResource);
		InventoryControllerScript.Instance.AddItems(reward.GetItems);
		InventoryControllerScript.Instance.AddSplinters(reward.GetSplinters);
		Debug.Log("Add reward");
		UpdateAllResource();
	}

	public bool CheckResource(Resource res){
		return CheckResource(new ListResource(res));
	}

	public bool CheckResource(ListResource listResource){
		return player.PlayerGame.StoreResources.CheckResource(listResource);
	}

	public void AddResource(params Resource[] resources){
		for(int i = 0; i < resources.Length; i++)
			AddResource(new ListResource(resources[i]));
	}

	public void AddResource(ListResource listResource){
		player.PlayerGame.StoreResources.AddResource(listResource);
		UpdateResource(listResource);
	}

	public void AddResource(List<Resource> listResource){
		player.PlayerGame.StoreResources.AddResource(listResource);
		UpdateResource(new ListResource(listResource));
	}

	public void SubtractResource(params Resource[] resources){
		for(int i = 0; i < resources.Length; i++)
			SubtractResource(new ListResource(resources[i]));
	}

	public void SubtractResource(ListResource listResource){
		player.PlayerGame.StoreResources.SubtractResource(listResource);
		UpdateResource(listResource);
	}

	public void SubtractResource(List<Resource> listResource){
		player.PlayerGame.StoreResources.SubtractResource(listResource);
		UpdateResource(new ListResource(listResource));
	}
	
	public Resource GetResource(TypeResource name){
		return player.PlayerGame.StoreResources.GetResource(name);
	}

	public void ClearAllResource(){
		player.PlayerGame.StoreResources.Clear();
		UpdateAllResource();
	}

	List<ObserverResource> observersResource = new List<ObserverResource>();

	public void UpdateAllResource()
	{
		UpdateResource(player.PlayerGame.StoreResources);
	}

	public void UpdateResource(ListResource listResources)
	{
		ObserverResource workObserver = null;
		foreach(Resource res in listResources.List)
		{
			workObserver = observersResource.Find(x => x.typeResource == res.Name);
			workObserver?.ChangeResource(player.PlayerGame.StoreResources.GetResource(res.Name));
		}
	}

	public void RegisterOnChangeResource(Action<Resource> d, TypeResource type)
	{
		bool findObserver = false;
		ObserverResource observer = new ObserverResource(TypeResource.Gold);
		foreach(ObserverResource obs in observersResource){
			if(obs.typeResource == type){
				findObserver = true;
				observer = obs;
				break;
			}
		}
		if(findObserver == false) {
			observer = new ObserverResource(type);
			observersResource.Add(observer);
		}	
		observer.RegisterOnChangeResource(d);
	}

	public void UnRegisterOnChangeResource(Action<Resource> d, TypeResource type){
		foreach(ObserverResource obs in observersResource){
			if(obs.typeResource == type){
				obs.UnRegisterOnChangeResource(d);
				break;
			}
		}
	}
	public void RegisterOnLevelUP(Action<BigDigit> d){ player.RegisterOnLevelUP(d); }
//Geters
	public static CitySaveObject GetCitySave{ get => instance.player.PlayerGame.citySaveObject;}
	public static PlayerSaveObject GetPlayerSave{ get => instance.player.PlayerGame.playerSaveObject;}
	public static Game GetPlayerGame{ get => instance.player.PlayerGame;}
	public static PlayerInfo GetPlayerInfo{get => GetPlayerGame.playerInfo;}

//LoadGame
	private Action onLoadedGame;
	private Action<Vector2Int> onRegisterOnRegisterOnLoading;
	private Vector2Int countObjectWaitLoadingGame = new Vector2Int(0, 0);
	public void RegiterOnRegisterOnLoadGame(Action<Vector2Int> d){onRegisterOnRegisterOnLoading += d; OnRegisterOnLoadGame();}
	public void UnregiterOnRegisterOnLoadGame(Action<Vector2Int> d){onRegisterOnRegisterOnLoading -= d;}
	public void RegisterOnLoadGame(Action d){
		countObjectWaitLoadingGame.y += 1;
		OnRegisterOnLoadGame();
		if(flagLoadedGame){
			d(); OnLoadStage();
		}else{
			onLoadedGame += d;
			onLoadedGame += OnLoadStage;
		}
	}
	private void OnLoadStage(){
		countObjectWaitLoadingGame.x += 1;
		OnRegisterOnLoadGame();
	}
	private void OnRegisterOnLoadGame(){
		if(onRegisterOnRegisterOnLoading != null)
			onRegisterOnRegisterOnLoading(countObjectWaitLoadingGame);
	}
	private void OnLoadedGame(){if(onLoadedGame != null) onLoadedGame(); onLoadedGame = null;}
//Level

	void OnApplicationFocus(bool hasFocus){
		#if UNITY_ANDROID && !UNITY_EDITOR
        SaveGame();
        #endif
    }
	public void RegisterPlayer(string name){
		GetPlayerInfo.Register(name);
	}    
}

