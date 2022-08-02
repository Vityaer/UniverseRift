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

	void Awake(){
		image = GetComponent<Image>();
	} 
	void Start(){
		if(item?.Image != null) {SetBonus(); }else{ DefaulfView();}
	}


	public void DefaulfView(){
		item = null;
		image.sprite = defaultSprite;
		ratingController?.ShowRating(0);
	} 
	private void SetBonus(){
		TrainCampScript.Instance.ReturnSelectHero().CostumeHero.TakeOn(item);
		TrainCampScript.Instance.UpdateTextAboutHero();
		UpdateUI();
	}

	private void UpdateUI(){
		image.sprite =  item.Image;
		image.color  = new Color(255, 255, 255, 1); 
		ratingController?.ShowRating(item.Rating);
	}
//API
	public void Clear(){
		item = null;
		DefaulfView();
	}
	public void SetItem(Item newItem){
		if(item != null) {
			InventoryControllerScript.Instance.AddItem(item);
			TrainCampScript.Instance.TakeOff(item);
		}
		if(newItem != null){
			if(newItem.Type == typeCell){
				item = newItem;
			}
		}else{
			item = null;
		}
		if(newItem != null) {SetBonus(); }else{ DefaulfView();}
	}

	public void ClickOnCell(){
		if(item != null) {
			InventoryControllerScript.Instance.OpenInfoItem(item, typeCell, this);
		}else{
			InventoryControllerScript.Instance.Open(typeCell, this);
		}
	} 
}
