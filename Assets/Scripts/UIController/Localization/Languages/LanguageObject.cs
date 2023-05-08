using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLanguage", menuName = "New Language")]
public class LanguageObject : ScriptableObject
{
    public int IDLanguage;
    public GeneralDefinition generalInfo;
    public List<HeroLocalization> listHeroesLocalitation = new List<HeroLocalization>();
    public List<BuildingLocalization> listBuildingLocalitation = new List<BuildingLocalization>();
    public List<MessageLocalization> listMessageLocalization = new List<MessageLocalization>();

    public HeroLocalization GetLocalizationHero(string ID)
    {
        HeroLocalization result = null;
        foreach (HeroLocalization hero in listHeroesLocalitation)
        {
            if (hero.HeroId == ID)
            {
                result = hero;
                break;
            }
        }
        return result;
    }

    public BuildingLocalization GetLocalizationBuildind(int ID)
    {
        BuildingLocalization result = null;
        return result;
    }

    public MessageLocalization GetLocalizationMessage(int ID)
    {
        MessageLocalization result = null;
        return result;
    }

}

[System.Serializable]
public class HeroLocalization
{
    public string Name;
    public string HeroId;

    public string Description;
    public List<SkillLocalization> Skills = new List<SkillLocalization>();

    public SkillLevelLocalization GetDescriptionSkill(string ID, int numSkill)
    {
        SkillLevelLocalization result = null;
        if (Skills != null)
        {
            return null;
        }

        if (Skills.Count == 0)
        {
            Debug.LogError("Skills.Count == 0");
            return null;
        }

        if (Skills.Count > numSkill && Skills[numSkill].levels.Count > 0)
        {
            result = Skills[numSkill]?.levels[0];
        }
        else
        {
            Debug.LogError("Skills[numSkill].levels.Count == 0");
        }

        return result;
    }
}

[System.Serializable]
public class SkillLocalization
{
    public string ID;
    public List<SkillLevelLocalization> levels = new List<SkillLevelLocalization>();
}

[System.Serializable]
public class SkillLevelLocalization
{
    public string name;
    public string description;
    public Sprite image;
}

[System.Serializable]
public class GeneralDefinition
{
    public string settings;
    public string avatar;
    public string goFight;
    public string skip;
}

[System.Serializable]
public class BuildingLocalization
{
    public string name;
    public string description;
}

[System.Serializable]
public class MessageLocalization
{
    public string name;
    public string containText;
}