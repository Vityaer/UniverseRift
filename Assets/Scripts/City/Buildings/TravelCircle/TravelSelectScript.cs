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
		TravelCircleScript.Instance.OpenTravel(race);
	}
	public void Select(){
		if(selectedRace != null){
			selectedRace.Diselect();
		}
		this.selectBorder.SetActive(false);
	}
	public void Diselect(){
		selectBorder.SetActive(false);
	}
}