using City.Buildings.General;
using City.Buildings.Tavern;
using Common;
using Common.Resourses;
using Models.Heroes;
using System.Collections.Generic;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Sanctuary
{
    public class Sanctuary : BuildingWithHeroesList
    {
        public Resource costReplacementFourRating, costReplacementFiveRating;
        private Card selectedHero;
        public Button btnSave, btnReplacement;
        public HeroModel newHero;


        private List<HeroModel> heroes = new List<HeroModel>();
        public void ReplacementHero()
        {
            Resource resCost = null;
            if (selectedHero != null)
            {
                if (selectedHero.Hero.General.RatingHero == 4)
                {
                    resCost = costReplacementFourRating;
                    heroes = TavernController.Instance.GetListHeroes.FindAll(x => x.General.RatingHero == 4);
                }
                else
                {
                    resCost = costReplacementFiveRating;
                    heroes = TavernController.Instance.GetListHeroes.FindAll(x => x.General.RatingHero == 5);
                }

                if (GameController.Instance.CheckResource(resCost))
                {
                    heroes = heroes.FindAll(x => x.General.RatingHero == selectedHero.Hero.General.RatingHero && x.General.Race == selectedHero.Hero.General.Race && x.General.ViewId != selectedHero.Hero.General.ViewId);
                    if (heroes.Count > 0)
                    {
                        newHero = (HeroModel)heroes[Random.Range(0, heroes.Count)].Clone();
                        btnSave.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void SaveReplacement()
        {
            if (newHero != null)
            {
                listHeroesController.RemoveCards(new List<Card> { selectedHero });
                GameController.Instance.RemoveHero(selectedHero.Hero);
                GameController.Instance.AddHero(newHero);
                btnSave.gameObject.SetActive(false);
            }
        }
        public override void SelectHero(Card newCardHero)
        {
            if (selectedHero != null) selectedHero.Unselect();
            selectedHero = newCardHero;
            selectedHero.Select();
            btnReplacement.interactable = true;
        }

        protected override void OpenPage()
        {
            listHeroes = GameController.Instance.GetListHeroes;
            LoadListHeroes();
            listHeroesController.EventOpen();
            listHeroesController.RegisterOnSelect(SelectHero);
        }
        protected override void ClosePage()
        {
            btnSave.gameObject.SetActive(false);
            selectedHero = null;
            listHeroesController.UnRegisterOnSelect(SelectHero);
            listHeroesController.EventClose();
        }

    }
}