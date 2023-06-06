using UnityEngine;
using UnityEngine.UI;
public class PetDetail : MonoBehaviour
{
    public Image imagePet;
    public Text level;
    public Text name;
    public Button btnUpdateLevel;

    [Header("Controller")]
    public Button btnBack;
    public PetsZoo mainController;
    PetAvatar pet;
    public void ShowPet(PetAvatar pet)
    {
        gameObject.SetActive(true);
        this.pet = pet;
        imagePet.sprite = pet.Avatar;
        level.text = pet.Level.ToString();
        name.text = pet.Name;
        ChangeBtnBack();
    }
    public void UpdateLevel()
    {
        pet.Level += 1;
        level.text = pet.Level.ToString();
    }

    public void BackToList()
    {
        mainController.Open();
        gameObject.SetActive(false);
    }

    private void ChangeBtnBack()
    {
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(() => BackToList());
    }
}
