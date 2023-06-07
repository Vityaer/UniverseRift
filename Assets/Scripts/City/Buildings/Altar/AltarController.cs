using Assets.Scripts.City.Buildings.General;
using Assets.Scripts.GeneralObject;
using Assets.Scripts.Models.Heroes;
using Common;
using System;
using System.Collections.Generic;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace Altar
{
    public class AltarController : BuildingWithHeroesList
    {
        [SerializeField] private List<AltarReward> _templateRewards = new List<AltarReward>();
        [SerializeField] protected Button MusterOutButton;

        private List<Card> selectedHeroCards = new List<Card>();

        protected override void OnStart()
        {
            listHeroesController.RegisterOnSelect(SelectHero);
            listHeroesController.RegisterOnUnSelect(UnselectHero);
            MusterOutButton.onClick.AddListener(FiredHeroes);
        }

        protected override void OpenPage()
        {
            listHeroes = GameController.Instance.GetListHeroes;
            LoadListHeroes();
            listHeroesController.EventOpen();
        }

        protected override void ClosePage()
        {
            for (int i = 0; i < selectedHeroCards.Count; i++) selectedHeroCards[i].Unselect();
            selectedHeroCards.Clear();
            listHeroesController.EventClose();
        }

        public void FiredHeroes()
        {
            List<HeroModel> heroes = new List<HeroModel>();

            OnDissamble(selectedHeroCards.Count);

            foreach (Card card in selectedHeroCards)
            {
                heroes.Add(card.hero);
                card.Unselect();
            }
            selectedHeroCards.Clear();

            GetRewardFromHeroes(heroes);

            for (int i = 0; i < heroes.Count; i++)
            {
                GameController.Instance.RemoveHero(heroes[i]);
            }
        }

        private void GetRewardFromHeroes(List<HeroModel> heroes)
        {
            var reward = new Reward();

            foreach (HeroModel hero in heroes)
            {
                for (int i = 0; i < _templateRewards.Count; i++)
                {
                    var currentResource = _templateRewards[i].CalculateReward(hero);
                    if (currentResource != null)
                    {
                        reward.AddResource(currentResource);
                    }
                }
            }

            GameController.Instance.AddReward(reward);
        }

        public override void SelectHero(Card cardHero)
        {
            selectedHeroCards.Add(cardHero);
            cardHero.Select();
        }

        public override void UnselectHero(Card cardHero)
        {
            selectedHeroCards.Remove(cardHero);
            cardHero.Unselect();
        }

        //Observers
        private Action<BigDigit> observerDissamble;
        public void RegisterOnDissamble(Action<BigDigit> d) { observerDissamble += d; }

        private void OnDissamble(int amount)
        {
            if (observerDissamble != null)
                observerDissamble(new BigDigit(amount));
        }


    }
}