using City.Buildings.Abstractions;
using City.Buildings.Tavern;
using Common.Heroes;
using Common.Resourses;
using Models;
using Models.Common;
using Models.Heroes;
using System.Collections.Generic;
using UIController.Cards;
using VContainer;
using UniRx;
using System;
using Hero;
using Db.CommonDictionaries;
using System.Linq;
using static UnityEngine.Rendering.DebugUI;
using Assets.Scripts.ClientServices;

namespace City.Buildings.Sanctuary
{
    public class SanctuaryController : BuildingWithHeroesList<SanctuaryView>, IDisposable
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public HeroModel newHeroModel;
        private Card _selectedCard;
        private IDisposable _disposable;

        public void ReplacementHero()
        {
            GameResource resCost = null;
            if (_selectedCard != null)
            {
                foreach (var cost in View.Costs)
                {
                    if (_selectedCard.Hero.HeroData.Rating >= cost.Key)
                        resCost = cost.Value;
                }

                if (_resourceStorageController.CheckResource(resCost))
                {
                    var heroes = _commonDictionaries.Heroes.Where(x => x.Value.General.Rating == _selectedCard.Hero.HeroData.Rating && x.Value.General.Race == _selectedCard.Hero.Model.General.Race && x.Value.General.ViewId != _selectedCard.Hero.Model.General.ViewId).ToList();
                    if (heroes.Count > 0)
                    {
                        var randomIndex = UnityEngine.Random.Range(0, heroes.Count);
                        newHeroModel = _commonDictionaries.Heroes.ElementAt(randomIndex).Value;
                        View.SaveButton.interactable = true;
                    }
                }
            }
        }

        public void SaveReplacement()
        {
            if (newHeroModel != null)
            {
                var newHero = new HeroData()
                {
                    HeroId = newHeroModel.Id,
                    Level = 1,
                    Rating = 1,
                    CurrentBreakthrough = 0
                };

                //ListHeroesController.RemoveCards(new List<Card> { _selectedCard });
                _heroesStorageController.RemoveHero(_selectedCard.Hero);
                //_heroesStorageController.AddHero(newHero);
                View.SaveButton.interactable = false;
            }
        }

        public override void SelectHero(Card newCardHero)
        {
            if (_selectedCard != null) _selectedCard.Unselect();
            _selectedCard = newCardHero;
            _selectedCard.Select();
            View.ReplacementButton.interactable = true;
        }

        protected override void OpenPage()
        {
            //LoadListHeroes();
            //ListHeroesController.EventOpen();
            //_disposable = ListHeroesController.OnSelect.Subscribe(SelectHero);
        }

        protected override void ClosePage()
        {
            //View.SaveButton.gameObject.SetActive(false);
            _selectedCard = null;
            _disposable?.Dispose();
            //ListHeroesController.EventClose();
        }

        public void Disposable()
        {
            _disposable?.Dispose();
            base.Dispose();
        }
    }
}