using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using System;
public class FormRegisterController : Building{
	[SerializeField] private PanelRegistration panelRegistration;
	protected override void Start(){
		PlayerScript.Instance.RegisterOnLoadGame(OnLoadGame);
	}
	protected override void OnLoadGame(){
		Debug.Log("name: " + PlayerScript.GetPlayerInfo.Name);
		if(PlayerScript.GetPlayerInfo.Name.Equals(string.Empty)){
			Debug.Log("OpenForRegister");
			panelRegistration.Open();
		}
	}
	private PlayerInfo playerInfo;
	public void CreateAccount(string name){
		PlayerScript.Instance.RegisterPlayer(name);
		SaveGame();
	}
}