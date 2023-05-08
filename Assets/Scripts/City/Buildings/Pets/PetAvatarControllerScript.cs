using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetAvatarControllerScript : MonoBehaviour{
	PetAvatar pet;

	[Header("UI")]
	public Text textName;
	public Text textLevel;
	public RatingHero rating;
	public Image background;
	public Image imagePet;


	public PetsZooScript mainController;
	public void SetData(PetAvatar newPet, PetsZooScript mainController){
		this.mainController = mainController;
		this.pet = newPet;
		UpdateUI();
	}
	public void UpdateUI(){
		textName.text  = pet.Name;
		textLevel.text = pet.Level.ToString();
		rating.ShowRating(pet.Rating);
		imagePet.sprite = pet.Avatar;
	}
	public void OpenPet(){
		mainController.OpenPet(pet);
	}   
}
