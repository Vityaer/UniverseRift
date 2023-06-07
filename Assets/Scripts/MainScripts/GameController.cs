using Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<HeroModel> listHeroes = new List<HeroModel>();
    public Player player;
    public bool flagLoadedGame = false;
    public Action<HeroModel> observerChangeListHeroes;
    public Action observerChangeCountHeroes;

    private static GameController instance;
    public static GameController Instance { get => instance; }

    public List<HeroModel> GetListHeroes { get => listHeroes; }
    public int GetMaxCountHeroes { get => player.PlayerGame.maxCountHeroes; }
    public int GetCurrentCountHeroes { get => listHeroes.Count; }

    void Awake()
    {
        instance = this;
        SaveLoadController.LoadGame(player.PlayerGame);
        SaveLoadController.LoadListHero(listHeroes);
        flagLoadedGame = true;
    }

    void Start()
    {
        UpdateAllResource();
        OnLoadedGame();
    }

    public void SaveGame()
    {
        Debug.Log("Save game");
        if (flagLoadedGame)
        {
            SaveLoadController.SaveGame(player.PlayerGame);
            SaveLoadController.SaveListHero(listHeroes);
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		// SaveGame();
#endif
    }

    //API List Heroes

    public void RegisterOnChangeCountHeroes(Action d) { observerChangeCountHeroes += d; }

    public void AddMaxCountHeroes(int amount)
    {
        player.PlayerGame.maxCountHeroes += amount;
        if (observerChangeCountHeroes != null) observerChangeCountHeroes();
    }



    public void GetListHeroesWithObserver(ref List<HeroModel> listHeroes, Action<HeroModel> d)
    {
        observerChangeListHeroes += d;
        listHeroes = this.listHeroes;
    }

    public void AddHero(HeroModel newHero)
    {
        newHero.PrepareLocalization();
        listHeroes.Add(newHero);
        OnChangeListHeroes(newHero);
        SaveGame();
    }

    public void RemoveHero(HeroModel removeHero)
    {
        bool flag = listHeroes.Remove(removeHero);
        if (flag) Debug.Log("герой успешно удалён!");
        OnChangeListHeroes(removeHero);
        SaveGame();
    }

    private void OnChangeListHeroes(HeroModel hero)
    {
        if (observerChangeListHeroes != null) observerChangeListHeroes(hero);
        if (observerChangeCountHeroes != null) observerChangeCountHeroes();
    }

    //API resources	
    public void AddReward(Reward reward)
    {
        GameController.Instance.AddResource(reward.GetListResource);
        InventoryController.Instance.AddItems(reward.GetItems);
        InventoryController.Instance.AddSplinters(reward.GetSplinters);
        Debug.Log("Add reward");
        UpdateAllResource();
    }

    public bool CheckResource(Resource res)
    {
        return CheckResource(new ListResource(res));
    }

    public bool CheckResource(ListResource listResource)
    {
        return player.PlayerGame.StoreResources.CheckResource(listResource);
    }

    public void AddResource(params Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
            AddResource(new ListResource(resources[i]));
    }

    public void AddResource(ListResource listResource)
    {
        player.PlayerGame.StoreResources.AddResource(listResource);
        UpdateResource(listResource);
    }

    public void AddResource(List<Resource> listResource)
    {
        player.PlayerGame.StoreResources.AddResource(listResource);
        UpdateResource(new ListResource(listResource));
    }

    public void SubtractResource(params Resource[] resources)
    {
        for (int i = 0; i < resources.Length; i++)
            SubtractResource(new ListResource(resources[i]));
    }

    public void SubtractResource(ListResource listResource)
    {
        player.PlayerGame.StoreResources.SubtractResource(listResource);
        UpdateResource(listResource);
    }

    public void SubtractResource(List<Resource> listResource)
    {
        player.PlayerGame.StoreResources.SubtractResource(listResource);
        UpdateResource(new ListResource(listResource));
    }

    public Resource GetResource(TypeResource name)
    {
        return player.PlayerGame.StoreResources.GetResource(name);
    }

    public void ClearAllResource()
    {
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
        foreach (Resource res in listResources.List)
        {
            workObserver = observersResource.Find(x => x.typeResource == res.Name);
            workObserver?.ChangeResource(player.PlayerGame.StoreResources.GetResource(res.Name));
        }
    }

    public void RegisterOnChangeResource(Action<Resource> d, TypeResource type)
    {
        bool findObserver = false;
        ObserverResource observer = new ObserverResource(TypeResource.Gold);
        foreach (ObserverResource obs in observersResource)
        {
            if (obs.typeResource == type)
            {
                findObserver = true;
                observer = obs;
                break;
            }
        }
        if (findObserver == false)
        {
            observer = new ObserverResource(type);
            observersResource.Add(observer);
        }
        observer.RegisterOnChangeResource(d);
    }

    public void UnregisterOnChangeResource(Action<Resource> d, TypeResource type)
    {
        foreach (ObserverResource obs in observersResource)
        {
            if (obs.typeResource == type)
            {
                obs.UnRegisterOnChangeResource(d);
                break;
            }
        }
    }
    public void RegisterOnLevelUP(Action<BigDigit> d) { player.RegisterOnLevelUP(d); }
    //Geters
    public static CityModel GetCitySave { get => instance.player.PlayerGame.citySaveObject; }
    public static PlayerModel GetPlayerSave { get => instance.player.PlayerGame.playerSaveObject; }
    public static Game GetPlayerGame { get => instance.player.PlayerGame; }
    public static PlayerInfoModel GetPlayerInfo { get => GetPlayerGame.playerInfo; }

    //LoadGame
    private Action onLoadedGame;
    private Action<Vector2Int> onRegisterOnRegisterOnLoading;
    private Vector2Int countObjectWaitLoadingGame = new Vector2Int(0, 0);
    public void RegiterOnRegisterOnLoadGame(Action<Vector2Int> d) { onRegisterOnRegisterOnLoading += d; OnRegisterOnLoadGame(); }
    public void UnregiterOnRegisterOnLoadGame(Action<Vector2Int> d) { onRegisterOnRegisterOnLoading -= d; }
    public void RegisterOnLoadGame(Action action)
    {
        countObjectWaitLoadingGame.y += 1;
        OnRegisterOnLoadGame();
        if (flagLoadedGame)
        {
            action();
            OnLoadStage();
        }
        else
        {
            onLoadedGame += action;
            onLoadedGame += OnLoadStage;
        }
    }

    private void OnLoadStage()
    {
        countObjectWaitLoadingGame.x += 1;
        OnRegisterOnLoadGame();
    }

    private void OnRegisterOnLoadGame()
    {
        if (onRegisterOnRegisterOnLoading != null)
            onRegisterOnRegisterOnLoading(countObjectWaitLoadingGame);
    }

    private void OnLoadedGame()
    {
        if (onLoadedGame != null)
            onLoadedGame();
        onLoadedGame = null;
    }
    //Level

    void OnApplicationFocus(bool hasFocus)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        SaveGame();
#endif
    }

    public void RegisterPlayer(string name, int playerId)
    {
        GetPlayerInfo.Register(name, playerId);
    }
}

