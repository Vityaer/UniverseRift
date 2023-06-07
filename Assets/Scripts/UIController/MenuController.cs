using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[SerializeField] private GameObject canvasMainMenu;
	[SerializeField] private Button btnTrainCamp;
	[SerializeField] private Button btnCampaign;

	private bool isInteractableButtons = false;
	private static MenuController instance;
	public  static MenuController Instance{get => instance;}
	public MainPage CurrentPage;
	private FooterButton currentPageButton;
	public FooterButton startPageButton;
	public GameObject background;
	private List<HeroModel> listHeroes = new List<HeroModel>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		GameController.Instance.GetListHeroesWithObserver(ref listHeroes, OnChangeListHeroes);
		OpenPage(startPageButton);
	}

	private void OnChangeListHeroes(HeroModel hero){
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

	public void OpenPage(FooterButton newPageButton){
		if(currentPageButton != newPageButton){
			if(currentPageButton != null) currentPageButton.UnSelect();
			newPageButton.Select();
			currentPageButton = newPageButton;
		}
	}
}
