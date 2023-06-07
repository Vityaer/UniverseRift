using Assets.Scripts.City.Buildings.Tavern;
using Assets.Scripts.Models.Heroes;
using City.Buildings.General;
using Common.Resourses;
using System.Collections.Generic;
using UIController.Buttons;
using UnityEngine;

namespace City.Buildings.MagicCircle
{
    public class MagicCircle : Building
    {
        [SerializeField] private ButtonWithObserverResource _oneHire;
        [SerializeField] private ButtonWithObserverResource _tenHire;
        [SerializeField] private string _selectedRace;
        private List<HeroModel> _listHeroes = new List<HeroModel>();
        private Resource RaceHireCost = new Resource(TypeResource.RaceHireCard, 1, 0);

        protected override void OpenPage()
        {
            if (_listHeroes.Count == 0)
            {
                _listHeroes = Tavern.Instance.GetListHeroes;
            }
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
                    workList = workList.FindAll(x => x.General.RatingHero == 4 && x.General.Race == _selectedRace);
                }
                else
                {
                    workList = workList.FindAll(x => x.General.RatingHero == 5 && x.General.Race == _selectedRace);
                }
                hero = (HeroModel)workList[UnityEngine.Random.Range(0, workList.Count)].Clone();

                if (hero != null)
                {
                    hero.General.Name = hero.General.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
                    AddNewHero(hero);
                }
            }
        }

        private void AddNewHero(HeroModel hero)
        {
            Tavern.Instance.AddNewHero(hero);
        }


        protected override void OnStart()
        {
            _oneHire.ChangeCost(HireHero);
            _tenHire.ChangeCost(HireHero);
        }
    }
}