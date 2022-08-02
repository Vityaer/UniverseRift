using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Challenge{

	[SerializeField] private string name;
	public string Name{
		get => name; 
		set {if (value.Length >= 3) { name = value; }else{ MessageControllerScript.Instance.AddMessage("Name of chellenge must have more 2 simbols"); }}
	}
	[SerializeField] private int id;
	public int ID{get => id;}

	[SerializeField] private Mission _mission;
	public Mission mission{get => _mission;}

	[SerializeField] private bool isDone = false;
	public bool IsDone{get => isDone; set => isDone = value;}
	public Challenge(string name, Mission _mission, bool isDone = false){
		this.name     = name;
		this._mission = _mission;
		this.isDone   = isDone;
	}
}	