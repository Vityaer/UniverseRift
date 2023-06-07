using Assets.Scripts.City.Buildings.Pets;
using City.Pets;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Pets
{
    public class PetAvatarController : MonoBehaviour
    {
        PetAvatar pet;

        [Header("UI")]
        public Text textName;
        public Text textLevel;
        public RatingHero rating;
        public Image background;
        public Image imagePet;


        public PetsZoo mainController;
        public void SetData(PetAvatar newPet, PetsZoo mainController)
        {
            this.mainController = mainController;
            pet = newPet;
            UpdateUI();
        }
        public void UpdateUI()
        {
            textName.text = pet.Name;
            textLevel.text = pet.Level.ToString();
            rating.ShowRating(pet.Rating);
            imagePet.sprite = pet.Avatar;
        }
        public void OpenPet()
        {
            mainController.OpenPet(pet);
        }
    }
}