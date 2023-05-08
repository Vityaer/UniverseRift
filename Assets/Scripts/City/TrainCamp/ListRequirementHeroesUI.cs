using System.Collections.Generic;
using UnityEngine;

public class ListRequirementHeroesUI : MonoBehaviour
{
    public LevelUpRatingHero mainController;

    [SerializeField] private List<RequireCard> requireCards = new List<RequireCard>();
    private List<RequirementHero> requirementHeroes = new List<RequirementHero>();

    public void SetData(List<RequirementHero> requirementHeroes)
    {
        this.requirementHeroes = requirementHeroes;
        for (int i = 0; i < requireCards.Count; i++)
        {
            if (i < requirementHeroes.Count)
            {
                requireCards[i].SetData(requirementHeroes[i]);
            }
            else
            {
                requireCards[i].Hide();
            }
        }
    }

    public bool GetCanLevelUpRating()
    {
        bool result = false;
        return result;
    }

    public void HeroSelectDiselect()
    {
        mainController.CheckHeroes();
    }

    public bool IsAllDone()
    {
        bool result = true;
        for (int i = 0; i < requirementHeroes.Count; i++)
        {
            if (requireCards[i].CheckHeroes())
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public void ClearData()
    {
        foreach (RequireCard requireCard in requireCards)
        {
            requireCard.ClearData();
        }
    }
    public void DeleteSelectedHeroes()
    {
        for (int i = 0; i < requirementHeroes.Count; i++)
        {
            requireCards[i].DeleteSelectedHeroes();
        }
    }
}