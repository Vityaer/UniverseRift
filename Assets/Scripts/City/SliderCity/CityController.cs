using UnityEngine;

public class CityController : MainPage
{
    public CityScroller sliderCity;

    [Header("UI")]
    private Canvas canvasCity;
    public GameObject background, cityParent;

    [Header("UI Button")]
    [SerializeField] GameObject canvasButtonsUI;

    protected override void Awake()
    {
        canvasCity = GetComponent<Canvas>();
        base.Awake();
    }

    public override void Open()
    {
        base.Open();
        sliderCity.enabled = true;
        canvasButtonsUI.SetActive(true);
        cityParent.SetActive(true);
        BackgroundController.Instance.OpenBackground(background);
    }

    public override void Close()
    {
        sliderCity.enabled = false;
        cityParent.SetActive(false);
        canvasButtonsUI.SetActive(false);
    }
}
