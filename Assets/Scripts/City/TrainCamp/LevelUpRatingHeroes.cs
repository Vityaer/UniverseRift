using UIController.ItemVisual;
using Models.Heroes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUPRaitingHeroes", menuName = "Custom ScriptableObject/LevelUpRatingHeroes", order = 61)]
[System.Serializable]
public class LevelUpRatingHeroes : ScriptableObject
{
    [Header("Ratings")]
    public List<LevelUpRaiting> ratings = new List<LevelUpRaiting>();

    public LevelUpRaiting GetRequirements(HeroModel hero)
    {
        int currentRating = hero.General.RatingHero;
        LevelUpRaiting result = ratings.Find(x => x.level == (currentRating + 1));
        if (result != null)
        {
            result.UpdateData(hero);
        }
        else
        {
            Debug.Log("not found LevelUpRaiting");
        }
        return result;
    }
}

[System.Serializable]
public class LevelUpRaiting
{
    public string Name;
    public int level;
    [Header("Requirements")]
    [SerializeField] private ListResource list;
    public List<RequirementHero> requirementHeroes = new List<RequirementHero>();
    public ListResource Cost => list;


    public void UpdateData(HeroModel hero)
    {
        requirementHeroes.ForEach(x => x.UpdateData(hero));
    }
}

[System.Serializable]
public class RequirementHero
{
    public string ID;
    public int rating, count;
    public RequireRaceUpRating requireRace;
    public string race;
    private HeroModel dataHero;

    public HeroModel GetData => dataHero;

    public void UpdateData(HeroModel hero)
    {
        var IDHero = hero.General.ViewId;
        race = hero.General.Race;
        dataHero = new HeroModel();
        if (ID.Equals(string.Empty))
        {
            ID = IDHero;
            dataHero.General = (GeneralInfoHero)hero.General.Clone();
            dataHero.General.Level = 1;
        }
        else
        {
            dataHero.General = new GeneralInfoHero();
            dataHero.General.RatingHero = rating;
            dataHero.General.Race = race;
            SpriteName spriteName = SpriteName.OneStarHero;
            switch (rating)
            {
                case 1:
                    spriteName = SpriteName.OneStarHero;
                    break;
                case 2:
                    spriteName = SpriteName.TwoStarHero;
                    break;
                case 3:
                    spriteName = SpriteName.ThreeStarHero;
                    break;
                case 4:
                    spriteName = SpriteName.FourStartHero;
                    break;
                case 5:
                    spriteName = SpriteName.FiveStarHero;
                    break;
            }
            if (SystemSprites.Instance == null) Debug.Log("SystemSprites.Instance null");
            if (dataHero == null) Debug.Log("dataHero null");
            if (dataHero.General == null) Debug.Log("dataHero.generalInfo null");
            dataHero.General.ImageHero = SystemSprites.Instance.GetSprite(spriteName);
        }
    }
}

public enum RequireRaceUpRating
{
    Equal,
    Friend,
    Enemy,
    God,
    Any
}