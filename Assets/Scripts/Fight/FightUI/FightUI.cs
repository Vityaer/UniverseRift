using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fight.Grid;

public class FightUI : MonoBehaviour{

	public FightDirectionsControllerScript SelectDirection;
	private List<HeroControllerScript> listWaits = new List<HeroControllerScript>();
	public Button btnSpell, btnWait;
	public RectTransform panelControllers;
	private HeroControllerScript heroController;
	public MelleeAtackDirectionController melleeAttackController => SelectDirection.melleeAttackController;
	private static FightUI instance;
	public static FightUI Instance => instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		FightControllerScript.Instance.RegisterOnFinishFight(CloseControllers);
		FightControllerScript.Instance.RegisterOnEndRound(ClearData);
	}

	public void WaitTurn()
	{
		if(GridController.PlayerCanController)
		{
			FightControllerScript.Instance.WaitTurn();
			listWaits.Add(FightControllerScript.Instance.GetCurrentHero());
		}
	}

	public void StartDefend()
	{
		if(GridController.PlayerCanController)
		{
			FightControllerScript.Instance.GetCurrentHero().StartDefend();
		}
	}

	public void UseSpell()
	{
		FightControllerScript.Instance.GetCurrentHero().UseSpecialSpell();
	}

//API
	public void OpenControllers(HeroControllerScript heroController)
	{
		this.heroController = heroController;
		panelControllers.gameObject.SetActive(true);
		HeroControllerScript.RegisterOnEndAction(ClearController);
		btnWait.interactable = !listWaits.Contains(heroController);
		btnSpell.gameObject.SetActive(heroController.SpellExist);
		heroController.statusState.RegisterOnChangeStamina(HeroChangeStamina);
		HeroChangeStamina(heroController.Stamina);

	}

	private void HeroChangeStamina(int stamina)
	{
		Debug.Log($"HeroChangeStamina: {stamina}");
		btnSpell.interactable = (stamina == 100);
	}

	public void CloseControllers()
	{
		panelControllers.gameObject.SetActive(false);
	}	

	private void ClearController()
	{
		HeroControllerScript.UnregisterOnEndAction(ClearController);
		heroController?.statusState?.UnregisterOnChangeStamina(HeroChangeStamina);
		heroController = null;
		btnSpell.interactable = false;
	}

	void ClearData()
	{
		listWaits.Clear();
	}
}