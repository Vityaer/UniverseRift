using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinterSystem : MonoBehaviour{

	public SplintersList splintersList;
	public Splinter GetSplinter(int ID){ return splintersList.GetSplinter(ID); }

	void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Debug.Log("splinter system create twice, gameObject: " + gameObject.name);
		}

	}
	private static SplinterSystem instance;
	public static SplinterSystem Instance{get => instance;}
}