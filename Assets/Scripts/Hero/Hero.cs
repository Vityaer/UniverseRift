using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public partial class Hero{
	[Header("Current")]
	public GeneralInfoHero generalInfo;

	[SerializeField]
	private GameObject prefabArrow;
	public  GameObject PrefabArrow{get => prefabArrow;}


	public Characteristics characts;
	public Resistance resistances = new Resistance();  

	public int MaxHP;

	public List<Skill> skills = new List<Skill>();

	private int currentBreakthrough;

	public BaseCharacteristic GetBaseCharacteristic{get => characts.baseCharacteristic;}
	public Hero(InfoHero hero){
		SetHero(hero);
	}
	public void SetHero(InfoHero hero){
		this.generalInfo    = (GeneralInfoHero)hero.generalInfo.Clone(); 
		this.prefabArrow    = hero.PrefabArrow;
		this.characts       = (Characteristics)hero.characts.Clone();
		this.resistances    = (Resistance)hero.resistances.Clone();
		characts.Damage     = (int) Mathf.Floor(hero.GetCharacteristic(TypeCharacteristic.Damage) );
		characts.GeneralArmor      = (int) Mathf.Floor(hero.GetCharacteristic(TypeCharacteristic.Defense) );
		characts.GeneralAttack      = (int) Mathf.Floor(hero.GetCharacteristic(TypeCharacteristic.Attack) );
		characts.Initiative = (int) Mathf.Floor(hero.GetCharacteristic(TypeCharacteristic.Initiative) );
		characts.HP         = (int) Mathf.Floor(hero.GetCharacteristic(TypeCharacteristic.HP) );
		this.MaxHP          = this.characts.HP;
		this.skills         = hero.skills;
		if(hero.Evolutions != null){
			currentBreakthrough = hero.Evolutions.currentBreakthrough;
		}else{
			Debug.Log("hero.Evolutions not exist, id-hero: " + hero.generalInfo.idHero.ToString());
			currentBreakthrough = 0;
		}
	}
	public void PrepareSkills(HeroControllerScript master){
		foreach (Skill skill in skills){
			skill.CreateSkill(master, currentBreakthrough);
		}
	}
}