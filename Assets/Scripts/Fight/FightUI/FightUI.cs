using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FightUI : MonoBehaviour{

	public FightDirectionsControllerScript SelectDirection;


	public MelleeAtackDirectionController melleeAttackController{ get => SelectDirection.melleeAttackController;}
	private static FightUI instance;
	public static FightUI Instance{get => instance;}
	void Awake(){
		instance = this;
	}
	void Start(){
		FightControllerScript.Instance.RegisterOnFinishFight(CloseControllers);
		FightControllerScript.Instance.RegisterOnEndRound(ClearData);
	}
	private List<HeroControllerScript> listWaits = new List<HeroControllerScript>();
	public void WaitTurn(){
		if(HexagonGridScript.PlayerCanController){
			FightControllerScript.Instance.WaitTurn();
			listWaits.Add(FightControllerScript.Instance.GetCurrentHero());
		}
	}
	public void StartDefend(){
		if(HexagonGridScript.PlayerCanController){
			FightControllerScript.Instance.GetCurrentHero().StartDefend();
		}
	}
	public Button btnSpell, btnWait;
	public void UseSpell(){
		FightControllerScript.Instance.GetCurrentHero().UseSpecialSpell();
	}

//API
	public RectTransform panelControllers;
	private HeroControllerScript heroController;
	public void OpenControllers(HeroControllerScript heroController){
		this.heroController = heroController;
		panelControllers.gameObject.SetActive(true);
		HeroControllerScript.RegisterOnEndAction(ClearController);
		btnWait.interactable = !listWaits.Contains(heroController);
		btnSpell.interactable = (heroController.Stamina == 100f);
	}
	public void CloseControllers(){
		panelControllers.gameObject.SetActive(false);
	}	

	private void ClearController(){
		HeroControllerScript.UnregisterOnEndAction(ClearController);
		heroController = null;
		btnSpell.interactable = false;
	}
	void ClearData(){
		listWaits.Clear();
	}
}