using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroStatusScript : MonoBehaviour{
	private List<Buff> listBuff = new List<Buff>();
	public void SetBuff(Buff buff){
		listBuff.Add(buff);
	}
	public int GetAllBuffArmor(){
		int armor = 0;
		List<Buff> armorBuffs = listBuff.FindAll(x => x.type == TypeBuff.Armor);
		for(int i = 0; i < armorBuffs.Count; i++){
			armor += (int) Mathf.Round(armorBuffs[i].GetCurrentAmount);
		}
		Debug.Log("bonus armor: " + armor.ToString());
		return armor;
	}
	private void CheckBuffs(){
		for(int i = 0; i < listBuff.Count; i++)
			listBuff[i].FinishRound();
		listBuff = listBuff.FindAll(x => x.rounds.Count > 0);		
	}
}

[System.Serializable]
public class Buff{
	public TypeBuff type;
	public List<Round> rounds = new List<Round>();

	public Buff(TypeBuff type, float amount, int countRound = 1, TypeNumber typeNumber = TypeNumber.Num){
		this.type = type;
		for(int i = 0; i < countRound; i++)
			rounds.Add(new Round(amount, typeNumber));
	}
	public void FinishRound(){
		rounds.RemoveAt(0);
	}
	public float GetCurrentAmount{get => rounds[0].amount;}
}
public enum TypeBuff{
	Armor
}