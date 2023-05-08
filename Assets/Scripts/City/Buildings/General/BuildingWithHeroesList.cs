using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithHeroesList : Building{
	
	[SerializeField] protected ListCardOnWarTable listHeroesController;
	protected List<InfoHero> listHeroes = new List<InfoHero>();

    public virtual void SelectHero(Card cardHero){}

	public virtual void UnselectHero(Card cardHero){}

	protected virtual void FilterHeroes(List<InfoHero> heroes){}

	protected void LoadListHeroes(){
		FilterHeroes(listHeroes);
		listHeroesController.SetList(listHeroes);
		listHeroesController.RegisterOnSelect(SelectHero);
	}

	protected void OnDestroy(){
		listHeroesController.UnRegisterOnSelect(SelectHero);
	}
}
