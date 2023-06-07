using System.Collections.Generic;
using UnityEngine;

public class ListMyHeroesController : MonoBehaviour
{
    [SerializeField] private ListCardOnWarTable listHeroesController;
    protected List<HeroModel> listHeroes = new List<HeroModel>();

    [Header("UI")]
    private Canvas canvas;
    public GameObject background;
    public FooterButton btnOpenClose;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        btnOpenClose.RegisterOnChange(Change);
    }

    void Change(bool isOpen)
    {
        if (isOpen) { Open(); } else { Close(); }
    }

    void LoadListHeroes()
    {
        listHeroes = GameController.Instance.GetListHeroes;
        listHeroesController.SetList(listHeroes);
    }

    public void Open()
    {
        LoadListHeroes();
        canvas.enabled = true;
        listHeroesController.EventOpen();
        BackgroundController.Instance.OpenBackground(background);
    }

    public void Close()
    {
        canvas.enabled = false;
        listHeroesController.EventClose();
    }
}
