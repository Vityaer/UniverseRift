using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelNewPlayerName : BasePanelScript{
	public TMP_InputField inputFieldNewNamePlayer;
	public ButtonCostScript buttonPayNewName;
	[SerializeField] private PanelPlayerScript mainPlayerController;
	private string currentName;
	void Start(){
		buttonPayNewName.RegisterOnBuy(SaveNewName);
	}
	public void OnChangeNewName(){
		if(inputFieldNewNamePlayer.text.Equals(currentName) == false){
			if(inputFieldNewNamePlayer.text.Length > 3){
				buttonPayNewName.EnableButton();
			}else{
				buttonPayNewName.Disable();
			}
		}else{
			buttonPayNewName.Disable();
		}
	}
	public void SaveNewName(int count){
		currentName = inputFieldNewNamePlayer.text;
		mainPlayerController.SaveNewName(inputFieldNewNamePlayer.text);
		Close();
	}
	protected override void OnOpen(){
		currentName = PlayerScript.Instance.player.GetPlayerInfo.Name;
		inputFieldNewNamePlayer.text = currentName;
		inputFieldNewNamePlayer.Select();
	}
}
