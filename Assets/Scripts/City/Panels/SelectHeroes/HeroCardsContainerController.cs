using Hero;
using System;
using System.Collections.Generic;
using UIController.Cards;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace City.Panels.SelectHeroes
{
    public class HeroCardsContainerController : MonoBehaviour, IDisposable
    {
        public Transform Content;
        public Card Prefab;

        private List<Card> listCard = new List<Card>();
        private bool loadedListHeroes = false;

        private List<GameHero> listHeroes = new List<GameHero>();
        private ReactiveCommand<GameHero> _onSelect = new ReactiveCommand<GameHero>();
        private ReactiveCommand<GameHero> _onDiselect = new ReactiveCommand<GameHero>();
        private CompositeDisposable _disposables = new CompositeDisposable();

        public bool LoadedListHeroes => loadedListHeroes;
        public IObservable<GameHero> OnSelect => _onSelect;
        public IObservable<GameHero> OnDiselect => _onDiselect;

        public void ShowCards(List<GameHero> heroes)
        {
            if (heroes == null)
                return;

            CheckCountCards(heroes.Count);

            for(var i = 0; i < heroes.Count; i++)
            {
                listCard[i].SetData(heroes[i]);
            }

            for (var i = heroes.Count; i < listCard.Count; i++)
            {
                listCard[i].Clear();
            }
        }

        private void CheckCountCards(int requireCount)
        {
            if (listCard.Count < requireCount)
            {
                for (int i = listCard.Count; i < requireCount; i++)
                {
                    var card = Instantiate(Prefab, Content);
                    card.OnClick.Subscribe(OnCardClick).AddTo(_disposables);
                    listCard.Add(card);
                }
            }
        }

        public void ShowRace(string Race)
        {
            foreach (var card in listCard)
            {
                card.gameObject.SetActive(card.Hero.Model.General.Race == Race);
            }
        }

        public void ShowAllCards()
        {
            foreach (Card card in listCard)
            {
                card.gameObject.SetActive(true);
            }
        }

        private void SortOfLevel()
        {
            //HeroModel helpCard = null;
            //int levelFirst, levelSecond;
            //for (int i = 0; i < listHeroes.Count; i++)
            //{
            //    for (int j = i + 1; j < listHeroes.Count; j++)
            //    {
            //        levelFirst = listHeroes[i].General.Level;
            //        levelSecond = listHeroes[i].General.Level;
            //        if (levelFirst < levelSecond || levelFirst == levelSecond && listHeroes[i].General.RatingHero < listHeroes[j].General.RatingHero)
            //        {
            //            helpCard = listHeroes[i];
            //            listHeroes[i] = listHeroes[j];
            //            listHeroes[j] = helpCard;
            //        }
            //    }
            //}
            //UpdateAllCard();
        }

        private void SortOfRating()
        {
            //HeroModel helpCard = null;
            //for (int i = 0; i < listHeroes.Count; i++)
            //{
            //    for (int j = i + 1; j < listHeroes.Count; j++)
            //    {
            //        if (listHeroes[i].General.RatingHero < listHeroes[j].General.RatingHero)
            //        {
            //            helpCard = listHeroes[i];
            //            listHeroes[i] = listHeroes[j];
            //            listHeroes[j] = helpCard;
            //        }
            //    }
            //}
            //UpdateAllCard();
        }

        //API
        public void Sort(Toggle toggle)
        {
            if (toggle.isOn)
            {
                SortOfRating();
            }
            else
            {
                SortOfLevel();
            }
        }

        public void RemoveCards(List<Card> cardsForRemove)
        {
            foreach (Card card in cardsForRemove)
            {
                //card.DestroyCard();
            }
        }

        public void EventOpen()
        {
            ShowAllCards();
            UpdateAllCard();
        }

        public void EventClose()
        {
        }

        public void Clear()
        {
            listCard.ForEach(card => card.Clear());
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

        private void UpdateAllCard()
        {
            for (int i = 0; i < listCard.Count; i++)
            {
                listCard[i].SetData(listHeroes[i]);
            }
        }

        public void RemoveCardFromList(Card card)
        {
            listCard.Remove(card);
        }

        public void SelectCards(List<GameHero> selectedCard)
        {
            Card currentCard = null;
            GameHero currentHero = null;
            for (int i = 0; i < selectedCard.Count; i++)
            {
                currentHero = selectedCard[i];
                currentCard = listCard.Find(x => x.Hero == currentHero);
                if (currentCard != null)
                {
                    currentCard.Select();
                }
            }
        }

        public void UnselectCards(List<GameHero> unselectedCard)
        {
            Card currentCard = null;
            GameHero currentHero = null;
            for (int i = 0; i < unselectedCard.Count; i++)
            {
                currentHero = unselectedCard[i];
                currentCard = listCard.Find(x => x.Hero == currentHero);
                if (currentCard != null)
                {
                    currentCard.Unselect();
                }
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}