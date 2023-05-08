using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public partial class Hero{
	[Header("Current")]
	public GeneralInfoHero generalInfo;

	[SerializeField] private GameObject prefabArrow;
	public  GameObject PrefabArrow{get => prefabArrow;}


	public FightCharacteristics characts;
	public Resistance resistances = new Resistance();  


	public List<Skill> skills = new List<Skill>();

	private int currentBreakthrough;

	public BaseCharacteristic GetBaseCharacteristic{get => characts.baseCharacteristic;}
	public Hero(InfoHero hero){
		SetHero(hero);
	}
	public void SetHero(InfoHero hero){
		this.generalInfo    = (GeneralInfoHero)hero.generalInfo.Clone(); 
		this.prefabArrow    = hero.PrefabArrow;
		this.resistances    = (Resistance)hero.resistances.Clone();
		PrepareCharacts(hero);
		this.skills         = hero.skills;
		if(hero.Evolutions != null){
			currentBreakthrough = hero.Evolutions.currentBreakthrough;
		}else{
			Debug.Log("hero.Evolutions not exist, id-hero: " + hero.generalInfo.ViewId.ToString());
			currentBreakthrough = 0;
		}
	}
	private void PrepareCharacts(InfoHero hero){
		this.characts       = new FightCharacteristics(hero.characts.Clone());
		characts.Damage     = hero.GetCharacteristic(TypeCharacteristic.Damage);
		this.characts.GeneralArmor  = (int) hero.GetCharacteristic(TypeCharacteristic.Defense);
		this.characts.GeneralAttack = (int) hero.GetCharacteristic(TypeCharacteristic.Attack);
		this.characts.Initiative    = hero.GetCharacteristic(TypeCharacteristic.Initiative);
		this.characts.HP            = Mathf.Round(hero.GetCharacteristic(TypeCharacteristic.HP));
		this.characts.MaxHP         = this.characts.HP;
	}
	public void PrepareSkills(HeroController master){
		foreach (Skill skill in skills){
			skill.CreateSkill(master, currentBreakthrough);
		}
	}
	public float MaxHP{get => this.characts.MaxHP; set => this.characts.MaxHP = value;}
}
[System.Serializable]
public class FightCharacteristics : Characteristics{
	public int GeneralAttack = 0, GeneralArmor = 0;
	public float MaxHP;
	public FightCharacteristics(Characteristics heroCharacts){
		this.baseCharacteristic = heroCharacts.baseCharacteristic;
		this.limitLevel = heroCharacts.limitLevel;
		this.ProbabilityCriticalAttack = heroCharacts.ProbabilityCriticalAttack;
	 	this.DamageCriticalAttack = heroCharacts.DamageCriticalAttack;
	 	this.Accuracy = heroCharacts.Accuracy;
	 	this.CleanDamage = heroCharacts.CleanDamage;
	 	this.Dodge = heroCharacts.Dodge;
	 	this.CountTargetForSimpleAttack = heroCharacts.CountTargetForSimpleAttack;
	 	this.CountTargetForSpell = heroCharacts.CountTargetForSpell;

	}
}