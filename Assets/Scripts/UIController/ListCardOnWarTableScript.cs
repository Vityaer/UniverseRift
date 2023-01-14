using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; 
public class ListCardOnWarTableScript : MonoBehaviour{

	public GameObject prefabCard;
	private List<InfoHero> listHeroes = new List<InfoHero>(); 
	[SerializeField] private List<CardScript> listCard = new List<CardScript>();
	[SerializeField] private bool loadedListHeroes = false;
	public bool LoadedListHeroes{get => loadedListHeroes;}
	private void CreateCards(List<InfoHero> heroes){
		GameObject cardObject;
		CardScript cardScript;
		foreach (InfoHero hero in heroes){
			cardObject = Instantiate(prefabCard, transform);
			cardScript = cardObject.GetComponent<CardScript>();
			listCard.Add(cardScript);
			cardScript.ChangeInfo(hero, this); 
		}
	}
	public void ShowRace(int Race){
		foreach(CardScript card in listCard){
			card.gameObject.SetActive((int) card.hero.generalInfo.race == Race);
		}
	}
	public void ShowAllCards(){
		foreach (CardScript card in listCard){
			card.gameObject.SetActive(true);
		}
	} 

	private void SortOfLevel(){
		InfoHero helpCard = null;
		int levelFirst, levelSecond;
		for(int i = 0; i < listHeroes.Count; i++){
			for(int j = i + 1; j < listHeroes.Count; j++){
				levelFirst = listHeroes[i].generalInfo.Level; 
				levelSecond = listHeroes[i].generalInfo.Level;
				if((levelFirst < levelSecond) || ((levelFirst == levelSecond) && (listHeroes[i].generalInfo.ratingHero < listHeroes[j].generalInfo.ratingHero))){
					helpCard = listHeroes[i];
					listHeroes[i] = listHeroes[j];
					listHeroes[j] = helpCard;
				}
			}
		}
		UpdateAllCard();
	}
	private void SortOfRating(){
		InfoHero helpCard = null;
		for(int i = 0; i < listHeroes.Count; i++){
			for(int j = i + 1; j < listHeroes.Count; j++){
				if((listHeroes[i].generalInfo.ratingHero < listHeroes[j].generalInfo.ratingHero)){
					helpCard = listHeroes[i];
					listHeroes[i] = listHeroes[j];
					listHeroes[j] = helpCard;
				}
			}
		}
		UpdateAllCard();

	}  
//API
	public void Sort(Toggle toggle){if(toggle.isOn){ SortOfRating(); }else{ SortOfLevel(); } }
	public void RemoveCards(List<CardScript> cardsForRemove){
		foreach(CardScript card in cardsForRemove){
			card.DestroyCard();
		}
	}
	public void SetList(List<InfoHero> listHeroes){
		this.listHeroes = listHeroes;
		loadedListHeroes = true;
		Clear();
		CreateCards(listHeroes);
	} 
	public void EventOpen(){
		ShowAllCards();
		UpdateAllCard();
	}
	public void EventClose(){
	} 

	public void Clear(){
		for(int i = listCard.Count - 1; i>=0; i--){
			Destroy(listCard[i].gameObject);
		}
		listCard.Clear();
	} 
//Delegate
	public void SelectCard(CardScript card){ EventSelectCard(card); }
	public void UnSelectCard(CardScript card){ EventUnSelectCard(card); }
	private Action<CardScript> delSelect, delUnSelect;
	public void RegisterOnSelect(Action<CardScript> d){ delSelect += d; }
	public void UnRegisterOnSelect(Action<CardScript> d){ delSelect -= d; }
	public void RegisterOnUnSelect(Action<CardScript> d){ delUnSelect += d; }
	public void UnRegisterOnUnSelect(Action<CardScript> d){ delUnSelect -= d; }
	
	private void EventSelectCard(CardScript card)
	{
		if((delSelect != null) && (card != null)){ delSelect(card); }
	}

	private void EventUnSelectCard(CardScript card)
	{
		if((delUnSelect != null) && (card != null)){ delUnSelect(card); }
	}

	private void UpdateAllCard()
	{
		for(int i =0; i < listCard.Count; i++){
			listCard[i].ChangeInfo(listHeroes[i]);
		}
	}

	public void RemoveCardFromList(CardScript card)
	{
		listCard.Remove(card);
	}

	public void SelectCards(List<InfoHero> selectedCard)
	{
		CardScript currentCard = null;
		InfoHero currentHero = null;
		for(int i= 0; i < selectedCard.Count; i++){
			currentHero = selectedCard[i]; 
			currentCard = listCard.Find(x => x.hero == currentHero);
			if(currentCard != null){
				currentCard.Selected();
			}
		}
	}
}
