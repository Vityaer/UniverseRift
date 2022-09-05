using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TravelSelectScript : MonoBehaviour{
	[SerializeField] private GameObject selectBorder; 
	public Race race;
	public RaceUIScript raceUI;
	private static TravelSelectScript selectedRace = null;
	void Start(){
		raceUI.SetData(race);
	}
	public void Open(){
		TravelCircleScript.Instance.ChangeTravel(race);
	}
	public void Select(){
		if(selectedRace != null) selectedRace.Diselect();
		selectedRace = this;
		this.selectBorder.SetActive(true);
	}
	public void Diselect(){
		selectBorder.SetActive(false);
	}
}