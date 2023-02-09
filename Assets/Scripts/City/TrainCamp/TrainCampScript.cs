using System.Collections;
using System.Collections.Generic;
using IdleGame.Touch;
using UnityEngine;
using UnityEngine.UI;

public class TrainCampScript : MainPage
{
	public HeroPanel heroPanel;
	public List<InfoHero> listHeroes = new List<InfoHero>();
	public int numSelectHero = 0;
	private InfoHero hero;
	public ListCardOnWarTableScript listCardPanel;
	public ListMyHeroesControllerScript ListCard;

	private static TrainCampScript instance;
	public static TrainCampScript Instance => instance;

	protected override void Awake()
	{
		instance = this;
		base.Awake();
	} 
	
	private void SelectHero(int num)
	{
		if(num <= 0)
			num = 0;

		if(num >= (listHeroes.Count - 1))
			num = listHeroes.Count - 1;

		numSelectHero = num;
		hero = listHeroes[numSelectHero];
		heroPanel.ShowHero(hero);
	}
	
//API
	public void TakeOff(Item item)
	{
		heroPanel.TakeOff(item);
	}

	public void SelectHero(CardScript card)
	{
		numSelectHero = listHeroes.FindIndex(x => x == card.hero);
		SelectHero(numSelectHero);
		OpenHeroPanel();
	}

	public InfoHero ReturnSelectHero()
	{
		return listHeroes[numSelectHero];
	}

	public void OpenHeroPanel()
	{
		MainTouchControllerScript.Instance.RegisterOnObserverSwipe(OnSwipe);
		ListCard.Close();
		MenuControllerScript.Instance.CloseMainPage();
		heroPanel.Open();
	}

	public void CloseHeroPanel()
	{
		MainTouchControllerScript.Instance.UnregisterOnObserverSwipe(OnSwipe);
		heroPanel.Close();
		ListCard.Open();
	}

	public override void Open()
	{
		base.Open();
		ListCard.Open();
	}

	public override void Close()
	{
		ListCard.Close();
	} 

	void Start()
	{
		listHeroes = PlayerScript.Instance.GetListHeroes;
		listCardPanel.RegisterOnSelect(SelectHero);
		heroPanel.LeftButtonClick += () => SelectHero(numSelectHero - 1);
		heroPanel.RightButtonClick += () => SelectHero(numSelectHero + 1);
	}

	private void OnSwipe(TypeSwipe typeSwipe)
	{
		switch(typeSwipe){
			case TypeSwipe.Left:
				SelectHero(numSelectHero - 1);
				break;
			case TypeSwipe.Right:
				SelectHero(numSelectHero + 1);
				break;	
		}
	}
}
