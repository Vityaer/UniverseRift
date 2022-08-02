using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Round : ICloneable{
	public float      amount;
	public TypeNumber typeNumber;

	public void SetData(float amount, TypeNumber typeNumber){
		this.amount     = amount;
		this.typeNumber = typeNumber; 
	}
	public bool IsPercent{ get => (typeNumber == TypeNumber.Percent) ? true : false;}
	public object Clone(){
        return new Round  { amount = this.amount,
        					typeNumber = this.typeNumber
							};	
	}						
}
