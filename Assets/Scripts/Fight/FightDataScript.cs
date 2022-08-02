using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDataScript : MonoBehaviour{
   
   	public float lastDamage;
	public delegate void Del(float damage);
	public Del delsLastDamage;
	public void RegisterOnLastDamage(Del d){
		delsLastDamage += d;
	}
	public void UnRegisterOnLastDamage(Del d){
		delsLastDamage += d;
	}
	public void GetLastDamage(){
		if(delsLastDamage != null)
			delsLastDamage(lastDamage);
	}

	private static FightDataScript instance;
	public static FightDataScript Instance{get => instance;}
	void Awake(){
		if(instance == null){
			instance = this;
		}else{
			Destroy(this);
		}
	}
}
