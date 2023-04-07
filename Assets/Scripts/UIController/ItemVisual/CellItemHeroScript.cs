using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellItemHeroScript : MonoBehaviour{

	public TypeItem typeCell;
	public Sprite defaultSprite;

	[Header("Info")]
	public Image image;
	public RatingHeroScript ratingController;

	private Item item;

	void Awake()
	{
		image = GetComponent<Image>();
	} 

	void Start()
	{
		DefaulfView();
	}


	public void DefaulfView()
	{
		image.sprite = defaultSprite;
		ratingController?.ShowRating(0);
	} 

	private void SetBonus()
	{
		TrainCampScript.Instance.ReturnSelectHero().CostumeHero.TakeOn(item);
		TrainCampScript.Instance.heroPanel.UpdateTextAboutHero();
		UpdateUI();
	}

	private void UpdateUI()
	{
		image.sprite =  item.Image;
		image.color  = new Color(255, 255, 255, 1); 
		ratingController?.ShowRating(item.Rating);
	}
//API
	public void Clear()
	{
		Debug.Log("clear");
		item = null;
		DefaulfView();
	}

	public void SetItem(Item newItem)
	{
		if(item != null) 
		{
		Debug.Log("set item");
			InventoryControllerScript.Instance.AddItem(item);
			TrainCampScript.Instance.TakeOff(item);
			TrainCampScript.Instance.heroPanel.UpdateTextAboutHero();
		}

		item = newItem;

		if(item != null)
		{
			SetBonus();
		}
		else
		{ 
			DefaulfView();
		}
	}

	public void ClickOnCell()
	{
		if(item != null) {
			InventoryControllerScript.Instance.OpenInfoItem(item, typeCell, this);
		}else{
			InventoryControllerScript.Instance.Open(typeCell, this);
		}
	} 
}
