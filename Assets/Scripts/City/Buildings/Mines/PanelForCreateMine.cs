using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelForCreateMine : MonoBehaviour{
	
	public GameObject panel, panelController;
	public Button btnCreate;
	public List<MineCardScript> mineCards = new List<MineCardScript>(); 
	public CostUIListScript costController;
	private PlaceForMineScript place;
	public void Open(PlaceForMineScript placeMine){
		place = placeMine;
		for(int i = 0; i < placeMine.types.Count; i++){
			mineCards[i].SetData(placeMine.types[i]);
		}
		for(int i = placeMine.types.Count; i < mineCards.Count; i++){
			mineCards[i].Hide();
		}
		MineCardScript startSelectCard = mineCards.Find(x => (x.GetCanCreateFromCount == true));
		if(startSelectCard != null){
			startSelectCard.Select();
		}else{
			panelController.SetActive(false);
		}
		panel.SetActive(true);

	}
	DataAboutMines data = null;
	public void UpdateUI(DataAboutMines newData){
		this.data = newData;
		panelController.SetActive(true);
		costController.ShowCosts(data.costCreate);
		btnCreate.interactable = PlayerScript.Instance.CheckResource( data.costCreate );
	}
	public void CreateMine(){
		MineCardScript.DiselectAfterCreate();
		PlayerScript.Instance.SubtractResource( data.costCreate );
		Close();
		MinesScript.Instance.CreateNewMine(place, data);
	}
	public void Close(){
		panel.SetActive(false);
	}
}
