using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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
		OnRatingUp();
		Debug.Log("rating up");
		Close();
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
	private ObserverRatingUp observersRatingUp = new ObserverRatingUp();
	public void RegisterOnRatingUp(Action<BigDigit> d, int rating, int ID = 0){observersRatingUp.Add(d, ID, rating);}
	public void UnregisterOnRatingUp(Action<BigDigit> d, int rating, int ID = 0){observersRatingUp.Remove(d, ID, rating);}
	private void OnRatingUp(){
		observersRatingUp.OnRatingUp(0, currentHero.generalInfo.ratingHero);
		observersRatingUp.OnRatingUp(currentHero.generalInfo.idHero, currentHero.generalInfo.ratingHero);
	}
	

	private static LevelUpRatingHeroScript instance;
	public static LevelUpRatingHeroScript Instance{get => instance;}
	void Awake(){instance = this;}
	
	public class ObserverRatingUp{
		private List<Observer> observers = new List<Observer>();
		public void Add(Action<BigDigit> del, int ID, int rating){
			Observer work = GetObserver(ID, rating);
			if(work != null){
				work.Add(del);
			}else{
				observers.Add(new Observer(del, ID, rating));
			}
		}
		public void Remove(Action<BigDigit> del,int ID, int rating){
			Observer work = GetObserver(ID, rating);
			if(work != null){
				work.Remove(del);
				if(work.del == null){
					observers.Remove(work);
				}	
			}
		}
		public void OnRatingUp(int ID, int rating){
			Observer work = GetObserver(ID, rating); 
			if(work != null){
				work.DoAction();
			}
		}
		private Observer GetObserver(int ID, int rating){
			return observers.Find(x => (x.rating == rating) && (x.ID == ID));
		}

		public class Observer{
			public Action<BigDigit> del;
			public int rating;
			public int ID;
			public Observer(Action<BigDigit> d, int ID, int rating){
				del = d;
				this.ID = ID;
				this.rating = rating;
			}
			public void Add(Action<BigDigit> d){ del += d; }
			public void Remove(Action<BigDigit> d){ del -= d; }
			public void DoAction(){
				if(del != null) del(new BigDigit(1));
			}
		}
	}
}
