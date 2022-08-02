using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListFightTextsScript : MonoBehaviour{
	public List<DamageHealTextScript> listFightTextChangeHP = new List<DamageHealTextScript>();
	public GameObject prefabTextFight;
	public void ShowDamage(float damage, Vector2 pos){
		GetFightText().PlayDamage(damage, pos);
	}
	public void ShowHeal(float heal, Vector2 pos){
		GetFightText().PlayDamage(heal, pos);
	}
	public void ShowMessage(string message, Vector2 pos){
		// GetFightText()?.PlayMessage(message, pos);
	}
	private DamageHealTextScript GetFightText(){
		DamageHealTextScript result = listFightTextChangeHP.Find(x => x.InWork == false);
		if(result == null)
			result = Instantiate(prefabTextFight).GetComponent<DamageHealTextScript>();
		return result;
	}

	private static ListFightTextsScript instance;
	public static ListFightTextsScript Instance{get => instance;}
	void Awake(){
		if(instance == null){
			instance = this;
		}else{Destroy(this);}
	}
}
