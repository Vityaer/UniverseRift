using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ActionEffect{
	private void ExecuteSimpleAction(){
		Debug.Log("simple action: " + simpleAction.ToString());
		switch (simpleAction){
			case EffectSimpleAction.Damage:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.GetDamage(new Strike(amount, heroController.hero.characts.GeneralAttack, typeNumber: typeNumber));
				break;
			case EffectSimpleAction.Heal:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.GetHeal((int) Mathf.Floor(amount), typeNumber);
				break;
			case EffectSimpleAction.HealFromDamage:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.GetHeal((int) Mathf.Floor(amount), typeNumber);
				break;		
		}
	}
	
	private void ExecuteBuff(){
		switch(effectBuff){
			case EffectBuff.HateGood:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.Good, amount);	
				break;
			case EffectBuff.HateBad:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.Bad, amount);	
				break;	
			case EffectBuff.HateUndead:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.Undead, amount);
				break;
			case EffectBuff.HateElf:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.Elf, amount);	
				break;
			case EffectBuff.HatePeople:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.People, amount);	
				break;
			case EffectBuff.HateGods:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.God, amount);
				break;
			case EffectBuff.HateDarkGods:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.DarkGod, amount);
				break;	
			case EffectBuff.HateBoss:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.SetHate(Attachment.Boss, amount);
				break;
		}
	}
	private void ExecuteDebuff(){
	}
	private void ExecuteDots(){
		switch(effectDots){
			case EffectDots.Poison:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDot(TypeDot.Poison, amount, typeNumber, rounds);
				break;
			case EffectDots.Bleending:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDot(TypeDot.Bleending, amount, typeNumber, rounds);
				break;
			case EffectDots.Rot:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDot(TypeDot.Rot, amount, typeNumber, rounds);
				break;
			case EffectDots.Corrosion:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDot(TypeDot.Corrosion, amount, typeNumber, rounds);
				break;
			case EffectDots.Combustion:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDot(TypeDot.Combustion, amount, typeNumber, rounds);
				break;
		}
	}
	private void ExecuteChangeCharacteristic(){
		switch(effectChangeCharacteristic){
			case EffectChangeCharacteristic.ChangeMaxHP:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.ChangeMaxHP((int) Mathf.Floor(amount), typeNumber);
				break;
			case EffectChangeCharacteristic.ChangeAttack:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangePhysicalAttack((int) Mathf.Floor(amount), typeNumber, rounds);
				break;
			case EffectChangeCharacteristic.ChangeArmor:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeArmor((int) Mathf.Floor(amount), typeNumber, rounds);	
				break;		
			case EffectChangeCharacteristic.ChangeInitiative:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeInitiative((int) Mathf.Floor(amount), typeNumber, rounds);	
				break;
			case EffectChangeCharacteristic.ChangeMagicResistance:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeMagicResistance(amount, rounds);	
				break;
			case EffectChangeCharacteristic.ChangeCountTargetForSimpleAttack:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeCountTargetForSimpleAttack((int) Mathf.Floor(amount), rounds);	
				break;	
			case EffectChangeCharacteristic.ChangeCountTargetForSpell:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeCountTargetForSpell((int) Mathf.Floor(amount), rounds);	
				break;	
			case EffectChangeCharacteristic.ChangeProbabilityCriticalAttack:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeProbabilityCriticalAttack(amount, rounds);	
				break;	
			case EffectChangeCharacteristic.ChangeEfficiencyHeal:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeEfficiencyHeal(amount, rounds);	
				break;
			case EffectChangeCharacteristic.ChangeDodge:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeDodge(amount, rounds);	
				break;
			case EffectChangeCharacteristic.ChangeAccuracy:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeAccuracy(amount, rounds);
				break;
			case EffectChangeCharacteristic.ChangeStamina:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.hero.ChangeStamina((int) Mathf.Floor(amount), rounds);
				break;
		}
	}
	private void ExecuteStatusHero(){
		Debug.Log("effectStatus: " + effectStatus.ToString() + " on listTarget.Count:" + listTarget.Count.ToString());
		switch(effectStatus){
			case EffectStatus.Stun:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDebuff(State.Stun, (int) amount);
				break;
			case EffectStatus.Petrification:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDebuff(State.Petrification, (int) amount);	
				break;
			case EffectStatus.Freezing:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDebuff(State.Freezing, (int) amount);	
				break;
			case EffectStatus.Astral:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDebuff(State.Astral, (int) amount);
				break;
			case EffectStatus.Silence:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetDebuff(State.Silence, (int) amount);
				break;
		}
	}
	private void ExecuteMark(){
		switch(effectMark){
			case EffectMark.Hellish:
				foreach (HeroControllerScript heroController in listTarget)
					heroController.statusState.SetMark(Mark.Hellish, amount, rounds);	
				break;
		}
	}
	private void ExecuteSpecial(){

	}
	private void ExecuteOther(){

	}		
}
