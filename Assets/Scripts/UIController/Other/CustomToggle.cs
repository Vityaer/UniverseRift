using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomToggle : MonoBehaviour{
	public Image imageToggle;
	[SerializeField] private bool isOn = false;
	public Sprite spriteIsOn, spriteIsOff;
	[ContextMenu("ChangeState")]
	public void ChangeState(){
		isOn = !isOn;
		imageToggle.sprite = isOn ? spriteIsOn : spriteIsOff;
	} 
}
