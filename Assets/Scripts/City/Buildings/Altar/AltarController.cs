using System;
using System.Collections.Generic;
using UIController.Cards;
using UnityEngine;
using UIController.Rewards;
using City.Buildings.Abstractions;
using Models.Common.BigDigits;
using City.Buildings.Altar;
using UniRx;
using VContainer.Unity;
using Common.Heroes;
using VContainer;
using Hero;
using Cysharp.Threading.Tasks;
using Network.DataServer.Models;
using Network.DataServer;
using ClientServices;
using Common.Rewards;
using Misc.Json;
using Network.DataServer.Messages;
using Db.CommonDictionaries;

namespace Altar
{
    public class AltarController : BuildingWithHeroesList<AltarView>, IInitializable
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        [SerializeField] private List<AltarReward> _templateRewards = new List<AltarReward>();

        private readonly ReactiveCommand<BigDigit> _onDissamble = new ReactiveCommand<BigDigit>();

        private List<GameHero> _listHeroes;
        private List<Card> _selectedHeroCards = new();

        public IObservable<BigDigit> OnDissamble => _onDissamble;

        public void Initialize()
        {
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
            View.MusterOutButton.OnClickAsObservable().Subscribe(_ => FiredHeroes().Forget()).AddTo(Disposables);
            Resolver.Inject(View.CardsContainer);
        }

        protected override void OnLoadGame()
        {
            _listHeroes = _heroesStorageController.ListHeroes;
        }

        public override void OnShow()
        {
            View.CardsContainer.ShowCards(_listHeroes);
            base.OnShow();
        }

        public async UniTaskVoid FiredHeroes()
        {
            var fireContainer = new FireContainer();

            foreach (var card in _selectedHeroCards)
            {
                fireContainer.HeroesIds.Add(card.Hero.HeroData.Id);
            }

            var message = new FireHeroesMessage { PlayerId = CommonGameData.PlayerInfoData.Id, Container = fireContainer };
            var result = await DataServer.PostData(message);

            var rewardModel = _jsonConverter.FromJson<RewardModel>(result);
            var reward = new GameReward(rewardModel, _commonDictionaries);
            _clientRewardService.ShowReward(reward);

            var heroes = new List<GameHero>();

            _onDissamble.Execute(new BigDigit(_selectedHeroCards.Count));

            foreach (var card in _selectedHeroCards)
            {
                heroes.Add(card.Hero);
                card.Unselect();
            }
            View.CardsContainer.RemoveCards(_selectedHeroCards);
            _selectedHeroCards.Clear();

            for (int i = 0; i < heroes.Count; i++)
            {
                _heroesStorageController.RemoveHero(heroes[i]);
            }
        }

        public void SelectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            _selectedHeroCards.Add(selectedCard);
            selectedCard.Select();
        }

        public void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            _selectedHeroCards.Remove(selectedCard);
            selectedCard.Unselect();
        }


    }
}