using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using IdleGame.AdvancedObservers;
using Misc.Json;
using Models;
using Models.City.Hires;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer;

namespace City.Buildings.Tavern
{
    public class TavernController : BaseBuilding<TavernView>
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly HeroesHireResultPanelController _heroesHireResultPanelController;

        private HireContainerModel _simpleHireContainer;
        private HireContainerModel _specialHireContainer;
        private HireContainerModel _friendHireContainer;

        private ReactiveCommand<BigDigit> _observerSimpleHire = new();
        private ReactiveCommand<BigDigit> _observerSpecialHire = new();
        private ReactiveCommand<BigDigit> _observerFriendHire = new();
        private ObserverActionWithHero _observersHireRace = new();

        public IObservable<BigDigit> ObserverSimpleHire => _observerSimpleHire;
        public IObservable<BigDigit> ObserverSpecialHire => _observerSpecialHire;
        public IObservable<BigDigit> ObserverFriendHire => _observerFriendHire;

        protected override void OnStart()
        {
            _simpleHireContainer = _commonDictionaries.HireContainerModels["SimpleHireContainer"];
            _specialHireContainer = _commonDictionaries.HireContainerModels["SpecialHireContainer"];
            _friendHireContainer = _commonDictionaries.HireContainerModels["FriendHireContainer"];

            Resolver.Inject(View.ObserverSimpleHire);
            Resolver.Inject(View.ObserverSpecialHire);

            Resolver.Inject(View.ResourceObjectCostOneHire);
            Resolver.Inject(View.ResourceObjectCostManyHire);

            SelectHire<SpecialHire>(new GameResource(_specialHireContainer.Cost), _observerSpecialHire);

            View.SimpleHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<SimpleHire>(new GameResource(_simpleHireContainer.Cost), _observerSimpleHire))
                .AddTo(Disposables);

            View.SpecialHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<SpecialHire>(new GameResource(_specialHireContainer.Cost), _observerSpecialHire))
                .AddTo(Disposables);

            View.FriendHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<FriendHire>(new GameResource(_friendHireContainer.Cost), _observerFriendHire))
                .AddTo(Disposables);
        }

        public void SelectHire<T>(GameResource costOneHire, ReactiveCommand<BigDigit> onHireHeroes)
            where T : AbstractHireMessage, new()
        {
            View.CostOneHireController
                .ChangeCost(costOneHire, () => HireHero<T>(1, onHireHeroes, costOneHire).Forget());

            View.CostManyHireController
                .ChangeCost(costOneHire * 10, () => HireHero<T>(10, onHireHeroes, costOneHire * 10).Forget());
        }

        private async UniTaskVoid HireHero<T>(int count, ReactiveCommand<BigDigit> onHireHeroes, GameResource cost)
            where T : AbstractHireMessage, new()
        {
            var message = new T { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);

            if (string.IsNullOrEmpty(result))
            {
                return;
			}

			var newHeroDatas = _jsonConverter.Deserialize<List<HeroData>>(result);

            if (newHeroDatas == null)
            {
                return;
            }

            var heroes = new List<GameHero>(newHeroDatas.Count);
            for (int i = 0; i < newHeroDatas.Count; i++)
            {
                var model = _commonDictionaries.Heroes[newHeroDatas[i].HeroId];
                var hero = new GameHero(model, newHeroDatas[i]);
                AddNewHero(hero);
                heroes.Add(hero);
            }
            _resourceStorageController.SubtractResource(cost);

            onHireHeroes.Execute(new BigDigit(count));

            _heroesHireResultPanelController.ShowHeroes(heroes);
        }

        private void AddNewHero(GameHero hero)
        {
            _heroesStorageController.AddHero(hero);
        }
    }
}