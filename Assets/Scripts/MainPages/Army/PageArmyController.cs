using City.MainPages.Army;
using City.SliderCity;
using City.TrainCamp;
using Common.Heroes;
using Hero;
using System.Collections.Generic;
using UIController.Cards;
using UIController.GameSystems;
using UiExtensions.MainPages;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace UIController
{
    public class PageArmyController : UiMainPageController<PageArmyView>, IInitializable
    {
        [Inject] private readonly HeroPanelController _heroPanel;
        //[Inject] private readonly MainSwipeController _mainSwipeController;
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;

        private int numSelectHero = 0;
        private List<GameHero> _listHeroes = new();
        private GameHero hero;
        private CompositeDisposable _disposables = new CompositeDisposable();
        private bool _created;

        public new void Initialize()
        {
            base.Initialize();
            _heroPanel.OnSwipe.Subscribe(OnSwipe).AddTo(_disposables);
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(_disposables);
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

        public override void OnHide()
        {
            base.OnHide();
        }

        private void OnSwipe(SwipeType typeSwipe)
        {
            switch (typeSwipe)
            {
                case SwipeType.Left:
                    SelectHero(numSelectHero - 1);
                    break;
                case SwipeType.Right:
                    SelectHero(numSelectHero + 1);
                    break;
            }
        }

        private void SelectHero(int num)
        {
            num = Mathf.Clamp(num, 0, _listHeroes.Count - 1);

            numSelectHero = num;
            hero = _listHeroes[numSelectHero];
            _heroPanel.ShowHero(hero);
        }

        private void SelectHero(GameHero hero)
        {
            numSelectHero = _listHeroes.IndexOf(hero);
            SelectHero(numSelectHero);
            OpenHeroPanel();
        }

        private void OpenHeroPanel()
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<HeroPanelController>(openType: OpenType.Exclusive);
        }

        public GameHero ReturnSelectHero()
        {
            return hero;
        }
    }
}