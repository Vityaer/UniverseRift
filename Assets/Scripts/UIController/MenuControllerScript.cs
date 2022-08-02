using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuControllerScript : MonoBehaviour{
	[SerializeField] private GameObject canvasMainMenu;
	public CityControllerScript canvasCity;
	private static MenuControllerScript instance;
	public  static MenuControllerScript Instance{get => instance;}
	private FooterButtonScript currentPage;
	public FooterButtonScript startPage;
	public GameObject background;
	void Awake(){
		instance = this;
	}
	private List<InfoHero> listHeroes = new List<InfoHero>();
	void Start(){
		PlayerScript.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
		OpenPage(startPage);
	}

	private bool isInteractableButtons = false;
	private void OnChangeListHeroes(InfoHero hero){
		if(isInteractableButtons != (listHeroes.Count > 0)){
			isInteractableButtons = !isInteractableButtons;
			InteractableBtnCampaign(isInteractableButtons);
			InteractableBtnTrainCamp(isInteractableButtons);
		}
	}
//API

	public void OpenMainPage(){
		canvasMainMenu.SetActive(true);
	}	   
	public void CloseMainPage(){
		canvasCity.Close();
		canvasMainMenu.SetActive(false);
	}
	[SerializeField] private Button btnTrainCamp;
	private void InteractableBtnTrainCamp(bool flag){
		btnTrainCamp.interactable = flag;
	}
	[SerializeField] private Button btnCampaign;
	private void InteractableBtnCampaign(bool flag){
		btnCampaign.interactable = flag;
	}
	public void OpenPage(FooterButtonScript newPage){
		if(currentPage != newPage){
			if(currentPage != null) currentPage.UnSelect();
			newPage.Select();
			currentPage = newPage;
		}
	}
}
