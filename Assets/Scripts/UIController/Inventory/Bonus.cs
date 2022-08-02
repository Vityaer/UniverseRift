using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Bonus{
	[SerializeField]
	private TypeCharacteristic name;
	public TypeCharacteristic Name{get => name; set => name = value;}

	[SerializeField]
	private float count;
	public float Count{get => count; set => count = value;} 
}
