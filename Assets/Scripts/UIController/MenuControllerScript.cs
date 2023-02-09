using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControllerScript : MonoBehaviour
{
	[SerializeField] private GameObject canvasMainMenu;
	[SerializeField] private Button btnTrainCamp;
	[SerializeField] private Button btnCampaign;

	private bool isInteractableButtons = false;
	private static MenuControllerScript instance;
	public  static MenuControllerScript Instance{get => instance;}
	public MainPage CurrentPage;
	private FooterButtonScript currentPageButton;
	public FooterButtonScript startPageButton;
	public GameObject background;
	private List<InfoHero> listHeroes = new List<InfoHero>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		PlayerScript.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
		OpenPage(startPageButton);
	}

	private void OnChangeListHeroes(InfoHero hero){
		if(isInteractableButtons != (listHeroes.Count > 0)){
			isInteractableButtons = !isInteractableButtons;
			InteractableBtnCampaign(isInteractableButtons);
			InteractableBtnTrainCamp(isInteractableButtons);
		}
	}
//API
	public void OpenMainPage()
	{
		CurrentPage.Open();
		canvasMainMenu.SetActive(true);
	}	

	public void CloseMainPage()
	{
		CurrentPage.Close();
		canvasMainMenu.SetActive(false);
	}

	private void InteractableBtnTrainCamp(bool flag)
	{
		btnTrainCamp.interactable = flag;
	}

	private void InteractableBtnCampaign(bool flag)
	{
		btnCampaign.interactable = flag;
	}

	public void OpenPage(FooterButtonScript newPageButton){
		if(currentPageButton != newPageButton){
			if(currentPageButton != null) currentPageButton.UnSelect();
			newPageButton.Select();
			currentPageButton = newPageButton;
		}
	}
}
