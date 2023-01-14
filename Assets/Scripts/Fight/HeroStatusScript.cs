using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class HeroStatusScript : MonoBehaviour{
	public SliderScript sliderHP;
	public SliderScript sliderStamina;
	private Vector2 delta = new Vector2(0, 30);
	private HeroControllerScript heroController;
	// private GameObject panelStatus;
	void Awake(){
		heroController = GetComponent<HeroControllerScript>();
	}
	void Start(){
		FightControllerScript.Instance.RegisterOnEndRound(RoundFinish);
		gameObject.transform.Find("CanvasHeroesStatus").gameObject.SetActive(true);
	}
//Helth	
	private float currentHP;
	public void ChangeHP(float HP){
		if(HP < currentHP){ListFightTextsScript.Instance.ShowDamage(currentHP - HP, gameObject.transform.position);} else{ListFightTextsScript.Instance.ShowHeal(HP - currentHP, gameObject.transform.position);}
		currentHP = HP;
		sliderHP.ChangeValue(HP);
		if((HP / sliderHP.maxValue < 0.5f) &&(HP / sliderHP.maxValue > 0.3f)){
			heroController.OnHPLess50();
		}else if(HP / sliderHP.maxValue < 0.3f){
			heroController.OnHPLess30();
		}
	}
	public void SetMaxHealth(float maxHP){
		if(currentHP == 0) currentHP = maxHP;
		sliderHP.SetMaxValue(maxHP);
	}
	public void ChangeMaxHP(float amountChange){
		SetMaxHealth(sliderHP.maxValue + amountChange);
	}
	public void Death(){
		sliderHP.Death();
		sliderStamina.Death();
	}
//Stamina
	private int stamina = 25;
	public int Stamina{get => stamina;}
	public void ChangeStamina(int addStamina){
		Debug.Log("change stamina: " + addStamina.ToString());
		stamina += addStamina;
		if(stamina > 100) stamina = 100;
		if(stamina < 0) stamina = 0;
		sliderStamina.ChangeValue(stamina);
		OnChangeStamina(stamina);
	}
	private Action<int> observerStamina;
	public void RegisterOnChangeStamina(Action<int> d){observerStamina += d;}
	public void UnregisterOnChangeStamina(Action<int> d){observerStamina -= d;}
	private void OnChangeStamina(int num){if(observerStamina != null) observerStamina(num);}
}
