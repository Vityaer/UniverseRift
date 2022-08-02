using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
public class CountControllerScript : MonoBehaviour{
	private Action<int> observerOnCount;
	private int count = 1;
	[SerializeField] private int minCount = 1, maxCount = 10, delta = 1;
	public int Count{get => count;}
	public int MinCount{get => minCount;}
	public int MaxCount{get => maxCount;}
	[Header("UI")]
	public TextMeshProUGUI textCount;
	public Button buttonIncereaseCount, buttonDeacreaseCount; 
	void Start(){
		count = minCount;
	}
	public void IncreaseCount(){
		if(count == minCount) buttonDeacreaseCount.interactable = true;
		if(count < maxCount) count += delta;
		if(count > maxCount) count = maxCount;
		if(count == maxCount) buttonIncereaseCount.interactable = false;
		OnChangeCount();
	}
	public void DecreaseCount(){
		if(count == maxCount) buttonIncereaseCount.interactable = true;
		if(count > minCount) count -= delta;
		if(count < minCount) count = minCount;
		if(count == minCount) buttonDeacreaseCount.interactable = false;
		OnChangeCount();
	}
	private void UpdateUI(){
		textCount.text = count.ToString();
	}
	public void RegisterOnChangeCount(Action<int> d){observerOnCount += d;}
	public void UnregisterOnChangeCount(Action<int> d){observerOnCount -= d;}
	private void OnChangeCount(){
		UpdateUI();
		if(observerOnCount != null)
			observerOnCount(count);
	}

//API
	public void SetMax(int newMax){
		maxCount = newMax;
		if(count > newMax) count = newMax;
		OnChangeCount();
	}
	public void SetMin(int newMin){
		minCount = newMin;
		if(count < newMin) count = newMin;
		OnChangeCount();
	}	
}