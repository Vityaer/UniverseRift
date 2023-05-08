using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectSave;
using System;
public class FormRegisterController : Building{
	[SerializeField] private PanelRegistration panelRegistration;
	protected override void Start(){
		GameController.Instance.RegisterOnLoadGame(OnLoadGame);
	}
	protected override void OnLoadGame(){
		Debug.Log("name: " + GameController.GetPlayerInfo.Name);
		if(GameController.GetPlayerInfo.Name.Equals(string.Empty)){
			Debug.Log("OpenForRegister");
			panelRegistration.Open();
		}
	}
	private PlayerInfo playerInfo;
	public void CreateAccount(string name){
		GameController.Instance.RegisterPlayer(name);
		GetStartPack();
		SaveGame();
	}
	private void GetStartPack(){
		GameController.Instance.AddResource(new Resource(TypeResource.SimpleHireCard, 10));
		GameController.Instance.AddResource(new Resource(TypeResource.Diamond, 100));
		GameController.Instance.AddResource(new Resource(TypeResource.CoinFortune, 5));
	}
}