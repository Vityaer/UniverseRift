using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CycleEventsController : MonoBehaviour{
	public List<MainEventController> listEvents = new List<MainEventController>();
	[SerializeField] private int cicleTimeDay;
	void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
	}
	private void OnLoadGame(){
		Debug.Log("load game empty: " + gameObject.name);
	}
}