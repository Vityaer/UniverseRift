using System;
using System.Collections.Generic;
using System.Linq;
using City.Panels.SelectHeroes.Comparers;
using Common.Db.CommonDictionaries;
using Hero;
using Models.Fights.Campaign;
using UIController.Cards;
using UiExtensions.CustomElements;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainerUi.Abstraction;

namespace City.Panels.SelectHeroes
{
    public class HeroCardsContainerController : UiView
    {
        [Inject] private IObjectResolver m_resolver;
        [Inject] private CommonDictionaries m_commonDictionaries;

        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private Card _prefab;
        [SerializeField] private AnimateCustomToggle _customToggle;
        [SerializeField] private Dictionary<string, Button> _buttons = new();
        [SerializeField] private Button _allHeroesButton;

        private Button m_currentButton;
        private DynamicUiList<Card, GameHero> m_cardPool;
        private bool m_loadedListHeroes = false;

        private List<GameHero> m_listHeroes = new();
        private ReactiveCommand<GameHero> m_onSelect = new();
        private ReactiveCommand<GameHero> m_onDiselect = new();
        private CompositeDisposable m_disposables = new();
        private GameHeroRatingComparer m_gameHeroRatingComparer = new();
        private GameHeroLevelComparer m_gameHeroLevelComparer = new();

        public Transform Content => _scroll.content;
        public IObservable<GameHero> OnSelect => m_onSelect;
        public IObservable<GameHero> OnDiselect => m_onDiselect;
        public List<Card> Cards => m_cardPool.Views;

        private void TryCreateCardPool()
        {
            if (m_cardPool != null) return;

            m_cardPool = new(_prefab, _scroll.content, _scroll, m_commonDictionaries, OnCardSelect, OnCardCreate);
        }

        protected override void Start()
        {
            TryCreateCardPool();

            foreach (KeyValuePair<string, Button> buttonContainer in _buttons)
                buttonContainer.Value.OnClickAsObservable()
                    .Subscribe(_ => ShowRace(buttonContainer))
                    .AddTo(m_disposables);

            _allHeroesButton.OnClickAsObservable()
                .Subscribe(_ => ShowAllCards())
                .AddTo(m_disposables);
            
            m_currentButton = _allHeroesButton;
            m_currentButton.interactable = false;

            _customToggle.OnChange.Subscribe(ChangeSortCardType).AddTo(m_disposables);
        }

        private void ChangeSortCardType(bool flag)
        {
            m_listHeroes.Sort(flag ? m_gameHeroRatingComparer : m_gameHeroLevelComparer);
            m_cardPool.ShowDatas(m_listHeroes);
        }

        public void ShowCards(List<GameHero> heroes, List<AbstractHeroRestriction> restrictions = null)
        {
            if (heroes == null)
                return;

            if (restrictions != null)
                m_listHeroes = heroes.FindAll(hero => restrictions
                    .TrueForAll(restriction => restriction.CheckHero(hero)));
            else
                m_listHeroes = heroes;

            m_listHeroes.Sort(_customToggle.IsOn ? m_gameHeroRatingComparer : m_gameHeroLevelComparer);

            if (m_currentButton)
                m_currentButton.interactable = true;

            m_currentButton = _allHeroesButton;
            m_currentButton.interactable = false;

            TryCreateCardPool();
            m_cardPool.ShowDatas(m_listHeroes);
        }

        public void ShowRace(KeyValuePair<string, Button> raceButton)
        {
            if (m_currentButton != null) m_currentButton.interactable = true;

            m_currentButton = raceButton.Value;
            m_currentButton.interactable = false;

            List<GameHero> raceHeroes = m_listHeroes
                .Where(hero => hero.Model.General.Race.Equals(raceButton.Key))
                .ToList();

            TryCreateCardPool();
            m_cardPool.ShowDatas(raceHeroes);
        }

        public void ShowAllCards()
        {
            if (m_currentButton != null) m_currentButton.interactable = true;

            m_currentButton = _allHeroesButton;
            m_currentButton.interactable = false;

            TryCreateCardPool();
            m_cardPool.ShowDatas(m_listHeroes);
        }

        public void RemoveCards(List<Card> cardsForRemove)
        {
            foreach (Card card in cardsForRemove) m_cardPool.RemoveElement(card);
        }

        public void RemoveCard(Card cardForRemove)
        {
            m_cardPool.RemoveElement(cardForRemove);
        }

        private void OnCardClick(Card card)
        {
            if (!card.Selected)
                m_onSelect.Execute(card.Hero);
            else
                m_onDiselect.Execute(card.Hero);
        }

        public void SelectCards(List<GameHero> selectedCard)
        {
            GameHero currentHero = null;
            foreach (GameHero hero in selectedCard)
            {
                currentHero = hero;
                Card currentCard = Cards.Find(x => x.Hero == currentHero);
                if (!currentCard)
                    continue;

                currentCard.Select();
            }
        }

        public void UnselectCards(List<GameHero> unselectedCard)
        {
            GameHero currentHero = null;
            foreach (GameHero hero in unselectedCard)
            {
                currentHero = hero;
                Card currentCard = Cards.Find(x => x.Hero == currentHero);
                if (currentCard) currentCard.Unselect();
            }
        }

        public void UnselectCard(GameHero hero)
        {
            Card card = Cards.Find(x => x.Hero == hero);
            if (card) card.Unselect();
        }

        private void OnCardCreate(Card card)
        {
            m_resolver.Inject(card);
            card.OnClick.Subscribe(OnCardClick).AddTo(m_disposables);
        }

        private void OnCardSelect(Card card)
        {
        }

        private new void OnDestroy()
        {
            m_disposables.Dispose();
        }
    }
}