using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IdleGame.AdvancedObservers;
public class LevelUpRatingHeroScript : Building
{
	[Header("Data")]
	public LevelUpRatingHeroes listCost;
	[Header("UI")]
	public Button buttonLevelUP;
	public ListRequirementHeroesUI listRequirementHeroes;
	private InfoHero currentHero;
	[SerializeField] private ResourceObjectCost objectCost;
	private LevelUpRaiting data;
	private bool resourceDone = false, requireHeroesDone = false;

	protected override void OpenPage()
	{
		currentHero = TrainCampScript.Instance.ReturnSelectHero();
		if(currentHero == null) Debug.Log("currentHero null");
		data = listCost.GetRequirements(currentHero);
		listRequirementHeroes.SetData( data.requirementHeroes );
		PlayerScript.Instance.RegisterOnChangeResource( CheckResource, TypeResource.ContinuumStone );
		CheckCanUpdateRating();
	}

	public void RatingUp(int count = 1)
	{
		PlayerScript.Instance.SubtractResource(data.Cost);
		listRequirementHeroes.DeleteSelectedHeroes();
		currentHero.UpRating();
		OnRatingUp();
		Debug.Log("rating up");
		Close();
	}

	public void CheckCanUpdateRating()
	{
		resourceDone = PlayerScript.Instance.CheckResource( data.Cost.GetResource(TypeResource.ContinuumStone) );
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
	
	protected virtual void ClosePage()
	{
		PlayerScript.Instance.UnRegisterOnChangeResource( CheckResource, TypeResource.ContinuumStone );
		listRequirementHeroes.ClearData();
	} 

	public override void Close()
	{
		ClosePage();
		CanvasBuildingsUI.Instance.CloseBuilding(building);
	}

	private ObserverActionWithHero observersRatingUp = new ObserverActionWithHero();
	public void RegisterOnRatingUp(Action<BigDigit> d, int rating, int ID = 0){observersRatingUp.Add(d, ID, rating);}
	public void UnregisterOnRatingUp(Action<BigDigit> d, int rating, int ID = 0){observersRatingUp.Remove(d, ID, rating);}
	private void OnRatingUp(){
		observersRatingUp.OnAction(0, currentHero.generalInfo.ratingHero);
		observersRatingUp.OnAction(currentHero.generalInfo.idHero, currentHero.generalInfo.ratingHero);
	}
	

	private static LevelUpRatingHeroScript instance;
	public static LevelUpRatingHeroScript Instance{get => instance;}
	private void Awake(){instance = this;}
	
	
}
