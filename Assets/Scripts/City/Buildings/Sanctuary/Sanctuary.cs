using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sanctuary : BuildingWithHeroesList
{
    public Resource costReplacementFourRating, costReplacementFiveRating;
    public Tavern tavernController;
    private Card selectedHero;
    public Button btnSave, btnReplacement;
    public InfoHero newHero;


    private List<InfoHero> heroes = new List<InfoHero>();
    public void ReplacementHero()
    {
        Resource resCost = null;
        if (selectedHero != null)
        {
            if (selectedHero.hero.generalInfo.RatingHero == 4)
            {
                resCost = costReplacementFourRating;
                heroes = tavernController.GetListHeroes.FindAll(x => (x.generalInfo.RatingHero == 4));
            }
            else
            {
                resCost = costReplacementFiveRating;
                heroes = tavernController.GetListHeroes.FindAll(x => (x.generalInfo.RatingHero == 5));
            }

            if (GameController.Instance.CheckResource(resCost))
            {
                heroes = heroes.FindAll(x => ((x.generalInfo.RatingHero == selectedHero.hero.generalInfo.RatingHero) && (x.generalInfo.Race == selectedHero.hero.generalInfo.Race) && (x.generalInfo.ViewId != selectedHero.hero.generalInfo.ViewId)));
                if (heroes.Count > 0)
                {
                    newHero = (InfoHero)heroes[Random.Range(0, heroes.Count)].Clone();
                    btnSave.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SaveReplacement()
    {
        if (newHero != null)
        {
            listHeroesController.RemoveCards(new List<Card> { selectedHero });
            GameController.Instance.RemoveHero(selectedHero.hero);
            GameController.Instance.AddHero(newHero);
            btnSave.gameObject.SetActive(false);
        }
    }
    public override void SelectHero(Card newCardHero)
    {
        if (selectedHero != null) selectedHero.Unselect();
        selectedHero = newCardHero;
        selectedHero.Select();
        btnReplacement.interactable = true;
    }

    protected override void OpenPage()
    {
        listHeroes = GameController.Instance.GetListHeroes;
        LoadListHeroes();
        listHeroesController.EventOpen();
        listHeroesController.RegisterOnSelect(SelectHero);
    }
    protected override void ClosePage()
    {
        btnSave.gameObject.SetActive(false);
        selectedHero = null;
        listHeroesController.UnRegisterOnSelect(SelectHero);
        listHeroesController.EventClose();
    }

}
