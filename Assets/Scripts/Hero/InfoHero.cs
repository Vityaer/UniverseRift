using ObjectSave;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InfoHero", menuName = "Custom ScriptableObject/Info hero", order = 51)]
[System.Serializable]
public class InfoHero : ICloneable
{
    [Header("General")]
    public GeneralInfoHero generalInfo;
    [SerializeField]
    private GameObject prefabArrow;
    public GameObject PrefabArrow { get => prefabArrow; set => prefabArrow = value; }

    [Header("Сharacteristics")]
    public Characteristics characts;

    public float GetStrength { get => (Mathf.Round(GetCharacteristic(TypeCharacteristic.Damage) + GetCharacteristic(TypeCharacteristic.HP) / 3f)); }

    [Header("Resistance")]
    public Resistance resistances;

    [Header("Increase Сharacteristics")]
    [SerializeField]
    private IncreaseCharacteristics incCharacts;
    public IncreaseCharacteristics IncCharacts { get => incCharacts; set => incCharacts = value; }

    [HideInInspector]
    public CostumeHeroController CostumeHero;
    [Header("Skills")]
    public List<Skill> skills = new List<Skill>();

    [Header("Breakthroughs")]
    public BreakthroughHero Evolutions;

    public HeroLocalization localization = null;

    public Sprite GetMainImage => generalInfo.ImageHero;


    public InfoHero()
    {
    }

    public InfoHero(HeroSave heroSave)
    {
        var hero = Tavern.Instance.GetListHeroes.Find(x => x.generalInfo.HeroId == heroSave.HeroId);
        CopyData(hero);
        generalInfo.HeroId = heroSave.HeroId;
        generalInfo.ViewId = heroSave.HeroId;
        generalInfo.Name = heroSave.Name;
        generalInfo.RatingHero = heroSave.Rating;
        LevelUP(heroSave.Level - 1);
        CostumeHero = new CostumeHeroController();
        CostumeHero.SetData(heroSave.Costume.ItemIds);
        Preparation();
    }

    private void CopyData(InfoHero Data)
    {
        generalInfo = (GeneralInfoHero) Data.generalInfo.Clone();
        incCharacts = (IncreaseCharacteristics) Data.IncCharacts.Clone();
        characts = Data.characts.Clone();
        resistances = (Resistance) Data.resistances.Clone();
        generalInfo.Prefab = Resources.Load<GameObject>(string.Concat("Heroes/", generalInfo.ViewId.ToString()));
        prefabArrow = Data.PrefabArrow;
        skills = Data.skills;
        Evolutions = Data.Evolutions;
        CostumeHero = new CostumeHeroController();

    }

    public void Preparation()
    {
        GetSkills();
        PrepareLocalization();
    }

    //API
    public void LevelUP(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            if (generalInfo.Level < Evolutions.LimitLevel)
            {
                generalInfo.Level += 1;
                Growth.GrowHero(characts, resistances, IncCharacts);
            }
        }
    }

    public float GetCharacteristic(TypeCharacteristic typeBonus)
    {
        float result = 0;
        switch (typeBonus)
        {
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

    public void PrepareHeroWithLevel(int level)
    {
        generalInfo.Level = level;
        Growth.GrowHero(characts, resistances, IncCharacts, generalInfo.Level);
    }

    private void GetSkills()
    {
        foreach (Skill skill in skills)
        {
            skill.GetSkill(Evolutions.currentBreakthrough);
        }
    }

    public void PrepareLocalization()
    {
        localization = LanguageController.Instance.GetLocalizationHero(generalInfo.ViewId);
    }

    public void PrepareSkillLocalization()
    {
        if (localization == null)
            PrepareLocalization();

        if (localization != null)
        {
            foreach (Skill skill in skills)
                skill.GetInfoAboutSkill(localization);
        }
        else
        {
            //Debug.LogError("localization not found");
        }
    }

    public bool CheckСonformity(RequirementHero requirementHero)
    {
        bool result = false;

        if ((generalInfo.RatingHero == requirementHero.rating) && (generalInfo.Race == requirementHero.race))
        {
            result = true;
        }

        return result;
    }

    public void UpRating()
    {
        generalInfo.RatingHero += 1;
        Growth.GrowHero(characts, resistances, Evolutions.GetGrowth(generalInfo.RatingHero));
        Evolutions.ChangeData(generalInfo);
    }

    public object Clone()
    {
        var copyHero = new InfoHero
        {
            generalInfo = (GeneralInfoHero)generalInfo.Clone(),
            characts = characts.Clone(),
            IncCharacts = (IncreaseCharacteristics)IncCharacts.Clone(),
            prefabArrow = prefabArrow,
            resistances = (Resistance)resistances.Clone(),
            CostumeHero = CostumeHero.Clone(),
            skills = skills,
            Evolutions = Evolutions,
            localization = localization
        };
        copyHero.Preparation();
        return copyHero;
    }
}
