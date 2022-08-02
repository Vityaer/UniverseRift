using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PanelInfoMine : MonoBehaviour{
	public GameObject panel, panelController;
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI textNameMine, textLevelMine, textIncome, textStore;
	public  ItemSliderControllerScript sliderAmount;
	// public SubjectCellControllerScript product;
	public CostUIListScript costController;
	[SerializeField] private Button buttonLevelUp;
	private Mine mine;
	MineControllerScript mineController;
	public void SetData(MineControllerScript mineController){
		this.mineController = mineController;
		this.mine = mineController.GetMine;
		UpdateUI();
		Open();
	}
	public void LevelUP(){
		mineController.UpdateLevel();
		UpdateUI();
	}
	ListResource cost = new ListResource();
	private void UpdateUI(){
		// product.SetItem(mine.income);
		textNameMine.text = mine.income.GetName();
		textLevelMine.text = FunctionHelp.TextLevel(mine.level); 
		textIncome.text = mine.income.GetTextAmount();
		textStore.text = mine.GetMaxStore.GetTextAmount();
		sliderAmount.SetAmount(mine.GetStore.Amount, mine.GetMaxStore.Amount);
		cost = mine.GetCostLevelUp();
		costController.ShowCosts( cost );
		buttonLevelUp.interactable = PlayerScript.Instance.CheckResource( cost );
	}
	public void Open(){
		panel.SetActive(true);
	}
	public void Close(){
		panel.SetActive(false);
	}
}
