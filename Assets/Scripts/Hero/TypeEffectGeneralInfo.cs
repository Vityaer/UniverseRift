using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectSimpleAction{
		Damage = 0,
		Heal = 1,
		HealFromDamage = 2
	}
	public enum EffectBuff{
		HateGood = 0,
		HateUndead = 1,
		HateElf = 2,
		HatePeople = 3,
		HateGods = 4,
		HateBoss = 5,
		HateBad = 6,
		HateDarkGods = 7
	}	
	public enum EffectDots{
		Poison = 0,
		Bleending = 1,
		Rot = 2,
		Corrosion = 3,
		Combustion = 4
	}
	public enum EffectChangeCharacteristic{
		ChangeMaxHP = 0,
		ChangeAttack = 1,
		ChangeCriticalAttack = 2,
		ChangeArmor = 3,
		ChangeInitiative = 4,
		ChangeMagicResistance = 5,
		ChangeCritResistance = 6,
		ChangePoisonResistance = 7,
		ChangeBleedingResistance = 8,
		ChangeCountTargetForSimpleAttack = 9,
		ChangeCountTargetForSpell = 10,
		ChangeProbabilityCriticalAttack = 11,
		ChangeEfficiencyHeal = 12,
		ChangeDodge = 13,
		ChangeAccuracy = 14,
		ChangeRepeatAttackOnOnlyOneTarget = 15,
		ChangeStamina = 16,
		ChangeTypeAttack = 17,
		ChangeMagicAttack = 18,
		ChangeCleanAttack = 19,
		ChangeElectricityAttack = 20,
		ChangeFieryAttack = 21,
		ChangeHolyAttack = 22,
		ChangeTypeSelectTargetForSimpleAttack = 23,
		ChangeMagicCritical = 24,
		ChangePosibleMagicCritical = 25

	}
	public enum EffectStatus{
		Stun = 0,
		Petrification = 1,
		Freezing = 2,
		Astral = 3,
		Silence = 4,
		Invulnerability = 5,
		Clear = 6
	}
	public enum EffectMark{
		Provocation = 1,
		Berserk = 2,
		Hellish = 3,
		RepeateAttackMark = 4
	}
	public enum EffectOther{
		CreateHero = 0,
		DeleteHero = 1
	}
	public enum EffectSpecial{
		Resurrection = 0
	}