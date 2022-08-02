using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleeAttackUI : MonoBehaviour{
	public GameObject directionObject;
	public NeighbourDirection direction;
	public void Open(){
		directionObject.SetActive(true);
	}
	public void Hide(){
		directionObject.SetActive(false);
	}
}
