using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelForCreateMine : MonoBehaviour{
	
	public GameObject panel, panelController;
	public Button btnCreate;
	public List<MineCard> mineCards = new List<MineCard>(); 
	public CostUIList costController;
	private PlaceForMine place;
	public void Open(PlaceForMine placeMine){
		place = placeMine;
		for(int i = 0; i < placeMine.types.Count; i++){
			mineCards[i].SetData(placeMine.types[i]);
		}
		for(int i = placeMine.types.Count; i < mineCards.Count; i++){
			mineCards[i].Hide();
		}
		MineCard startSelectCard = mineCards.Find(x => (x.GetCanCreateFromCount == true));
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
		btnCreate.interactable = GameController.Instance.CheckResource( data.costCreate );
	}
	public void CreateMine(){
		MineCard.DiselectAfterCreate();
		GameController.Instance.SubtractResource( data.costCreate );
		Close();
		MinesController.Instance.CreateNewMine(place, data);
	}
	public void Close(){
		panel.SetActive(false);
	}
}
