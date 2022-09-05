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
	public Round(float amount, TypeNumber typeNumber){
		this.amount = amount;
		this.typeNumber = typeNumber;
	}
	public void Add(float other){ amount += other; }
	public bool AmountEqualsZero(){return Mathf.Abs(amount) < 0.01f;}
	public bool IsPercent{ get => (typeNumber == TypeNumber.Percent) ? true : false;}
	public object Clone(){
        return new Round  (this.amount, this.typeNumber);
	}						
}
public enum TypeNumber{
	Percent,
	Num
}