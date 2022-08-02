using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListMyHeroesControllerScript : MonoBehaviour{

	[SerializeField] private ListCardOnWarTableScript listHeroesController;
	protected List<InfoHero> listHeroes = new List<InfoHero>();

	[Header("UI")]
	private Canvas canvas;
	public GameObject background;
	public FooterButtonScript btnOpenClose;
  	void Awake(){
		canvas = GetComponent<Canvas>();
		btnOpenClose.RegisterOnChange(Change);
	}
	void Change(bool isOpen){
		if(isOpen){ Open(); }else{ Close(); }
	}
	void LoadListHeroes(){
		PlayerScript.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
		listHeroesController.SetList(listHeroes);
	}
	public void Open(){
		if(listHeroesController.LoadedListHeroes == false) LoadListHeroes();
		canvas.enabled = true;
		listHeroesController.EventOpen();
		BackGroundControllerScript.Instance.OpenBackground(background);
	}
	public void Close(){
		canvas.enabled = false;
		listHeroesController.EventClose();
	}
	protected void OnChangeListHeroes(InfoHero hero){
		listHeroesController.ChangeList(hero);
	}
}
