using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelInfoItemScript : MonoBehaviour
{
	private static PanelInfoItemScript _instance;
	public static PanelInfoItemScript Instance => _instance;

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
	private Button componentButtonAction, componentButtonDrop, buttonSwapItems;
	private CellItemHeroScript cellItem; 

	void Awake()
	{
		_instance = this;
		Debug.Log(Instance == null);
		GetComponents();
	}

	void Start()
	{
		buttonSwapItems.onClick.AddListener(() => Close());
		buttonSwapItems.onClick.AddListener(() => InventoryControllerScript.Instance.Open(cellItem.typeCell, cellItem));
	}

	//Panel InfoAboutItem  
	public void OpenInfoAboutItem(Item item, CellItemHeroScript cellItem, bool onHero = false)
	{
		RemoveAllListenersOnButtons();
		componentButtonAction.onClick.AddListener( Close );
		componentButtonDrop.onClick.AddListener( Close );
		this.cellItem = cellItem;
		btnOpenInventory.SetActive(onHero);
		btnAction.SetActive( cellItem != null );

		if(onHero == false)
		{
			componentButtonAction.onClick.AddListener(InventoryControllerScript.Instance.SelectItem);
			componentButtonDrop.onClick.AddListener(InventoryControllerScript.Instance.DropItem);
			textButtonDrop.text = "Выбросить";
			textButtonAction.text = "Снарядить";
		}
		else
		{
			componentButtonDrop.onClick.AddListener( () => cellItem.SetItem(null) );
			componentButtonDrop.onClick.AddListener(() => InventoryControllerScript.Instance.Close());
			textButtonDrop.text = "Снять";
		}

		selectItem = item;
		UpdateUIInfo(item.Image, item.Name, type: item.Type.ToString(), generalInfo: item.GetTextBonuses());
		OpenPanel();
	}

	public void OpenInfoAboutItem(Resource res)
	{
		imageInfoItem.sprite = res.Image;
		textNameItem.text    = res.Name.ToString();
		panelInfoItem.SetActive(true);
	}

	public void OpenInfoAboutSplinter(SplinterController splinterController, bool withControl = false)
	{
		Debug.Log("open splinter");
		RemoveAllListenersOnButtons();
		componentButtonDrop.onClick.AddListener( Close );
		
		UpdateUIInfo(splinterController.splinter.Image, splinterController.splinter.Name);
		if(withControl){
			componentButtonAction.onClick.AddListener(() => splinterController.GetReward());
			componentButtonDrop.onClick.AddListener(() => InventoryControllerScript.Instance.DropSplinter(splinterController));
			textButtonDrop.text = "Выбросить";
			btnAction.SetActive(true);
		}
		OpenPanel();
	}

	public void OpenInfo(BaseObject subject)
	{
		UpdateUIInfo(subject.Image, subject.GetName());
	}

	void UpdateUIInfo(Sprite image, string name, string type = "", string generalInfo = "", string addactInfo = "")
	{
		imageInfoItem.sprite   = image;
		textNameItem.text      = name;
		textTypeItem.text      = type;
		textGeneralInfo.text   = generalInfo;
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
		componentButtonDrop   = btnDrop.GetComponent<Button>();
		buttonSwapItems       = btnOpenInventory.GetComponent<Button>();
	}

	void RemoveAllListenersOnButtons()
	{
		if(componentButtonAction == null) GetComponents();
		componentButtonAction.onClick.RemoveAllListeners();
		componentButtonDrop.onClick.RemoveAllListeners();
	}

	public void OpenInventory()
	{
		InventoryControllerScript.Instance.Open(cellItem.typeCell, cellItem);
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
