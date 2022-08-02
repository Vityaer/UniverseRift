using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ThingUIScript : MonoBehaviour{
	[Header("Info")]
	public  Image border;
	public  Image background;
	public  Backlight backlight;
	public  Image imageThing;
	public  RatingHeroScript ratingThing;
	public  ItemSliderControllerScript sliderAmount;
	public  TextMeshProUGUI textAmount;
	public  GameObject doneForUse;
	public  Image selectBorder;
	[Header("controller")]
	public bool OnlyMainData = false;
	public void UpdateUI(Sprite image, Rare rare, string text, int rating = 0){
		Clear();
		imageThing.sprite  = image;
		if(ratingThing != null) ratingThing.ShowRating(rating);
		textAmount.text    = text;
		imageThing.enabled = true;
	}
	public void UpdateUI(Sprite image, Rare rare, int amount = 0, int rating = 0){
		Clear();
		imageThing.sprite  = image;
		textAmount.text = (amount > 0) ? amount.ToString() : string.Empty;
		if(ratingThing != null) ratingThing.ShowRating(rating);
		imageThing.enabled = true;
	}
	public void UpdateUI(Resource res){
		Clear();
		Debug.Log(res.ToString());
		imageThing.sprite  = res.Image;
		textAmount.text    = res.ToString();
		imageThing.enabled = true;
	}
	public void UpdateUI(SplinterController splinterController){
		Clear();
		imageThing.sprite = splinterController.splinter.Image;
		if(sliderAmount != null){
			if(OnlyMainData == false){
				sliderAmount.SetAmount(splinterController.splinter.Amount, splinterController.splinter.RequireAmount);
			}else{
				sliderAmount.Hide();
			}
		}
		imageThing.enabled = true;
	}
	public void Select(){
		if(selectBorder != null)
			selectBorder.enabled = true;
	}
	public void Diselect(){
		if(selectBorder != null)
			selectBorder.enabled = false;
	} 

	public void SwitchDoneForUse(bool flag){
		if(doneForUse != null)
			doneForUse.SetActive(flag);
	}
	public void Clear(){
		imageThing.sprite = null;
		if(ratingThing != null) ratingThing.ShowRating(0);
		if(sliderAmount != null) sliderAmount.Hide();
		imageThing.enabled = false;
		textAmount.text = "";
		Diselect();
		SwitchDoneForUse(false);
	}
}
