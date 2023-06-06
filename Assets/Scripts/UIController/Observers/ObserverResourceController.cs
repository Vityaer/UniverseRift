using City.Buildings.Market;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObserverResourceController : MonoBehaviour
{
    [Header("General")]
    public TypeResource typeResource;
    private bool isMyabeBuy;
    public int cost;
    private Resource resource;

    [Header("UI")]
    public GameObject btnAddResource;
    public Image imageResource;
    public TextMeshProUGUI countResource;

    void Start()
    {
        isMyabeBuy = MarketResourceController.Instance.GetCanSellThisResource(typeResource);
        resource = new Resource(typeResource);
        imageResource.sprite = resource.Image;
        btnAddResource.SetActive(isMyabeBuy);
        GameController.Instance.RegisterOnChangeResource(UpdateUI, typeResource);
        UpdateUI(GameController.Instance.GetResource(typeResource));
    }

    public void UpdateUI(Resource res)
    {
        resource = res;
        countResource.text = resource.ToString();
    }

    public void OpenPanelForBuyResource()
    {
        MarketProduct<Resource> product = null;
        product = MarketResourceController.Instance.GetProductFromTypeResource(resource.Name);
        if (product != null)
            PanelBuyResource.StandartPanelBuyResource.Open(
                product.subject, product.Cost
                );
    }
}
