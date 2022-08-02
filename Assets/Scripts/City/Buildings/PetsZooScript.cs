using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PetsZooScript : Building{
	public List<PetAvatar> pets = new List<PetAvatar>();
	public Transform poolPanel;
	private List<PetAvatarControllerScript> cells = new List<PetAvatarControllerScript>();
	public GameObject mainList;
	public Button btnBack;

	void Awake(){
		foreach(Transform child in poolPanel)
			cells.Add(child.GetComponent<PetAvatarControllerScript>());
	}
	protected override void OpenPage(){
		mainList.SetActive(true);
		for(int i=0; i < pets.Count; i++){
			cells[i].SetData(pets[i], this);
		}
		ChangeBtnBack();
	}

	[Header("Pet detail")]
	public PetDetailScript petDetailController;
	public void OpenPet(PetAvatar pet){
		mainList.SetActive(false);
		petDetailController.ShowPet(pet);
	}

	private void ChangeBtnBack(){
		btnBack.onClick.RemoveAllListeners();
		btnBack.onClick.AddListener( () => Close() );  
	}
}
