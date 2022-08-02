using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListRequirementHeroesUI : MonoBehaviour{

	public LevelUpRatingHeroScript mainController;
	[SerializeField] private List<RequireCardScript> requireCards = new List<RequireCardScript>();
	List<RequirementHero> requirementHeroes = new List<RequirementHero>();
	public void SetData(List<RequirementHero> requirementHeroes){
		this.requirementHeroes = requirementHeroes;
		for(int i = 0; i < requireCards.Count; i++){
			if(i < requirementHeroes.Count){
				requireCards[i].SetData(requirementHeroes[i]);
			}else{
				requireCards[i].Hide();
			}
		}
	}
	public bool GetCanLevelUpRating(){
		bool result = false;
		return result;
	}
	public void HeroSelectDiselect(){
		mainController.CheckHeroes();
	}
	public bool IsAllDone(){
		Debug.Log("is all done?");
		bool result = true;
		for(int i = 0; i< requirementHeroes.Count; i++){
			if(requireCards[i].CheckHeroes()){
				result = false;
				break;
			}
		}
		return result;
	}
	public void ClearData(){
		foreach(RequireCardScript requireCard in requireCards){
			requireCard.ClearData();
		}
	}
	public void DeleteSelectedHeroes(){
		for(int i = 0; i< requirementHeroes.Count; i++){
			requireCards[i].DeleteSelectedHeroes();
		}
	}
}