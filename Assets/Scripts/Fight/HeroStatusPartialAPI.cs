﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroStatusScript : MonoBehaviour{

	public State currentState = State.Clear;  
	public bool GetCanAction(){ 
		bool result = true;
		switch(currentState){
			case State.Stun:
			case State.Freezing:
			case State.Petrification:
				result = false;		
				break;
		}
		return result;
	}
	public void SetDebuff(State debuff, int rounds){
		if(currentState != State.Clear) 
			FightEffectControllerScript.Instance.ClearEffectStateOnHero(gameObject, currentState);
		SaveDebuff(debuff, rounds);
	}
	
	public void SetDot(TypeDot dot, float amount, TypeNumber typeNumber, List<Round> rounds){
		for(int i = 0; i < rounds.Count; i++) 
			if(rounds[i].amount == 0) rounds[i].SetData(amount, typeNumber);
		SaveDot(dot, rounds);
	}

	public void SetMark(Mark mark, float amount, List<Round> rounds){

	}	


	public bool PermissionMakeStrike(Strike strike){ // разрешение на атаку
		bool result = true;
		if(currentState == State.Astral){
			if(strike.type == TypeStrike.Physical)
				result = false;
		}
		return result;
	}

	public bool PermissionGetStrike(Strike strike){ // разрешение на получение урона
		bool result = true;
		if(currentState == State.Astral){
			if(strike.type == TypeStrike.Magical){
				strike.AddBonus(50f, TypeNumber.Percent);
			}else{
				result = false;		
			}
		}
		return result;
	}
	void OnDestroy(){
		FightControllerScript.Instance.UnregisterOnEndRound(RoundFinish);
	}
}


public enum State{
	Stun,
	Petrification,
	Freezing,
	Astral,
	Silence,
	Clear
}

public enum TypeDot{
	Poison,
	Bleending,
	Rot,
	Corrosion,
	Combustion
}

public enum Mark{
	Provocation,
	Berserk,
	Hellish
}