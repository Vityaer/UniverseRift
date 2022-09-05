using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{
 	public void StartWait(){
 		EndTurn();
 	}
 	public void StartDefend(){
		Buff buffDefend = new Buff(TypeBuff.Armor, 1f);
		statusState.SetBuff(buffDefend);
		EndTurn();
	}
	public void UseSpecialSpell(){
		if(statusState.Stamina == 100f){
			DoSpell();
		}
	}
}	