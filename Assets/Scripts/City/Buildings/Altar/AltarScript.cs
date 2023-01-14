using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Altar
{
	public class AltarScript : BuildingWithHeroesList{

		private List<CardScript> selectedHeroCards = new List<CardScript>();
		[SerializeField] private List<AltarReward> _templateRewards = new List<AltarReward>();

		void Start()
		{
			listHeroesController.RegisterOnSelect(SelectHero);
			listHeroesController.RegisterOnUnSelect(UnselectHero);
		}

		protected override void OpenPage(){
			listHeroes = PlayerScript.Instance.GetListHeroes;
			LoadListHeroes();
			listHeroesController.EventOpen();
		}

		protected override void ClosePage(){
			for(int i=0; i < selectedHeroCards.Count; i++) selectedHeroCards[i].UnSelected();
			selectedHeroCards.Clear();
			listHeroesController.EventClose();
		}

		public void FiredHeroes(){
			List<InfoHero> heroes = new List<InfoHero>();

			OnDissamble(selectedHeroCards.Count);

			foreach(CardScript card in selectedHeroCards) {
				heroes.Add(card.hero);
				card.UnSelected();
			}
			selectedHeroCards.Clear();

			GetRewardFromHeroes(heroes);

			for(int i = 0; i < heroes.Count; i++)
			{
				PlayerScript.Instance.RemoveHero(heroes[i]);
			}
		}

		private void GetRewardFromHeroes(List<InfoHero> heroes){
			Reward reward = new Reward();
			Resource currentResource = null;

			foreach(InfoHero hero in heroes)
			{
				for(int i = 0; i < _templateRewards.Count; i++)
				{
					currentResource = _templateRewards[i].CalculateReward(hero);
					if(currentResource != null)
					{
						reward.AddResource(currentResource);
					}
				}
			}   

			PlayerScript.Instance.AddReward(reward);
		}

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
		
		private void OnDissamble(int amount)
		{
			if(observerDissamble != null) 
				observerDissamble(new BigDigit(amount));
		}	 


	}
}