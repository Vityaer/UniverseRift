using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLabelMessage : MonoBehaviour{
	public void ShowMessage(){
		MessageControllerScript.Instance.AddMessage(message);
	}
	[SerializeField] private string message;
}
