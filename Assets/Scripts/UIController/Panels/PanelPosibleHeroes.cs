using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPosibleHeroes : BasePanelScript{
	public Transform content;
	public GameObject prefabCard;
	public List<CardWithPercent> cardsWithPercent = new List<CardWithPercent>();
	public void Open(PosibleReward rewardInfo){
		CheckCountAvailableCard(rewardInfo.posibilityObjectRewards.Count);
		FillData(rewardInfo);
		base.Open();
	}


	private void CheckCountAvailableCard(int requireCount){
		if(cardsWithPercent.Count < requireCount)
			for(int i = cardsWithPercent.Count; i < requireCount; i++)
				cardsWithPercent.Add(Instantiate(prefabCard, transform).GetComponent<CardWithPercent>());
	}
	private void FillData(PosibleReward rewardInfo){
		for(int i = 0; i < rewardInfo.posibilityObjectRewards.Count; i++){
			var name = GameUtils.Utils.CastIdToName(rewardInfo.posibilityObjectRewards[i].ID);
            cardsWithPercent[i].SetData(name, rewardInfo.PosibleNumObject(i));
		}
		for (int i = rewardInfo.posibilityObjectRewards.Count; i < cardsWithPercent.Count; i++){
			cardsWithPercent[i].Hide();
		}
	}
}