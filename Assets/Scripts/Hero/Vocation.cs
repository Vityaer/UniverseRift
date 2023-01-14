using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Vocation{
	Warrior,
	Wizard,
	Archer,
	Pastor,
	Slayer,
	Tank,
	Support
}

internal class Growth{
	public static void GrowHero(Characteristics characts, Resistance resistance,IncreaseCharacteristics increaseCharacts, int level = 1){
		GrowthCharact(ref characts.HP                       , increaseCharacts.increaseHP, level );
		GrowthCharact(ref characts.Damage                   , increaseCharacts.increaseDamage, level );
		GrowthCharact(ref characts.Accuracy                 , increaseCharacts.increaseAccuracy, level );
		GrowthCharact(ref characts.Initiative               , increaseCharacts.increaseInitiative, level );
		GrowthCharact(ref characts.CleanDamage              , increaseCharacts.increaseCleanDamage, level );
		GrowthCharact(ref characts.DamageCriticalAttack     , increaseCharacts.increaseDamageCriticalAttack, level );
		GrowthCharact(ref characts.ProbabilityCriticalAttack, increaseCharacts.increaseProbabilityCriticalAttack, level );
		GrowthCharact(ref characts.Dodge                    , increaseCharacts.increaseDodge, level );
		
		GrowthCharact(ref resistance.CritResistance         , increaseCharacts.increaseCritResistance, level );
		GrowthCharact(ref resistance.MagicResistance        , increaseCharacts.increaseMagicResistance, level );
	}
	private static void GrowthCharact(ref int charact, float increase, int level){
		for(int i = 0; i < level; i++)
			charact = (int)   Mathf.Ceil(charact * (1 + increase / 100f)   );
	}
	private static void GrowthCharact(ref float charact, float increase, int level){
		for(int i = 0; i < level; i++){
			charact = charact * ( 1 + increase / 100f);
			charact = (float) Math.Round(charact, 3);
		}
	}
}

[System.Serializable]
public class IncreaseCharacteristics : ICloneable{
	public float increaseDamage;
	public float increaseHP;
	public float increaseInitiative;
	public float increaseProbabilityCriticalAttack;
	public float increaseDamageCriticalAttack;
	public float increaseDodge;
	public float increaseAccuracy;
	public float increaseCleanDamage;

	[Header("Increace resistance")]
	public float increaseMagicResistance;
	public float increaseCritResistance;
	public float increasePoisonResistance;

	public object Clone(){
        return new IncreaseCharacteristics  { 	increaseDamage = this.increaseDamage,
        							 	increaseHP = this.increaseHP,
        							 	increaseInitiative  = this.increaseInitiative,
        							 	increaseProbabilityCriticalAttack = this.increaseProbabilityCriticalAttack,
        							 	increaseDamageCriticalAttack = this.increaseDamageCriticalAttack,
        							 	increaseDodge = this.increaseDodge,
        							 	increaseAccuracy = this.increaseAccuracy,
        							 	increaseCleanDamage = this.increaseCleanDamage,
        							 	increaseMagicResistance = this.increaseMagicResistance,
        							 	increaseCritResistance = this.increaseCritResistance,
        							 	increasePoisonResistance = this.increasePoisonResistance
        							};				
    }
}

[System.Serializable]
public class Characteristics{
	public int   limitLevel;
	public float Damage;
	public float HP;
	public float Initiative;
	public float ProbabilityCriticalAttack;
	public float DamageCriticalAttack;
	public float Accuracy;
	public float CleanDamage;
	public float Dodge;
	public int CountTargetForSimpleAttack = 1;
	public int CountTargetForSpell = 1;
	public BaseCharacteristic baseCharacteristic;


	//API
	public Characteristics Clone(){
        return new Characteristics  { 	limitLevel = this.limitLevel,
        							 	Damage = this.Damage,
        							 	HP     = this.HP,
        							 	Initiative = this.Initiative,
        							 	ProbabilityCriticalAttack = this.ProbabilityCriticalAttack,
        							 	DamageCriticalAttack = this.DamageCriticalAttack,
        							 	Accuracy = this.Accuracy,
        							 	CleanDamage = this.CleanDamage,
        							 	Dodge = this.Dodge,
        							 	CountTargetForSimpleAttack = this.CountTargetForSimpleAttack,
        							 	CountTargetForSpell = this.CountTargetForSpell,
        							 	baseCharacteristic = this.baseCharacteristic
        							};				
    }
}
[System.Serializable]
public class Resistance : ICloneable{
	public float MagicResistance;
	public float CritResistance;
	public float PoisonResistance;
	public float StunResistance;
	public float PetrificationResistance;
	public float FreezingResistance;
	public float AstralResistance;
	public float DumbResistance;
	public float silinceResistance;
	public float EfficiencyHeal = 1f;
	public object Clone(){
        return new Resistance  { 	MagicResistance = this.MagicResistance,
        							 	CritResistance = this.CritResistance,
        							 	PoisonResistance     = this.PoisonResistance,
        							 	EfficiencyHeal  = this.EfficiencyHeal,
        							 	StunResistance = this.StunResistance,
        							 	PetrificationResistance = this.PetrificationResistance,
        							 	FreezingResistance = this.FreezingResistance,
        							 	AstralResistance = this.AstralResistance,
        							 	silinceResistance = this.silinceResistance,
        							 	DumbResistance = this.DumbResistance
        							};				
    }

}


[System.Serializable]
public class GeneralInfoHero{
	public  string Name;
	public  Race race;
	public  Vocation ClassHero;
	public  int ratingHero;
	public  Rare rare;
	public  int idHero;
	private Sprite _image;
	public  Sprite ImageHero{get{ if(_image == null) {_image = Prefab?.GetComponent<HeroControllerScript>().GetSprite;} return _image;} set{_image = value;}}
	public  int Level;
	public  bool isAlive = true;
	private GameObject prefab;
	public  GameObject Prefab{get {if(IDPrefab == 0) IDPrefab = idHero; if (prefab == null) prefab = Resources.Load<GameObject>( string.Concat("Heroes/", this.IDPrefab.ToString()) ); return prefab;} set => prefab = value;}
	[HideInInspector]public int IDCreate = 0;
	[SerializeField] private int IDPrefab = 0;
	
	public object Clone(){
        return new GeneralInfoHero  { 	Name = this.Name,
        							 	race = this.race,
        							 	ClassHero     = this.ClassHero,
        							 	ratingHero  = this.ratingHero,
        							 	rare = this.rare,
        							 	idHero = this.idHero,
        							 	Level = this.Level,
        							 	isAlive = this.isAlive,
        							 	IDCreate = this.IDCreate,
        							 	IDPrefab = this.IDPrefab
        							};				
    }

}
[System.Serializable]
public class BaseCharacteristic{
	public int Attack;
	public int Defense;
	public int Speed; 
	public TypeMovement typeMovement; 
	public  bool Mellee;
	public TypeStrike typeStrike;
	public bool CanRetaliation = true;
	public int CountCouterAttack = 1;
	public BaseCharacteristic Clone(){
        return new BaseCharacteristic  { Attack = this.Attack,
        							 	Defense = this.Defense,
        							 	Speed     = this.Speed,
        							 	typeMovement  = this.typeMovement,
        							 	Mellee = this.Mellee,
        							 	typeStrike = this.typeStrike,
        							 	CanRetaliation = this.CanRetaliation,
        							 	CountCouterAttack = this.CountCouterAttack
        							};				
    }
}
public enum TypeMovement{
	Ground,
	Fly
}
public enum TypeCharacteristic{
	Damage,
	HP,
	Defense,
	Initiative,
	Attack,
	ProbabilityCriticalAttack,
	DamageCriticalAttack,
	Accuracy,
	CleanDamage,
	Dodge,
	MagicResistance,
	CritResistance,
	PoisonResistance,
	StunResistance,
	PetrificationResistance,
	FreezingResistance,
	AstralResistance,
	DumbResistance,
	silinceResistance,
	EfficiencyHeal,
	Speed,
	CountTargetForSimpleAttack,
	CountTargetForSpell
}
public interface ICloneable{
    object Clone();
}