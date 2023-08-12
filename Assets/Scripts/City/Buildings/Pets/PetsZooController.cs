using City.Buildings.Abstractions;
using City.Pets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace City.Buildings.Pets
{
    public class PetsZooController : BaseBuilding<PetsZooView>, IInitializable
    {
        public List<PetAvatar> pets = new List<PetAvatar>();
        public Transform poolPanel;
        private List<PetAvatarController> cells = new List<PetAvatarController>();
        public GameObject mainList;
        public Button btnBack;
        public PetDetail PetDetailController;

        public void Initialize()
        {
            //foreach (Transform child in poolPanel)
            //    cells.Add(child.GetComponent<PetAvatarController>());
        }

        protected override void OpenPage()
        {
            mainList.SetActive(true);
            for (int i = 0; i < pets.Count; i++)
            {
                cells[i].SetData(pets[i], this);
            }
            ChangeBtnBack();
        }

        public void OpenPet(PetAvatar pet)
        {
            mainList.SetActive(false);
            PetDetailController.ShowPet(pet);
        }

        private void ChangeBtnBack()
        {
            btnBack.onClick.RemoveAllListeners();
            btnBack.onClick.AddListener(() => Close());
        }
    }
}