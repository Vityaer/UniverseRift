using Assets.Scripts.ClientServices;
using City.Buildings.Abstractions;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models;
using Models.Common;
using Models.Heroes;
using Network.DataServer;
using Network.DataServer.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIController.Cards;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Sanctuary
{
    public class SanctuaryController : BuildingWithHeroesList<SanctuaryView>, IInitializable, IDisposable
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IJsonConverter _jsonConverter;

        public HeroModel newHeroModel;
        private Card _selectedCard;
        private IDisposable _disposable;
        private List<GameHero> _listHeroes;

        public void Initialize()
        {
            View.ReplacementButton.interactable = false;
            View.ReplacementButton.OnClickAsObservable().Subscribe(_ => ReplacementHero().Forget()).AddTo(Disposables);
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
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

        private async UniTaskVoid ReplacementHero()
        {
            //GameResource resCost = null;
            if (_selectedCard != null)
            {
                //foreach (var cost in View.Costs)
                //{
                //    if (_selectedCard.Hero.HeroData.Rating >= cost.Key)
                //        resCost = cost.Value;
                //}

                //if (_resourceStorageController.CheckResource(resCost))
                //{
                    var message = new ReplaceHeroMessage { PlayerId = CommonGameData.PlayerInfoData.Id, HeroId = _selectedCard.Hero.HeroData.Id };
                    var result = await DataServer.PostData(message);

                    View.CardsContainer.RemoveCard(_selectedCard);
                    _heroesStorageController.RemoveHero(_selectedCard.Hero);

                    var heroData = _jsonConverter.FromJson<HeroData>(result);

                    var model = _commonDictionaries.Heroes[heroData.HeroId];
                    var hero = new GameHero(model, heroData);
                    _heroesStorageController.AddHero(hero);

                    _selectedCard = null;
                    View.ReplacementButton.interactable = false;
                    View.CardsContainer.ShowCards(_listHeroes);
                //}
            }
        }

        public void SelectHero(GameHero hero)
        {
            UnityEngine.Debug.Log("select hero");
            if (_selectedCard != null)
            {
                _selectedCard.Unselect();
            }

            _selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            _selectedCard.Select();

            View.ReplacementButton.interactable = true;
        }

        public void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Unselect();
            _selectedCard = null;
            View.ReplacementButton.interactable = false;
        }


        public void Disposable()
        {
            _disposable?.Dispose();
            base.Dispose();
        }
    }
}