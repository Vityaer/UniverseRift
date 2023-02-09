using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionSheetScript : MainPage
{
	[SerializeField] private Canvas canvasActionUI;
	[SerializeField] private GameObject background;

	public override void Open()
	{
		base.Open();
		canvasActionUI.enabled = true;
		BackGroundControllerScript.Instance.OpenBackground(background);
	}

	public override void Close()
	{
		canvasActionUI.enabled = false;
	}
}
