using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AltarScript : BuildingWithHeroesList{


	protected override void OpenPage(){
		if(listHeroesController.LoadedListHeroes == false) {
			PlayerScript.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
			LoadListHeroes();
		}
		listHeroesController.EventOpen();
		listHeroesController.RegisterOnSelect(SelectHero);
		listHeroesController.RegisterOnUnSelect(UnselectHero);
	}

	protected override void ClosePage(){
		for(int i=0; i < selectedHeroCards.Count; i++) selectedHeroCards[i].UnSelected();
		selectedHeroCards.Clear();
		listHeroesController.EventClose();
	}
	public void MusterOut(){
		List<InfoHero> heroes = new List<InfoHero>();
		OnDissamble(selectedHeroCards.Count);
		foreach(CardScript card in selectedHeroCards) {
			heroes.Add(card.hero);
			card.UnSelected();
		}
		selectedHeroCards.Clear();
		GetRewardFromHeroes(heroes);
	}
	private void GetRewardFromHeroes(List<InfoHero> heroes){
		float rewardTicket = 0, rewardStone = 0;
		foreach(InfoHero hero in heroes){
			if(hero.generalInfo.ratingHero > 2)
				rewardTicket = ((int) hero.generalInfo.ratingHero + (int) hero.generalInfo.rare) * 5;
			rewardStone = ((int) hero.generalInfo.ratingHero + (int) hero.generalInfo.rare) * 6;	
			PlayerScript.Instance.RemoveHero(hero);
		}         
		Resource ticket = new Resource(TypeResource.TicketGrievance, rewardTicket, 0),
		         stone = new Resource(TypeResource.ForceStone, rewardStone, 0);

		PlayerScript.Instance.AddResource( ticket, stone );
	}

	private List<CardScript> selectedHeroCards = new List<CardScript>();
	public override void SelectHero(CardScript cardHero){
		selectedHeroCards.Add(cardHero);
		cardHero.Selected();
	}
	public override void UnselectHero(CardScript cardHero){
		selectedHeroCards.Remove(cardHero);
		cardHero.UnSelected();
	}
	
//Observers
	private Action<BigDigit> observerDissamble;
	public void RegisterOnDissamble(Action<BigDigit> d){observerDissamble += d;}	 
	private void OnDissamble(int amount) {if(observerDissamble != null) observerDissamble(new BigDigit(amount));}	 


}
