using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListCardOnWarTable : MonoBehaviour
{
    public GameObject prefabCard;

    [SerializeField] private List<Card> listCard = new List<Card>();
    [SerializeField] private bool loadedListHeroes = false;

    private List<HeroModel> listHeroes = new List<HeroModel>();
    private Action<Card> delSelect, delUnSelect;

    public bool LoadedListHeroes => loadedListHeroes;

    private void CreateCards(List<HeroModel> heroes)
    {
        GameObject cardObject;
        Card cardScript;
        foreach (HeroModel hero in heroes)
        {
            cardObject = Instantiate(prefabCard, transform);
            cardScript = cardObject.GetComponent<Card>();
            listCard.Add(cardScript);
            cardScript.ChangeInfo(hero, this);
        }
    }

    public void ShowRace(string Race)
    {
        foreach (Card card in listCard)
        {
            card.gameObject.SetActive(card.hero.General.Race == Race);
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
        HeroModel helpCard = null;
        int levelFirst, levelSecond;
        for (int i = 0; i < listHeroes.Count; i++)
        {
            for (int j = i + 1; j < listHeroes.Count; j++)
            {
                levelFirst = listHeroes[i].General.Level;
                levelSecond = listHeroes[i].General.Level;
                if ((levelFirst < levelSecond) || ((levelFirst == levelSecond) && (listHeroes[i].General.RatingHero < listHeroes[j].General.RatingHero)))
                {
                    helpCard = listHeroes[i];
                    listHeroes[i] = listHeroes[j];
                    listHeroes[j] = helpCard;
                }
            }
        }
        UpdateAllCard();
    }
    private void SortOfRating()
    {
        HeroModel helpCard = null;
        for (int i = 0; i < listHeroes.Count; i++)
        {
            for (int j = i + 1; j < listHeroes.Count; j++)
            {
                if ((listHeroes[i].General.RatingHero < listHeroes[j].General.RatingHero))
                {
                    helpCard = listHeroes[i];
                    listHeroes[i] = listHeroes[j];
                    listHeroes[j] = helpCard;
                }
            }
        }
        UpdateAllCard();
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
            card.DestroyCard();
        }
    }

    public void SetList(List<HeroModel> listHeroes)
    {
        this.listHeroes = listHeroes;
        loadedListHeroes = true;
        Clear();
        CreateCards(listHeroes);
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
        for (int i = listCard.Count - 1; i >= 0; i--)
        {
            Destroy(listCard[i].gameObject);
        }
        listCard.Clear();
    }

    //Delegate
    public void SelectCard(Card card) { EventSelectCard(card); }
    public void UnselectCard(Card card) { EventUnSelectCard(card); }
    public void RegisterOnSelect(Action<Card> d) { delSelect += d; }
    public void UnRegisterOnSelect(Action<Card> d) { delSelect -= d; }
    public void RegisterOnUnSelect(Action<Card> d) { delUnSelect += d; }
    public void UnRegisterOnUnSelect(Action<Card> d) { delUnSelect -= d; }

    private void EventSelectCard(Card card)
    {
        if ((delSelect != null) && (card != null)) { delSelect(card); }
    }

    private void EventUnSelectCard(Card card)
    {
        if ((delUnSelect != null) && (card != null)) { delUnSelect(card); }
    }

    private void UpdateAllCard()
    {
        for (int i = 0; i < listCard.Count; i++)
        {
            listCard[i].ChangeInfo(listHeroes[i]);
        }
    }

    public void RemoveCardFromList(Card card)
    {
        listCard.Remove(card);
    }

    public void SelectCards(List<HeroModel> selectedCard)
    {
        Card currentCard = null;
        HeroModel currentHero = null;
        for (int i = 0; i < selectedCard.Count; i++)
        {
            currentHero = selectedCard[i];
            currentCard = listCard.Find(x => x.hero == currentHero);
            if (currentCard != null)
            {
                currentCard.Select();
            }
        }
    }
}
