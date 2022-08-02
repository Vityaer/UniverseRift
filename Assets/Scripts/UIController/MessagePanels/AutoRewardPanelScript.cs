using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AutoRewardPanelScript : ResultPanelWithRewardScript{
	public SliderTimeScript sliderAccumulation;
	public TextMeshProUGUI textAutoRewardGold, textAutoRewardStone, textAutoRewardExperience;
	public void Open(AutoReward autoReward, DateTime previousDateTime){
		textAutoRewardGold.text       = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Gold).ToString(), "/ 5sec");
		textAutoRewardStone.text      = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.ContinuumStone).ToString(), "/ 5sec");
		textAutoRewardExperience.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Exp).ToString(), "/ 5sec");
		base.Open();
		sliderAccumulation.SetData(previousDateTime, new DateTime().AddHours(12));
	}
	protected override void OnClose(){
		AutoFightScript.Instance.heap.GetReward();
	}
}
