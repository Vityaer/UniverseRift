using City.Buildings.Abstractions;
using Common;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using IdleGame.AdvancedObservers;
using Misc.Json;
using Models;
using Models.Common.BigDigits;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Tavern
{
    public class TavernController : BaseBuilding<TavernView>, IStartable
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IObjectResolver _resolver;

        private GameResource _simpleHireCost = new GameResource(ResourceType.SimpleHireCard, 1, 0);
        private GameResource _specialHireCost = new GameResource(ResourceType.SpecialHireCard, 1, 0);
        private GameResource _friendHireCost = new GameResource(ResourceType.FriendHeart, 10, 0);

        private ReactiveCommand<BigDigit> _observerSimpleHire = new ReactiveCommand<BigDigit>();
        private ReactiveCommand<BigDigit> _observerSpecialHire = new ReactiveCommand<BigDigit>();
        private ReactiveCommand<BigDigit> _observerFriendHire = new ReactiveCommand<BigDigit>();

        private ObserverActionWithHero observersHireRace = new ObserverActionWithHero();

        protected override void OnStart()
        {
            _resolver.Inject(View.ObserverSimpleHire);
            _resolver.Inject(View.ObserverSpecialHire);

            _resolver.Inject(View.ResourceObjectCostOneHire);
            _resolver.Inject(View.ResourceObjectCostManyHire);

            SelectHire<SpecialHire>(_simpleHireCost, _observerSpecialHire);

            View.SimpleHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<SimpleHire>(_simpleHireCost, _observerSimpleHire));
            View.SpecialHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<SpecialHire>(_specialHireCost, _observerSpecialHire));
            View.FriendHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<FriendHire>(_friendHireCost, _observerFriendHire));
        }

        public void SelectHire<T>(GameResource costOneHire, ReactiveCommand<BigDigit> onHireHeroes) where T : AbstractHireMessage, new()
        {
            View.CostOneHireController.ChangeCost(costOneHire, () => HireHero<T>(1, onHireHeroes).Forget());
            View.CostManyHireController.ChangeCost(costOneHire * 10, () => HireHero<T>(10, onHireHeroes).Forget());
        }

        private async UniTaskVoid HireHero<T>(int count, ReactiveCommand<BigDigit> onHireHeroes) where T : AbstractHireMessage, new()
        {
            Debug.Log("start hire hero");
            var message = new T { PlayerId = CommonGameData.Player.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);
            Debug.Log(result);
            var newHeroes = _jsonConverter.FromJson<List<HeroData>>(result);

            for (int i = 0; i < newHeroes.Count; i++)
            {
                Debug.Log($"{newHeroes[i]}");
                var model = _commonDictionaries.Heroes[newHeroes[i].HeroId];
                var hero = new GameHero(model, newHeroes[i]);
                OnHireHeroes(hero);
                hero.Model.General.Name = $"{hero.Model.General.HeroId} #{UnityEngine.Random.Range(0, 1000)}";
                AddNewHero(hero);
            }

            onHireHeroes.Execute(new BigDigit(count));
        }

        private void AddNewHero(GameHero hero)
        {
            _heroesStorageController.AddHero(hero);
        }

        public void RegisterOnHireHeroes(Action<BigDigit> d, string ID = "")
        {
            observersHireRace.Add(d, ID, 1);
        }

        private void OnHireHeroes(GameHero hero)
        {
            //and Rarity
            observersHireRace.OnAction(string.Empty, 1);
            observersHireRace.OnAction(hero.Model.General.ViewId, 1);
        }


    }
}