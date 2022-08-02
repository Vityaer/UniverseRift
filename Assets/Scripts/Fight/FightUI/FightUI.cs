using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUI : MonoBehaviour{

	public FightDirectionsControllerScript SelectDirection;


	public MelleeAtackDirectionController melleeAttackController{ get => SelectDirection.melleeAttackController;}
	private static FightUI instance;
	public static FightUI Instance{get => instance;}
	void Awake(){
		instance = this;
	}

	public void WaitTurn(){
		if(HexagonGridScript.PlayerCanController){
			FightControllerScript.Instance.WaitTurn();
		}
	}
	public void StartDefend(){
		if(HexagonGridScript.PlayerCanController){
			FightControllerScript.Instance.GetCurrentHero().StartDefend();
		}
	}
}