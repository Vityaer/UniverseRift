using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithHeroesList : Building{
	
	[SerializeField] protected ListCardOnWarTable listHeroesController;
	protected List<HeroModel> listHeroes = new List<HeroModel>();

    public virtual void SelectHero(Card cardHero){}

	public virtual void UnselectHero(Card cardHero){}

	protected virtual void FilterHeroes(List<HeroModel> heroes){}

	protected void LoadListHeroes(){
		FilterHeroes(listHeroes);
		listHeroesController.SetList(listHeroes);
		listHeroesController.RegisterOnSelect(SelectHero);
	}

	protected void OnDestroy(){
		listHeroesController.UnRegisterOnSelect(SelectHero);
	}
}
