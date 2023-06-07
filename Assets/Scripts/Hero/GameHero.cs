using Models;
using Models.Heroes;
using Models.Heroes.Characteristics;
using Models.Heroes.Skills;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class GameHero : BaseModel
{
    [Header("Current")]
    public GeneralInfoHero generalInfo;
    public FightCharacteristics characts;
    public ResistanceModel resistances;
    public List<Skill> skills = new List<Skill>();

    private int currentBreakthrough;

    public BaseCharacteristicModel GetBaseCharacteristic => characts.baseCharacteristic;
    public GameObject PrefabArrow { get; private set; }

    public GameHero(HeroModel hero)
    {
        SetHero(hero);
    }

    public void SetHero(HeroModel hero)
    {
        this.generalInfo = (GeneralInfoHero)hero.General.Clone();
        this.PrefabArrow = hero.PrefabArrow;
        this.resistances = (ResistanceModel)hero.Resistances.Clone();
        PrepareCharacts(hero);
        this.skills = hero.skills;
        if (hero.Evolutions != null)
        {
            currentBreakthrough = hero.Evolutions.currentBreakthrough;
        }
        else
        {
            Debug.Log("hero.Evolutions not exist, id-hero: " + hero.General.ViewId.ToString());
            currentBreakthrough = 0;
        }
    }
    private void PrepareCharacts(HeroModel hero)
    {
        this.characts = new FightCharacteristics(hero.Characts.Clone());
        characts.Damage = hero.GetCharacteristic(TypeCharacteristic.Damage);
        this.characts.GeneralArmor = (int)hero.GetCharacteristic(TypeCharacteristic.Defense);
        this.characts.GeneralAttack = (int)hero.GetCharacteristic(TypeCharacteristic.Attack);
        this.characts.Initiative = hero.GetCharacteristic(TypeCharacteristic.Initiative);
        this.characts.HP = Mathf.Round(hero.GetCharacteristic(TypeCharacteristic.HP));
        this.characts.MaxHP = this.characts.HP;
    }
    public void PrepareSkills(HeroController master)
    {
        foreach (Skill skill in skills)
        {
            skill.CreateSkill(master, currentBreakthrough);
        }
    }
    public float MaxHP { get => this.characts.MaxHP; set => this.characts.MaxHP = value; }
}
[System.Serializable]
public class FightCharacteristics : HeroCharacteristics
{
    public int GeneralAttack = 0, GeneralArmor = 0;
    public float MaxHP;
    public FightCharacteristics(HeroCharacteristics heroCharacts)
    {
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