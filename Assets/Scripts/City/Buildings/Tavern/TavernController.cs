using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Hero;
using Misc.Json;
using Models;
using Models.City.Hires;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages;
using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using UniRx;
using VContainer;

namespace City.Buildings.Tavern
{
    public class TavernController : BaseBuilding<TavernView>
    {
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly HeroesHireResultPanelController m_heroesHireResultPanelController;

        private HireContainerModel m_simpleHireContainer;
        private HireContainerModel m_specialHireContainer;
        private HireContainerModel m_friendHireContainer;

        private readonly ReactiveCommand<BigDigit> m_observerSimpleHire = new();
        private readonly ReactiveCommand<BigDigit> m_observerSpecialHire = new();
        private readonly ReactiveCommand<BigDigit> m_observerFriendHire = new();

        public IObservable<BigDigit> ObserverSimpleHire => m_observerSimpleHire;
        public IObservable<BigDigit> ObserverSpecialHire => m_observerSpecialHire;
        public IObservable<BigDigit> ObserverFriendHire => m_observerFriendHire;

        protected override void OnStart()
        {
            m_simpleHireContainer = m_commonDictionaries.HireContainerModels["SimpleHireContainer"];
            m_specialHireContainer = m_commonDictionaries.HireContainerModels["SpecialHireContainer"];
            m_friendHireContainer = m_commonDictionaries.HireContainerModels["FriendHireContainer"];

            Resolver.Inject(View.ObserverSimpleHire);
            Resolver.Inject(View.ObserverSpecialHire);

            Resolver.Inject(View.ResourceObjectCostOneHire);
            Resolver.Inject(View.ResourceObjectCostManyHire);

            SelectHire<SpecialHire>(new GameResource(m_specialHireContainer.Cost), m_observerSpecialHire);

            View.SimpleHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<SimpleHire>(new GameResource(m_simpleHireContainer.Cost), m_observerSimpleHire))
                .AddTo(Disposables);

            View.SpecialHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<SpecialHire>(new GameResource(m_specialHireContainer.Cost), m_observerSpecialHire))
                .AddTo(Disposables);

            View.FriendHireButton.OnClickAsObservable()
                .Subscribe(_ => SelectHire<FriendHire>(new GameResource(m_friendHireContainer.Cost), m_observerFriendHire))
                .AddTo(Disposables);
        }

        private void SelectHire<T>(GameResource costOneHire, ReactiveCommand<BigDigit> onHireHeroes)
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
            string result = await DataServer.PostData(message);

            if (string.IsNullOrEmpty(result))
            {
                return;
			}

			var newHeroDatas = m_jsonConverter.Deserialize<List<HeroData>>(result);

            if (newHeroDatas == null)
            {
                return;
            }

            var heroes = new List<GameHero>(newHeroDatas.Count);
            foreach (var t in newHeroDatas)
            {
                var model = m_commonDictionaries.Heroes[t.HeroId];
                var hero = new GameHero(model, t);
                AddNewHero(hero);
                heroes.Add(hero);
            }
            m_resourceStorageController.SubtractResource(cost);

            onHireHeroes.Execute(new BigDigit(count));

            m_heroesHireResultPanelController.ShowHeroes(heroes);
        }

        private void AddNewHero(GameHero hero)
        {
            m_heroesStorageController.AddHero(hero);
        }
    }
}