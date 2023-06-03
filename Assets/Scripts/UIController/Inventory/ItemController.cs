using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemController : VisualAPI, ICloneable
{
	private Item _item;
	public Item item{get => _item; set => _item = value;} 
	protected int amount;
	public int Amount{get => amount;}
	protected SubjectCellControllerScript cell = null;
	public SubjectCellControllerScript cellInvenroty{get => cell; set => cell = value;}
	protected ThingUIScript UI;

	public void ClickOnItem()
	{
		InventoryController.Instance.OpenInfoItem(this);
	}

	public ItemController(Item item)
	{
		this.item = item;
		this.amount = (item.Amount > 0) ? item.Amount : 1;
	}

	public ItemController(Item item, int amount)
	{
		this.item = item;
		this.amount = (amount > 0) ? amount : 1;
	}

	public ItemController()
	{
		this.amount = 1;
	}

	public void SetUI(ThingUIScript UI){
		this.UI = UI;
		UpdateUI();
	}

	public void UpdateUI()
	{
		UI?.UpdateUI(item.Image, Amount);
	}

	public VisualAPI GetVisual()
	{
		return (this as VisualAPI);
	}

	public void ClearUI()
	{
		this.UI = null;
	}

	public void DecreaseAmount(int count)
	{
		amount -= count;
	}

	public void IncreaseAmount(int count)
	{
		amount += count;
	}

	public object Clone()
	{
        return new ItemController  { 	_item         = this._item,
        							 	amount     = this.amount
        							};				
    }
}

public interface VisualAPI{
	VisualAPI GetVisual();
	void ClearUI();
	void SetUI(ThingUIScript UI);
	void UpdateUI();
	void ClickOnItem();
}