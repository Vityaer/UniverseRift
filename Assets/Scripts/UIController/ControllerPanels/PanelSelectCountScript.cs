using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PanelSelectCountScript : MonoBehaviour{

	public SubjectCellController cellProduct;
	public CountController countController;
	public ItemSliderController slider;
	public Button btnAction;
	public GameObject panel;

	private int requireCount = 0, storeCount = 0, selectedCount = 0; 
	void Start(){
		countController.RegisterOnChangeCount(ChangeCount);
		btnAction.onClick.AddListener(() => SelectedCountDone() );
	}

	public void ChangeCount(int count){
		selectedCount = count;
		slider.SetAmount(count * requireCount, requireCount);
	}
	SplinterController splinterController;
	public void Open(SplinterController splinterController, int requireCount, int storeCount){
		RegisterOnSelectedCount(splinterController.GetReward);
		cellProduct.SetItem(splinterController);
		countController.SetMax(storeCount / requireCount);
		this.requireCount = requireCount;
		this.storeCount = storeCount;
		ChangeCount(count: storeCount / requireCount);
		panel.SetActive(true);
	}
	private Action<int> actionOnSelectedCount;
	private Action actionAfterUse;
	public void RegisterOnActionAfterUse(Action d){actionAfterUse += d;}
	private void RegisterOnSelectedCount(Action<int> d){actionOnSelectedCount += d;}
	void SelectedCountDone(){
		if(actionOnSelectedCount != null){
			actionOnSelectedCount(selectedCount);
			actionOnSelectedCount = null;
		}
		if(actionAfterUse != null){
			actionAfterUse();
			actionAfterUse = null;
		}
		Close();
	}

	void OnDestroy(){
		countController.UnregisterOnChangeCount(ChangeCount);
	}
    public void Close(){ panel.SetActive(false); }
    void Awake(){instance = this;}
    private static PanelSelectCountScript instance;
    public static PanelSelectCountScript Instance{get => instance;}
}
