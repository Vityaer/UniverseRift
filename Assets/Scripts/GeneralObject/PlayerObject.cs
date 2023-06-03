using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : BaseObject
{
	[SerializeField] private int amount;
	public int Amount{get => amount; set => amount = value;}
	[SerializeField] protected string name = string.Empty;
	public string Name{get => name;}

	[SerializeField] public string Rating;
	
}