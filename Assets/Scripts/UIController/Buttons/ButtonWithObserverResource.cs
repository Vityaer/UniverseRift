using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWithObserverResource : MonoBehaviour{
	public ResourceObjectCost resourceObserver;
	public ButtonCostScript buttonCostComponent;
	public Resource cost;
	public void ChangeCost(Resource cost){
		resourceObserver.SetData(cost);		
	}
	public void ChangeCost(Action<int> d){
		resourceObserver.SetData(this.cost);	
		buttonCostComponent.UpdateCostWithoutInfo(this.cost, d);

	}
	public void ChangeCost(Resource cost, Action<int> d){
		resourceObserver.SetData(cost);	
		buttonCostComponent.UpdateCostWithoutInfo(cost, d);

	}
}