using System.Collections.Generic;
using City.SliderCity;
using City.TrainCamp.HeroPanels;
using Common.Heroes;
using Hero;
using UiExtensions.MainPages;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace MainPages.Army
{
    public class PageArmyController : UiMainPageController<PageArmyView>, IInitializable
    {
        [Inject] private readonly IObjectResolver m_resolver;
        [Inject] private readonly HeroPanelController m_heroPanel;
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly IUiMessagesPublisherService m_messagesPublisher;

        private readonly CompositeDisposable m_disposables = new();
        
        private int m_numSelectHero;
        private List<GameHero> m_listHeroes = new();
        private GameHero m_hero;
        private bool m_created;

        public new void Initialize()
        {
            base.Initialize();
            m_heroPanel.OnSwipe.Subscribe(OnSwipe).AddTo(m_disposables);
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(m_disposables);
            m_resolver.Inject(View.CardsContainer);
        }

        protected override void OnLoadGame()
        {
            m_listHeroes = m_heroesStorageController.ListHeroes;
        }

        public override void OnShow()
        {
            View.CardsContainer.ShowCards(m_listHeroes);
            base.OnShow();
        }

        private void OnSwipe(SwipeType typeSwipe)
        {
            switch (typeSwipe)
            {
                case SwipeType.Left:
                    SelectHero(m_numSelectHero - 1);
                    break;
                case SwipeType.Right:
                    SelectHero(m_numSelectHero + 1);
                    break;
            }
        }

        private void SelectHero(int num)
        {
            num = Mathf.Clamp(num, 0, m_listHeroes.Count - 1);

            m_numSelectHero = num;
            m_hero = m_listHeroes[m_numSelectHero];
            m_heroPanel.ShowHero(m_hero);

            ChangeArrows();
        }

        private void ChangeArrows()
        {
            m_heroPanel.ShowLeftArrow(m_numSelectHero != 0);
            m_heroPanel.ShowRightArrow(m_numSelectHero != m_listHeroes.Count - 1);
        }

        private void SelectHero(GameHero hero)
        {
            m_numSelectHero = m_listHeroes.IndexOf(hero);
            SelectHero(m_numSelectHero);
            OpenHeroPanel();
        }

        private void OpenHeroPanel()
        {
            m_messagesPublisher.OpenWindowPublisher.OpenWindow<HeroPanelController>(openType: OpenType.Exclusive);
        }

        public GameHero ReturnSelectHero()
        {
            return m_hero;
        }
    }
}