using City.Buildings.Abstractions;
using Common.Heroes;
using Common.Resourses;
using Models;
using Models.Heroes;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleController : BaseBuilding<MagicCircleView>
    {
        private const string DEFAULT_RACE_NAME = "People";

        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly IObjectResolver _resolver;

        [SerializeField] private string _selectedRace;
        private List<HeroModel> _listHeroes = new List<HeroModel>();
        private GameResource RaceHireCost = new GameResource(ResourceType.RaceHireCard, 1, 0);

        protected override void OnStart()
        {
            foreach (var button in View.RaceSelectButtons)
            {
                button.Value.OnClickAsObservable().Subscribe(_ => ChangeHireRace(button.Key)).AddTo(Disposables);
            }

            _resolver.Inject(View.ObserverRaceHireCard);

            _resolver.Inject(View.ResourceObjectCostOneHire);
            _resolver.Inject(View.ResourceObjectCostManyHire);

            View.OneHire.ChangeCost(RaceHireCost, () => HireHero(1));
            View.ManyHire.ChangeCost(RaceHireCost * 10, () => HireHero(10));

            ChangeHireRace(DEFAULT_RACE_NAME);
        }

        public void ChangeHireRace(string stringRace)
        {
            _selectedRace = stringRace;
        }

        private void HireHero(int count = 1)
        {
            float rand = 0f;
            HeroModel hero = null;
            List<HeroModel> workList = new List<HeroModel>();
            for (int i = 0; i < count; i++)
            {
                rand = UnityEngine.Random.Range(0f, 100f);
                if (rand < 96f)
                {
                    workList = workList.FindAll(x => x.General.Rating == 4 && x.General.Race == _selectedRace);
                }
                else
                {
                    workList = workList.FindAll(x => x.General.Rating == 5 && x.General.Race == _selectedRace);
                }
                //hero = (HeroModel)workList[UnityEngine.Random.Range(0, workList.Count)].Clone();

                if (hero != null)
                {
                    hero.General.Name = hero.General.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
                    AddNewHero(hero);
                }
            }
        }

        private void AddNewHero(HeroModel newHeroModel)
        {
            var newHero = new HeroData()
            {
                HeroId = newHeroModel.Id,
                Level = 1,
                Rating = 1,
                CurrentBreakthrough = 0
            };

            //_heroesStorageController.AddHero(newHero);
        }
    }
}