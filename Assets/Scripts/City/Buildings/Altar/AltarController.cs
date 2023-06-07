using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Altar
{
    public class AltarController : BuildingWithHeroesList
    {

        [SerializeField] private List<AltarReward> _templateRewards = new List<AltarReward>();
        [SerializeField] protected Button _musterOutButton;

        private List<Card> selectedHeroCards = new List<Card>();

        protected override void OnStart()
        {
            listHeroesController.RegisterOnSelect(SelectHero);
            listHeroesController.RegisterOnUnSelect(UnselectHero);
            _musterOutButton.onClick.AddListener(FiredHeroes);
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
            Reward reward = new Reward();
            Resource currentResource = null;

            foreach (HeroModel hero in heroes)
            {
                for (int i = 0; i < _templateRewards.Count; i++)
                {
                    currentResource = _templateRewards[i].CalculateReward(hero);
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