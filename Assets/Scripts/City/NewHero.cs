using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHero : MonoBehaviour
{
    public List<InfoHero> listNewHero = new List<InfoHero>();

    public Color colorNotInteractable;
    public Color colorInteractable;
    public Resource DiamondCost;

    private Button btnBuy;
    private Image sprite;

    void Awake()
    {
        btnBuy = GetComponent<Button>();
        sprite = GetComponent<Image>();
    }

    void Start()
    {
        CheckResourceForBuyHero();
    }

    public void GetNewHero()
    {
        GameController.Instance.SubtractResource(DiamondCost);

        InfoHero hero = (InfoHero)(listNewHero[Random.Range(0, listNewHero.Count)].Clone());
        hero.generalInfo.Name = hero.generalInfo.Name + " №" + Random.Range(0, 1000).ToString();
        GetNewHero(hero);
        MessageController.Instance.AddMessage("Новый герой! Это - " + hero.generalInfo.Name);
        CheckResourceForBuyHero();
    }
    public void GetNewHero(InfoHero newHero)
    {
        GameController.Instance.AddHero(newHero);
    }

    private void CheckResourceForBuyHero()
    {
        bool result = GameController.Instance.CheckResource(DiamondCost);
        btnBuy.interactable = result;
        sprite.color = (result) ? colorInteractable : colorNotInteractable;
    }
}
