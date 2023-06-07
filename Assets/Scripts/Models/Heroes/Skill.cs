using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill{
	[SerializeField]
	private string name = "empty name";
	private int level = -1;
	public  string     ID;
	public  Sprite     image;
	[SerializeField]
	private string description = "empty description";
	public bool isActive = false;
	private List<Effect> effects = new List<Effect>();
	public List<SkillLevel> Skill_Level = new List<SkillLevel>();
	SkillLevelLocalization skillLocalization = null;

	public  string Name{get => skillLocalization != null? skillLocalization.name : name; set => name = value;}
	public string Description{get => skillLocalization != null? GetDescription(skillLocalization.description) : description;}
	public int Level{get => level;}
	
	public Skill()
	{
		image = null;
		isActive = false;
		effects = new List<Effect>();
	}

	public void GetSkill(int currentBreakthrough){
		for(int i = Skill_Level.Count - 1; i >= 0; i--){
			if(Skill_Level[i].requireNumBreakthrough <= currentBreakthrough){
				effects     = Skill_Level[ i ].effects;
				level = i;
				break;
			}
		}
	}

	public void GetSkillInfo(int currentBreakthrough)
	{
		GetSkill(currentBreakthrough);
		if(level == -1){
			effects = Skill_Level[ 0 ].effects;
			level = 0;
		}
	}

//API
	public void CreateSkill(HeroController master, int currentBreakthrough = 0)
	{
		GetSkill(currentBreakthrough);
		foreach (Effect effect in effects){
			effect.CreateEffect(master);
		}
		if(isActive){
			master.RegisterOnGetListForSpell(GetStartListForSpell);
		}
	}

	public void GetStartListForSpell(List<HeroController> listTarget)
	{
		if(effects.Count > 0)
			if(effects[0].listAction.Count > 0)
				effects[0].listAction[0].GetListForSpell(listTarget);
	}

//Info API	
	public void GetInfoAboutSkill(HeroLocalization localization)
	{
		skillLocalization = localization.GetDescriptionSkill(ID, level);
	}

	public string GetDescription(string description)
	{
		string strForReplace = ""; 
		if(effects.Count == 0) Debug.Log(string.Concat(Name, " not founed effects"));
		if(effects.Count == 1){
			for(int i=0; i < effects[0].listAction.Count; i++)
			{
				strForReplace = string.Concat("{Action", (i+1).ToString());
				description = description.Replace(string.Concat(strForReplace, ".Count}"), effects[0].listAction[i].countTarget.ToString());
				description = description.Replace(string.Concat(strForReplace, ".Amount}"), effects[0].listAction[i].amount.ToString());
				description = description.Replace(string.Concat(strForReplace, ".RoundCount}"), effects[0].listAction[i].rounds.Count.ToString());
			}
		}
		this.description = description;
		return description;
	}
}
