﻿using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
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
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly HeroesHireResultPanelController _heroesHireResultPanelController;
        
        private GameResource _simpleHireCost = new GameResource(ResourceType.SimpleHireCard, 1, 0);
        private GameResource _specialHireCost = new GameResource(ResourceType.SpecialHireCard, 1, 0);
        private GameResource _friendHireCost = new GameResource(ResourceType.FriendHeart, 10, 0);

        private ReactiveCommand<BigDigit> _observerSimpleHire = new ReactiveCommand<BigDigit>();
        private ReactiveCommand<BigDigit> _observerSpecialHire = new ReactiveCommand<BigDigit>();
        private ReactiveCommand<BigDigit> _observerFriendHire = new ReactiveCommand<BigDigit>();
        private ObserverActionWithHero _observersHireRace = new ObserverActionWithHero();

        public IObservable<BigDigit> ObserverSimpleHire => _observerSimpleHire;
        public IObservable<BigDigit> ObserverSpecialHire => _observerSpecialHire;
        public IObservable<BigDigit> ObserverFriendHire => _observerFriendHire;

        protected override void OnStart()
        {
            Resolver.Inject(View.ObserverSimpleHire);
            Resolver.Inject(View.ObserverSpecialHire);

            Resolver.Inject(View.ResourceObjectCostOneHire);
            Resolver.Inject(View.ResourceObjectCostManyHire);

            SelectHire<SpecialHire>(_simpleHireCost, _observerSimpleHire);

            View.SimpleHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<SimpleHire>(_simpleHireCost, _observerSimpleHire));
            View.SpecialHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<SpecialHire>(_specialHireCost, _observerSpecialHire));
            View.FriendHireButton.OnClickAsObservable().Subscribe(_ => SelectHire<FriendHire>(_friendHireCost, _observerFriendHire));
        }

        public void SelectHire<T>(GameResource costOneHire, ReactiveCommand<BigDigit> onHireHeroes) where T : AbstractHireMessage, new()
        {
            View.CostOneHireController.ChangeCost(costOneHire, () => HireHero<T>(1, onHireHeroes, costOneHire).Forget());
            View.CostManyHireController.ChangeCost(costOneHire * 10, () => HireHero<T>(10, onHireHeroes, costOneHire * 10).Forget());
        }

        private async UniTaskVoid HireHero<T>(int count, ReactiveCommand<BigDigit> onHireHeroes, GameResource cost)
            where T : AbstractHireMessage, new()
        {
            var message = new T { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);
            var newHeroDatas = _jsonConverter.Deserialize<List<HeroData>>(result);

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

        public void RegisterOnHireHeroes(Action<BigDigit> d, string ID = "")
        {
            _observersHireRace.Add(d, ID, 1);
        }
    }
}