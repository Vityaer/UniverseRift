using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelRegistration: BasePanelScript{
	public TMP_InputField inputFieldNewNamePlayer;
	public FormRegisterController registrationController;
	public Button buttonRegistration;
	private string currentName;
	public void SaveNewName(){
		currentName = inputFieldNewNamePlayer.text;
		registrationController.CreateAccount(currentName);
		Close();
	}
	public void OnChangeNewName(){
		if(inputFieldNewNamePlayer.text.Equals(currentName) == false){
			if(inputFieldNewNamePlayer.text.Length > 3){
				buttonRegistration.interactable = true;
			}else{
				buttonRegistration.interactable = false;
			}
		}else{
				buttonRegistration.interactable = false;
		}
	}
	protected override void OnOpen(){
		currentName = GameController.Instance.player.GetPlayerInfo.Name;
		inputFieldNewNamePlayer.text = currentName;
		inputFieldNewNamePlayer.Select();
	}
}