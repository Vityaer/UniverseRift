using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelUpRatingHeroScript : Building{
	[Header("Data")]
	public LevelUpRatingHeroes listCost;
	[Header("UI")]
	public Button buttonLevelUP;
	public ListRequirementHeroesUI listRequirementHeroes;
	private InfoHero currentHero;
	[SerializeField] private ResourceObjectCost objectCost;
	LevelUpRaiting data;
	protected override void OpenPage(){
		currentHero = TrainCampScript.Instance.ReturnSelectHero();
		if(currentHero == null) Debug.Log("currentHero null");
		data = listCost.GetRequirements(currentHero);
		listRequirementHeroes.SetData( data.requirementHeroes );
		PlayerScript.Instance.RegisterOnChangeResource( CheckResource, TypeResource.ContinuumStone );
	}
	public void RatingUp(int count = 1){
		PlayerScript.Instance.SubtractResource(data.Cost);
		listRequirementHeroes.DeleteSelectedHeroes();
		currentHero.UpRating();
		Close();
		Debug.Log("rating up");
	}
	public void CheckCanUpdateRating(){
		resourceDone = PlayerScript.Instance.CheckResource( data.Cost.GetResource(TypeResource.ContinuumStone) );
		requireHeroesDone = listRequirementHeroes.IsAllDone();
		buttonLevelUP.interactable = (resourceDone && requireHeroesDone);
	}
	bool resourceDone = false, requireHeroesDone = false;
	public void CheckResource(Resource res){ CheckCanUpdateRating(); }
	public void CheckHeroes(){ CheckCanUpdateRating(); }
	protected virtual void ClosePage(){
		PlayerScript.Instance.UnRegisterOnChangeResource( CheckResource, TypeResource.ContinuumStone );
		listRequirementHeroes.ClearData();
	} 
	public override void Close(){
		ClosePage();
		if(building != null){ CanvasBuildingsUI.Instance.CloseBuilding(building);  }
	}
}