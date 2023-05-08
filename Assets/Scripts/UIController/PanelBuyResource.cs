using UnityEngine;

public class PanelBuyResource : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public SubjectCellControllerScript cellProduct;
    public CountController countController;
    public ButtonWithObserverResource buttonBuy;
    public bool standartPanel = true;
    private Resource product, cost;
    private static PanelBuyResource standartPanelBuyResource;
    public static PanelBuyResource StandartPanelBuyResource { get => standartPanelBuyResource; }

    void Awake()
    {
        if (standartPanel) standartPanelBuyResource = this;
    }

    void Start()
    {
        countController.RegisterOnChangeCount(ChangeCount);
    }

    public void Open(Resource product, Resource cost)
    {
        this.cost = cost;
        this.product = product;
        ChangeCount(count: countController.MinCount);
        cellProduct.SetItem(product);
        panel.SetActive(true);
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void ChangeCount(int count)
    {
        buttonBuy.ChangeCost(cost * count);
    }

    public void Buy()
    {
        int count = countController.Count;
        if (GameController.Instance.CheckResource(cost * count))
        {
            GameController.Instance.SubtractResource(cost * count);
            GameController.Instance.AddResource(product * count);
        }
    }

}