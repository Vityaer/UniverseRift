using IdleGame.AdvancedObservers;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpRatingHero : Building
{
    [Header("Data")]
    public LevelUpRatingHeroes listCost;
    
    [Header("UI")]
    public Button buttonLevelUP;
    public ListRequirementHeroesUI listRequirementHeroes;
    
    [SerializeField] private ResourceObjectCost objectCost;

    private InfoHero currentHero;
    private LevelUpRaiting data;
    private bool resourceDone = false;
    private bool requireHeroesDone = false;
    private ObserverActionWithHero observersRatingUp = new ObserverActionWithHero();

    public static LevelUpRatingHero Instance { get; private set; }

    protected override void OpenPage()
    {
        currentHero = TrainCamp.Instance.ReturnSelectHero();
        data = listCost.GetRequirements(currentHero);
        listRequirementHeroes.SetData(data.requirementHeroes);
        GameController.Instance.RegisterOnChangeResource(CheckResource, TypeResource.ContinuumStone);
        CheckCanUpdateRating();
    }

    public void RatingUp(int count = 1)
    {
        GameController.Instance.SubtractResource(data.Cost);
        currentHero.UpRating();
        OnRatingUp();
        listRequirementHeroes.DeleteSelectedHeroes();
        Close();
    }

    public void CheckCanUpdateRating()
    {
        resourceDone = GameController.Instance.CheckResource(data.Cost.GetResource(TypeResource.ContinuumStone));
        requireHeroesDone = listRequirementHeroes.IsAllDone();
        buttonLevelUP.interactable = (resourceDone && requireHeroesDone);
    }

    public void CheckResource(Resource res)
    {
        CheckCanUpdateRating();
    }

    public void CheckHeroes()
    {
        CheckCanUpdateRating();
    }

    protected override void ClosePage()
    {
        GameController.Instance.UnregisterOnChangeResource(CheckResource, TypeResource.ContinuumStone);
        listRequirementHeroes.ClearData();
    }

    public override void Close()
    {
        ClosePage();
        CanvasBuildingsUI.Instance.CloseBuilding(building);
    }

    public void RegisterOnRatingUp(Action<BigDigit> d, int rating, string ID = "")
    {
        observersRatingUp.Add(d, ID, rating);
    }

    public void UnregisterOnRatingUp(Action<BigDigit> d, int rating, string ID)
    { 
        observersRatingUp.Remove(d, ID, rating);
    }

    private void OnRatingUp()
    {
        observersRatingUp.OnAction(string.Empty, currentHero.generalInfo.RatingHero);
        observersRatingUp.OnAction(currentHero.generalInfo.ViewId, currentHero.generalInfo.RatingHero);
        TrainCamp.Instance.HeroPanel.UpdateInfoAbountHero();
    }

    private void Awake()
    {
        Instance = this;
    }
}
