using System.Collections;
using System.Collections.Generic;
using IdleGame.Touch;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrainCampScript : MonoBehaviour{

	private Canvas trainCanvas;

	public List<InfoHero> listHeroes = new List<InfoHero>();
	public int numSelectHero = 0;
	private InfoHero hero;
	[Header("Controller")]
	public GameObject btnToLeftList;
	public GameObject btnToRightList;
	public Button btnLevelUP;
	public ListCardOnWarTableScript listCardPanel;
	public ListMyHeroesControllerScript ListCard;
	public CostLevelUp costLevelObject;
	private bool isOpen = false;

	private static TrainCampScript instance;
	public static TrainCampScript Instance{get => instance;}

	[Header("Information")]
	public Image imageHero;
	public TextMeshProUGUI textLevel;
	public TextMeshProUGUI textNameHero;
	public TextMeshProUGUI textHP;
	public TextMeshProUGUI textAttack;
	public TextMeshProUGUI textArmor;
	public TextMeshProUGUI textInitiative;
	public TextMeshProUGUI textStrengthHero;

	[Header("Items")]
	public List<CellItemHeroScript> CellsForItem = new List<CellItemHeroScript>(); 
	[Header("Skills")]
	public SkillUIControllerScript skillController;
	[Header("Costs")]
	public CostUIListScript costController;


	private void SelectHero(int num){
		if(num <= 0)
		{
			num = 0;
			btnToLeftList.SetActive(false);
		}else
		{
			btnToLeftList.SetActive(true);
		}
		if(num >= (listHeroes.Count - 1))
		{
			num = listHeroes.Count - 1;
			btnToRightList.SetActive(false);
		}else
		{
			btnToRightList.SetActive(true);
		}

		numSelectHero = num;
		hero = listHeroes[numSelectHero];
		UpdateInfoAbountHero();
	}


	public void UpdateInfoAbountHero(){
		imageHero.sprite    = hero.generalInfo.ImageHero;
		textNameHero.text   = hero.generalInfo.Name;
		UpdateTextAboutHero();
		foreach(CellItemHeroScript cell in CellsForItem){
			cell.Clear();
			cell.SetItem(hero.CostumeHero.GetItem(cell.typeCell));
		}
		CheckResourceForLevelUP();
	} 
	public void UpdateTextAboutHero(){
		textLevel.text      =  hero.generalInfo.Level.ToString();
		textHP.text         = ((int) hero.GetCharacteristic(TypeCharacteristic.HP)        ).ToString();
		textAttack.text     = ((int) hero.GetCharacteristic(TypeCharacteristic.Damage)    ).ToString();
		textArmor.text      = ((int) hero.GetCharacteristic(TypeCharacteristic.Defense)   ).ToString();
		textInitiative.text = ((int) hero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
		textStrengthHero.text  = hero.GetStrength.ToString(); 
		hero.PrepareSkillLocalization();
		skillController.ShowSkills(hero.skills);
		costController.ShowCosts( costLevelObject.GetCostForLevelUp(hero.generalInfo.Level) );
	}
	private void CheckResourceForLevelUP(){
		btnLevelUP.interactable = PlayerScript.Instance.CheckResource( costLevelObject.GetCostForLevelUp(hero.generalInfo.Level) );
	}

	
//API
	public void TakeOff(Item item){
		hero.CostumeHero.TakeOff(item);
		UpdateTextAboutHero();
	}
	public void SelectHero(CardScript card){
		numSelectHero = listHeroes.FindIndex(x => x == card.hero);
		SelectHero(numSelectHero);
		if(isOpen == false) OpenTrainCamp();
	}
	public InfoHero ReturnSelectHero(){ return listHeroes[numSelectHero]; }
	public void LevelUpHero(){
		PlayerScript.Instance.SubtractResource( costLevelObject.GetCostForLevelUp(hero.generalInfo.Level) );
		hero.LevelUP();
		UpdateInfoAbountHero();
	}

	public void NextHero(){
		SelectHero(numSelectHero + 1);
	}

	public void PreviousHero(){
		SelectHero(numSelectHero - 1);
	}

	private void OpenTrainCamp(){
		MainTouchControllerScript.Instance.RegisterOnObserverSwipe(OnSwipe);
		isOpen = true;
		ListCard.Close();
		trainCanvas.enabled = true;
		MenuControllerScript.Instance.CloseMainPage();
	}	
	public void Open(){
		ListCard.Open();
	}
	public void CloseTrainCamp(){
		MainTouchControllerScript.Instance.UnregisterOnObserverSwipe(OnSwipe);
		isOpen = false;
		trainCanvas.enabled = false;
		MenuControllerScript.Instance.OpenMainPage();
	} 

	void Awake(){
		instance = this;
		trainCanvas = GetComponent<Canvas>();
		if(trainCanvas.enabled){
			isOpen = false;
			trainCanvas.enabled = false;
		}
	} 
	void Start(){
		listHeroes = PlayerScript.Instance.GetListHeroes;
		listCardPanel.RegisterOnSelect(SelectHero);
	}
	private void OnSwipe(TypeSwipe typeSwipe){
		switch(typeSwipe){
			case TypeSwipe.Left:
				PreviousHero();
				break;
			case TypeSwipe.Right:
				NextHero();
				break;	
		}
	}
}
