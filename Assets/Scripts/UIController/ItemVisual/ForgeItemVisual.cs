using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ForgeItemVisual : MonoBehaviour{

	[Header("Info")]
	public  TypeMatter matter;
	private ItemSynthesis thing;
	public ItemSynthesis Thing{get => thing;}
	private Item item;
	public ThingUIScript UIItem;
	public ResourceObjectCost resourceCost;
	public ForgeItemObjectCost forgeItemCost;

	public void SetItem(ItemSynthesis item)
	{
		thing = item;
		UIItem.UpdateUI(thing.reward.Image, Rare.C, rating: item.reward.Rating);
	}

	public void SetItem(Item item)
	{
		this.item = item;
		UIItem.UpdateUI(item.Image, Rare.C, rating: item.Rating);
	}

	public void SetItem(Item item, int amount)
	{
		SetItem(item);
		forgeItemCost.SetInfo(item, amount);
	}

	public void SetResource(Resource res)
	{
		UIItem.UpdateUI(res.Image, Rare.C, 1);
		resourceCost.SetData(res);
	}

	public void SelectItem()
	{
		if(matter == TypeMatter.Synthesis)
		{
			ForgeScript.Instance.SelectItem(this, thing);
			UIItem.Select();
		}
	}
}

public class Backlight{
	public GameObject backlight;
}
public enum TypeMatter{
	Synthesis,
	Info
}