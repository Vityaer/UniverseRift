using City.Panels.SelectHeroes.Comparers;
using Hero;
using System;
using System.Collections.Generic;
using System.Linq;
using UIController.Cards;
using UiExtensions.CustomElements;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Panels.SelectHeroes
{
    public class HeroCardsContainerController : MonoBehaviour
    {
        [Inject] private IObjectResolver _resolver;

        [SerializeField] private ScrollRect _scroll;
        [SerializeField] private Card _prefab;
        [SerializeField] private AnimateCustomToggle _customToggle;

        private DynamicUiList<Card, GameHero> _cardPool;
        private bool _loadedListHeroes = false;

        private List<GameHero> _listHeroes = new();
        private ReactiveCommand<GameHero> _onSelect = new();
        private ReactiveCommand<GameHero> _onDiselect = new();
        private CompositeDisposable _disposables = new();
        private GameHeroRatingComparer _gameHeroRatingComparer = new();
        private GameHeroLevelComparer _gameHeroLevelComparer = new();

        public bool LoadedListHeroes => _loadedListHeroes;
        public IObservable<GameHero> OnSelect => _onSelect;
        public IObservable<GameHero> OnDiselect => _onDiselect;
        public List<Card> Cards => _cardPool.Views;

        private void Awake()
        {
            _cardPool = new(_prefab, _scroll.content, _scroll, OnCardSelect, OnCardCreate);
        }

        private void Start()
        {
            _customToggle.OnChange.Subscribe(ChangeSortCardType).AddTo(_disposables);
        }

        private void OnCardCreate(Card card)
        {
            _resolver.Inject(card);
            card.OnClick.Subscribe(OnCardClick).AddTo(_disposables);
        }

        private void OnCardSelect(Card card)
        {
        }

        private void ChangeSortCardType(bool flag)
        {
            _listHeroes.Sort(flag ? _gameHeroRatingComparer : _gameHeroLevelComparer);
            _cardPool.ShowDatas(_listHeroes);
        }

        public void ShowCards(List<GameHero> heroes)
        {
            if (heroes == null)
                return;

            _listHeroes = heroes;
            _listHeroes.Sort(_customToggle.IsOn ? _gameHeroRatingComparer : _gameHeroLevelComparer);
            _cardPool.ShowDatas(_listHeroes);
        }

        public void ShowRace(string Race)
        {
            var raceHeroes = _listHeroes
                .Where(hero => hero.Model.General.Race.Equals(Race))
                .ToList();

            _cardPool.ShowDatas(raceHeroes);
        }

        public void ShowAllCards()
        {
            _cardPool.ShowDatas(_listHeroes);
        }

        public void RemoveCards(List<Card> cardsForRemove)
        {
            foreach (var card in cardsForRemove)
            {
                _cardPool.RemoveElement(card);
            }
        }

        public void RemoveCard(Card cardForRemove)
        {
            _cardPool.RemoveElement(cardForRemove);
        }

        private void OnCardClick(Card card)
        {
            if (!card.Selected)
            {
                _onSelect.Execute(card.Hero);
            }
            else
            {
                _onDiselect.Execute(card.Hero);
            }
        }

        public void SelectCards(List<GameHero> selectedCard)
        {
            Card currentCard = null;
            GameHero currentHero = null;
            for (int i = 0; i < selectedCard.Count; i++)
            {
                currentHero = selectedCard[i];
                currentCard = Cards.Find(x => x.Hero == currentHero);
                if (currentCard == null)
                    continue;

                currentCard.Select();
            }
        }

        public void UnselectCards(List<GameHero> unselectedCard)
        {
            Card currentCard = null;
            GameHero currentHero = null;
            for (int i = 0; i < unselectedCard.Count; i++)
            {
                currentHero = unselectedCard[i];
                currentCard = Cards.Find(x => x.Hero == currentHero);
                if (currentCard != null)
                {
                    currentCard.Unselect();
                }
            }
        }

        public void UnselectCard(GameHero hero)
        {
            var card = Cards.Find(x => x.Hero == hero);
            if (card != null)
            {
                card.Unselect();
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}