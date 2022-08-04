using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SanctuaryScript : BuildingWithHeroesList{

	
	public Resource costReplacementFourRating, costReplacementFiveRating;
	public TavernScript tavernController;
	private CardScript selectedHero;
	public Button btnSave, btnReplacement;
	public InfoHero newHero;


	private List<InfoHero> heroes = new List<InfoHero>(); 
	public void ReplacementHero(){
		Resource resCost = null;
		if(selectedHero != null){
			if(selectedHero.hero.generalInfo.ratingHero == 4){
				resCost = costReplacementFourRating;
				heroes  = tavernController.GetListHeroes.FindAll(x => (x.generalInfo.ratingHero == 4));
			}else{
				resCost = costReplacementFiveRating;
				heroes  = tavernController.GetListHeroes.FindAll(x => (x.generalInfo.ratingHero == 5));
			}

			if(PlayerScript.Instance.CheckResource(resCost)){
				heroes  = heroes.FindAll(x => ((x.generalInfo.ratingHero == selectedHero.hero.generalInfo.ratingHero) && (x.generalInfo.race == selectedHero.hero.generalInfo.race)&&(x.generalInfo.idHero != selectedHero.hero.generalInfo.idHero) ));
				if(heroes.Count > 0){
					newHero = (InfoHero) heroes[ Random.Range(0, heroes.Count) ].Clone();
					btnSave.gameObject.SetActive(true);
				}
			}
		}
	}

	public void SaveReplacement(){
		if(newHero != null){
			listHeroesController.RemoveCards( new List<CardScript>{selectedHero} );
			PlayerScript.Instance.RemoveHero(selectedHero.hero);
			PlayerScript.Instance.AddHero(newHero);
			btnSave.gameObject.SetActive(false);
		}
	}
	public override void SelectHero(CardScript newCardHero){
		if(selectedHero != null) selectedHero.UnSelected();
		selectedHero = newCardHero;
		selectedHero.Selected();
		btnReplacement.interactable = true;
	}

	protected override void OpenPage(){
		if(listHeroesController.LoadedListHeroes == false){ 
			PlayerScript.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
			LoadListHeroes();
		}
		listHeroesController.EventOpen();
		listHeroesController.RegisterOnSelect(SelectHero);
	}
	protected override void ClosePage(){
		btnSave.gameObject.SetActive(false);
		selectedHero = null;
		listHeroesController.UnRegisterOnSelect(SelectHero);
		listHeroesController.EventClose();
	}

}
