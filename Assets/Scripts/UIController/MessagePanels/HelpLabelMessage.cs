using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpLabelMessage : MonoBehaviour{
	public void ShowMessage(){
		MessageController.Instance.AddMessage(message);
	}
	[SerializeField] private string message;
}
