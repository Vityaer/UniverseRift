using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplinterController : VisualAPI, ICloneable{
	[SerializeField]private Splinter _splinter;
	public Splinter splinter{get => _splinter;}
	public void ClickOnItem(){
		InventoryControllerScript.Instance.OpenInfoItem(this, withControl : true);
	}
	public SplinterController(Splinter splinter, int amount){
		this._splinter = (Splinter) splinter.Clone();
		this._splinter.SetAmount(amount);
	}
	public SplinterController(Splinter splinter){
		this._splinter = (Splinter) splinter.Clone();
	}
	public SplinterController() : base(){
		_splinter = null;
	}
	protected ThingUIScript UI;
	public void SetUI(ThingUIScript UI){
		this.UI = UI;
		UpdateUI();
	}
	public void UpdateUI(){
		UI?.UpdateUI(this);
	}
	public void ClearUI(){
		this.UI = null;
	}
	public VisualAPI GetVisual(){
		return (this as VisualAPI);
	}
	public void GetReward(int count = 1){
		splinter.GetReward(count);
		if(splinter.Amount == 0){
			InventoryControllerScript.Instance.DropSplinter(this);
		}
	}
	public void IncreaseAmount(int count){ splinter.AddAmount(count); }
	public object Clone(){
        return new SplinterController  { _splinter  = this._splinter };				
    }
    public int CountReward{get => splinter.CountReward;}
    public bool IsCanUse{get => splinter.IsCanUse; }
}
