using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine.UI;

public class MarketProductScript : MonoBehaviour
{
	[OdinSerialize] private MarketProduct marketProduct;
	[SerializeField] private GameObject soldOutPanel;
	
	public ButtonCostScript buttonCost;
	public SubjectCellControllerScript cellProduct;
	private BaseObject _subject;

	private Action<int, int> callback = null;

	public void SetData(MarketProduct<Resource> product, Action<int, int> callback)
	{
		this.callback = callback;
		marketProduct = product;
		buttonCost.UpdateCost(product.Cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
		_subject = product.subject;
	}

	public void SetData(MarketProduct<Item> product, Action<int, int> callback)
	{
		this.callback = callback;
		marketProduct = product;
		buttonCost.UpdateCost(product.Cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
		_subject = product.subject;
	}

	public void SetData(MarketProduct<Splinter> product, Action<int, int> callback)
	{
		this.callback = callback;
		marketProduct = product;
		buttonCost.UpdateCost(product.Cost, Buy);
		cellProduct.SetItem(product.subject);
		UpdateUI();
		_subject = product.subject;
	}

	public void UpdateUI()
	{
		if(marketProduct.CountLeftProduct == marketProduct.CountMaxProduct)
		{
			buttonCost.Disable();
			soldOutPanel.SetActive(true);
		}
		else
		{
			soldOutPanel.SetActive(false);
			buttonCost.EnableButton();
		}
	}

	public void Recovery()
	{
		marketProduct.Recovery();
		UpdateUI();
	}

    public void Buy(int count = 1)
    {
		if((count + marketProduct.CountLeftProduct) > marketProduct.CountMaxProduct)
			count = marketProduct.CountMaxProduct - marketProduct.CountLeftProduct;
		marketProduct.GetProduct(count);

		UpdateUI();
		if(callback != null)
			callback(marketProduct.ID, marketProduct.CountLeftProduct);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
