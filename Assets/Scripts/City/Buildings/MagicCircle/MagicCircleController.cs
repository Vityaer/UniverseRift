using City.Buildings.Abstractions;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.Hires;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleController : BaseBuilding<MagicCircleView>
    {
        private const string DEFAULT_RACE_NAME = "People";

        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        [SerializeField] private string _selectedRace;
        private List<HeroModel> _listHeroes = new List<HeroModel>();
        private GameResource RaceHireCost = new GameResource(ResourceType.RaceHireCard, 1, 0);

        protected override void OnStart()
        {
            foreach (var button in View.RaceSelectButtons)
            {
                button.Value.OnClickAsObservable().Subscribe(_ => ChangeHireRace(button.Key)).AddTo(Disposables);
            }

            //_resolver.Inject(View.ObserverRaceHireCard);

            //_resolver.Inject(View.ResourceObjectCostOneHire);
            //_resolver.Inject(View.ResourceObjectCostManyHire);

            View.OneHire.ChangeCost(RaceHireCost, () => HireHero(1).Forget());
            View.ManyHire.ChangeCost(RaceHireCost * 10, () => HireHero(10).Forget());

            ChangeHireRace(DEFAULT_RACE_NAME);
        }

        public void ChangeHireRace(string stringRace)
        {
            _selectedRace = stringRace;
        }

        private async UniTaskVoid HireHero(int count = 1)
        {
            var message = new MagicCircleHire { PlayerId = CommonGameData.PlayerInfoData.Id, Count = count };
            var result = await DataServer.PostData(message);
            var newHeroes = _jsonConverter.FromJson<List<HeroData>>(result);

            for (int i = 0; i < newHeroes.Count; i++)
            {
                Debug.Log($"{newHeroes[i].Id}");
                var model = _commonDictionaries.Heroes[newHeroes[i].HeroId];
                var hero = new GameHero(model, newHeroes[i]);
                hero.Model.General.Name = $"{hero.Model.General.HeroId} #{UnityEngine.Random.Range(0, 1000)}";
                AddNewHero(hero);
            }
            _resourceStorageController.SubtractResource(RaceHireCost * count);
        }

        private void AddNewHero(GameHero hero)
        {
            _heroesStorageController.AddHero(hero);
        }
    }
}