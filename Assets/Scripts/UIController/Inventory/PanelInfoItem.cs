using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfoItem : MonoBehaviour
{
    public static PanelInfoItem Instance { get; private set; }

    [Header("PanelInfo Item")]
    public GameObject panelInfoItem;
    public Image imageInfoItem;
    public TextMeshProUGUI textNameItem, textTypeItem, textGeneralInfo, textAddactiveInfo;
    private bool isOpenInfoPanel = false;
    private Item selectItem = null;

    [Header("Controllers")]
    public GameObject btnAction;
    public GameObject btnOpenInventory, btnDrop, btnClose;
    public TextMeshProUGUI textButtonAction, textButtonDrop;

    [Header("Panels")]
    public GameObject PanelCharacteristics;
    public GameObject PanelCostume;
    public GameObject PanelController;

    private Button componentButtonAction, componentButtonDrop, buttonSwapItems;
    private HeroItemCell cellItem;

    void Awake()
    {
        Instance = this;
        GetComponents();
    }

    void Start()
    {
        buttonSwapItems.onClick.AddListener(() => Close());
        buttonSwapItems.onClick.AddListener(() => InventoryController.Instance.Open(cellItem.typeCell, cellItem));
    }

    //Panel InfoAboutItem  
    public void OpenInfoAboutItem(Item item, HeroItemCell cellItem, bool onHero = false)
    {
        RemoveAllListenersOnButtons();
        componentButtonAction.onClick.AddListener(Close);
        componentButtonDrop.onClick.AddListener(Close);
        this.cellItem = cellItem;
        btnOpenInventory.SetActive(onHero);
        btnAction.SetActive(cellItem != null);

        PanelCharacteristics.SetActive(true);
        PanelCostume.SetActive(true);
        PanelController.SetActive(true);

        if (onHero == false)
        {
            componentButtonAction.onClick.AddListener(InventoryController.Instance.SelectItem);
            textButtonAction.text = "Снарядить";
            btnDrop.SetActive(false);
        }
        else
        {
            componentButtonDrop.onClick.AddListener(() => cellItem.SetItem(null));
            componentButtonDrop.onClick.AddListener(() => InventoryController.Instance.Close());
            textButtonDrop.text = "Снять";
            btnDrop.SetActive(true);
        }

        selectItem = item;
        UpdateUIInfo(item.Image, item.Id, type: item.Type.ToString(), generalInfo: item.GetTextBonuses());
        OpenPanel();
    }

    public void OpenInfoAboutItem(Resource res)
    {
        imageInfoItem.sprite = res.Image;
        textNameItem.text = res.Name.ToString();
        panelInfoItem.SetActive(true);
    }

    public void OpenInfoAboutSplinter(SplinterController splinterController, bool withControl = false)
    {
        RemoveAllListenersOnButtons();

        UpdateUIInfo(splinterController.splinter.Image, splinterController.splinter.Id);
        if (withControl)
        {
            componentButtonAction.onClick.AddListener(() => splinterController.GetReward());
            btnDrop.SetActive(false);
            btnAction.SetActive(true);
        }
        OpenPanel();
    }

    public void OpenInfo(BaseObject subject)
    {
        UpdateUIInfo(subject.Image, subject.GetName());
        PanelCharacteristics.SetActive(false);
        PanelCostume.SetActive(false);
        PanelController.SetActive(false);
        OpenPanel();
    }

    void UpdateUIInfo(Sprite image, string name, string type = "", string generalInfo = "", string addactInfo = "")
    {
        imageInfoItem.sprite = image;
        textNameItem.text = name;
        textTypeItem.text = type;
        textGeneralInfo.text = generalInfo;
        textAddactiveInfo.text = addactInfo;
    }

    void OpenPanel()
    {
        panelInfoItem.SetActive(true);
        btnClose.SetActive(true);
        isOpenInfoPanel = true;
    }

    void GetComponents()
    {
        componentButtonAction = btnAction.GetComponent<Button>();
        componentButtonDrop = btnDrop.GetComponent<Button>();
        buttonSwapItems = btnOpenInventory.GetComponent<Button>();
    }

    void RemoveAllListenersOnButtons()
    {
        if (componentButtonAction == null) GetComponents();
        componentButtonAction.onClick.RemoveAllListeners();
        componentButtonDrop.onClick.RemoveAllListeners();
    }

    public void OpenInventory()
    {
        InventoryController.Instance.Open(cellItem.typeCell, cellItem);
        Close();
    }

    public void Close()
    {
        panelInfoItem.SetActive(false);
        btnClose.SetActive(false);
        btnAction.SetActive(false);
        ClearInfo();
    }

    void ClearInfo()
    {
        UpdateUIInfo(null, string.Empty);
    }
}
