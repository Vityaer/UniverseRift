using Models.Heroes;
using Common;
using System;
using System.Collections.Generic;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;
using UIController.Rewards;
using City.Buildings.Abstractions;
using Models.Common.BigDigits;
using City.Buildings.Altar;
using UniRx;

namespace Altar
{
    public class AltarController : BuildingWithHeroesList<AltarView>
    {
        [SerializeField] private List<AltarReward> _templateRewards = new List<AltarReward>();

        private readonly ReactiveCommand<BigDigit> _onDissamble = new ReactiveCommand<BigDigit>();

        private List<Card> selectedHeroCards = new List<Card>();

        public IObservable<BigDigit> OnDissamble => _onDissamble;

        protected override void OnStart()
        {
            //ListHeroesController.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            //ListHeroesController.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
            //View.MusterOutButton.OnClickAsObservable().Subscribe(_ => FiredHeroes()).AddTo(Disposables);
        }

        protected override void OpenPage()
        {
            //ListHeroes = GameController.Instance.ListHeroes;
            //LoadListHeroes();
            //View.ListHeroesController.EventOpen();
        }

        protected override void ClosePage()
        {
            //for (int i = 0; i < selectedHeroCards.Count; i++) selectedHeroCards[i].Unselect();
            //selectedHeroCards.Clear();
            //View.ListHeroesController.EventClose();
        }

        public void FiredHeroes()
        {
            List<HeroModel> heroes = new List<HeroModel>();

            _onDissamble.Execute(new BigDigit(selectedHeroCards.Count));

            foreach (Card card in selectedHeroCards)
            {
                //heroes.Add(card.Hero);
                card.Unselect();
            }
            selectedHeroCards.Clear();

            GetRewardFromHeroes(heroes);

            for (int i = 0; i < heroes.Count; i++)
            {
                //GameController.Instance.RemoveHero(heroes[i]);
            }
        }

        private void GetRewardFromHeroes(List<HeroModel> heroes)
        {
            var reward = new RewardModel();

            foreach (HeroModel hero in heroes)
            {
                for (int i = 0; i < _templateRewards.Count; i++)
                {
                    var currentResource = _templateRewards[i].CalculateReward(hero);
                    if (currentResource != null)
                    {
                        //reward.AddResource(currentResource);
                    }
                }
            }

            //GameController.Instance.AddReward(reward);
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
    }
}