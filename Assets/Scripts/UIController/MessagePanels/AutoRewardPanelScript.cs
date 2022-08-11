using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AutoRewardPanelScript : RewardPanelScript{
	public SliderTimeScript sliderAccumulation;
	public TextMeshProUGUI textAutoRewardGold, textAutoRewardStone, textAutoRewardExperience;
	private DateTime maxTime; 
	void Start(){
		maxTime = new DateTime().AddHours(12);
	}
	public void Open(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime){
		textAutoRewardGold.text       = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Gold).ToString(), "/ 5sec");
		textAutoRewardStone.text      = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.ContinuumStone).ToString(), "/ 5sec");
		textAutoRewardExperience.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Exp).ToString(), "/ 5sec");
		base.Open(calculatedReward);
		sliderAccumulation.SetData(previousDateTime, maxTime);
	}
	protected override void OnClose(){
		AutoFightScript.Instance.heap.GetReward();
	}
}
