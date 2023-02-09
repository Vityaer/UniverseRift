using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
[CreateAssetMenu(fileName = "New InfoHero", menuName = "Custom ScriptableObject/Info hero", order = 51)]
[System.Serializable]
public class InfoHero : ScriptableObject, ICloneable
{
	[Header("General")]
	public GeneralInfoHero generalInfo;
	[SerializeField]
	private GameObject prefabArrow;
	public  GameObject PrefabArrow{get => prefabArrow; set => prefabArrow = value;}

	[Header("Сharacteristics")]
	public Characteristics characts;

	public float GetStrength{get => (Mathf.Round( GetCharacteristic(TypeCharacteristic.Damage) + GetCharacteristic(TypeCharacteristic.HP)/3f ) );}

	[Header("Resistance")]
	public Resistance resistances;  

	[Header("Increase Сharacteristics")]
	[SerializeField]
	private IncreaseCharacteristics incCharacts;
	public IncreaseCharacteristics IncCharacts{get => incCharacts; set => incCharacts = value;}

	[HideInInspector]
	public CostumeHeroControllerScript CostumeHero;
	[Header("Skills")]
	public List<Skill> skills = new List<Skill>();
	
	[Header("Breakthroughs")]
	public BreakthroughHero Evolutions;

	public HeroLocalization localization = null;

	public Sprite GetMainImage{get => generalInfo.ImageHero;}

	private void CopyData(InfoHero Data){
		this.generalInfo     = (GeneralInfoHero)Data.generalInfo.Clone();
		this.incCharacts     = (IncreaseCharacteristics)Data.IncCharacts.Clone();
		this.characts        = (Characteristics) Data.characts.Clone();
		this.resistances     = (Resistance) Data.resistances.Clone();
		this.generalInfo.Prefab = Resources.Load<GameObject>( string.Concat("Heroes/", this.generalInfo.idHero.ToString()) ); 
		this.prefabArrow = Data.PrefabArrow; 
		this.skills     = Data.skills;
		this.Evolutions = Data.Evolutions;
		this.CostumeHero = new CostumeHeroControllerScript();
		GetSkills();
		PrepareLocalization();
	}
	public InfoHero(HeroSave heroSave){
		CopyData( TavernScript.Instance.GetListHeroes.Find(x => (x.generalInfo.idHero == heroSave.ID)) );
		generalInfo.Name = heroSave.name;
		LevelUP(heroSave.level - 1);
		this.CostumeHero = new CostumeHeroControllerScript();
		this.CostumeHero.SetData(heroSave.costume.listID);
	}
	public InfoHero(){}

//API
	public void LevelUP(int count = 1){
		for(int i = 0; i < count; i++){
			if(this.generalInfo.Level < Evolutions.LimitLevel){
				this.generalInfo.Level += 1;
				Growth.GrowHero(this.characts, this.resistances, this.IncCharacts);
				if(Evolutions.OnLevelUp(this.generalInfo.Level)) {
					Growth.GrowHero(this.characts, this.resistances, Evolutions.GetGrowth());
					Evolutions.ChangeData(this.generalInfo); 
				}
			}
		}
	}

	public float GetCharacteristic(TypeCharacteristic typeBonus){
		float result = 0;
		switch (typeBonus){
			case TypeCharacteristic.HP:
				result += characts.HP;
				break;
			case TypeCharacteristic.Damage:
				result += characts.Damage;
				break;
			case TypeCharacteristic.Initiative:
				result += characts.Initiative;
				break;
			case TypeCharacteristic.Defense:
				result += characts.baseCharacteristic.Defense;
				break;  	
		}
		result += CostumeHero.GetBonus(typeBonus);
		return result;
	}

	public void PrepareHeroWithLevel(int level){
		this.generalInfo.Level = level;
		Growth.GrowHero(this.characts, this.resistances, this.IncCharacts, generalInfo.Level);
	}

	private void GetSkills(){
		foreach(Skill skill in skills){
			skill.GetSkill(Evolutions.currentBreakthrough);
		}
	}

	public void PrepareLocalization(){
		localization = LanguageControllerScript.Instance.GetLocalizationHero(generalInfo.idHero);
	}

	public void PrepareSkillLocalization(){
		if(localization == null) PrepareLocalization();
		if(localization != null)
		{
			foreach(Skill skill in skills)
				skill.GetInfoAboutSkill(localization);
		}else
		{
			Debug.LogError("localization not found");
		}
	}

	public bool CheckСonformity(RequirementHero requirementHero){
		bool result = false;
		if((generalInfo.ratingHero == requirementHero.rating) && (generalInfo.race == requirementHero.race)){
			result  = true;
		}
		return result;
	}

	public void UpRating(){
		generalInfo.ratingHero += 1;
	}

	public object Clone(){
		return new InfoHero{
			generalInfo = (GeneralInfoHero) this.generalInfo.Clone(),
			characts = (Characteristics)this.characts.Clone(),
			IncCharacts =(IncreaseCharacteristics) this.IncCharacts.Clone(),
			prefabArrow = this.prefabArrow,
			resistances = (Resistance) this.resistances.Clone(),
			CostumeHero  = this.CostumeHero.Clone(),
			skills = this.skills,
			Evolutions = this.Evolutions,
			localization = this.localization
		};
	}
}
