using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CardWithPercent : MonoBehaviour{
   
	public CardScript cardInfo;
	public TextMeshProUGUI textPercent;
	public void SetData(int ID, float percent){
		cardInfo.ChangeInfo(TavernScript.Instance.GetInfoHero(ID));
		textPercent.text = string.Concat(percent.ToString(), "%");
		gameObject.SetActive(true);
	}
	public void Hide(){
   		gameObject.SetActive(false);
   	}
}
