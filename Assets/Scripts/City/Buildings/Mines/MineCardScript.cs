using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MineCardScript : MonoBehaviour{
	public GameObject panel;
	public Image imageMine, outLight;
	[SerializeField] private TextMeshProUGUI textCountRequirement;
	private DataAboutMines data;
	public void SetData(TypeMine type){
		data = MinesScript.Instance.GetDataMineFromType(type);
		imageMine.sprite = data.image;
		textCountRequirement.text = FunctionHelp.AmountFromRequireCount(data.currentCount, data.maxCount); 
		panel.SetActive(true);
	}
	public bool GetCanCreateFromCount{get => (data.currentCount < data.maxCount);} 
	public void Select(){
		if(data.currentCount < data.maxCount){
			MinesScript.Instance.panelNewMineCreate.UpdateUI(data);
			if(selectMineCard != null) selectMineCard.Diselect();
			selectMineCard = this;
			outLight.enabled = true;;
		}
	}
	public void Diselect(){
		outLight.enabled = false;
	}
	public static void DiselectAfterCreate(){ selectMineCard?.Diselect();}
	private static MineCardScript selectMineCard = null;
	public void Hide(){ panel.SetActive(false); }
}
