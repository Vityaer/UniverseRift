using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelNewLevelPlayer : ResultPanelWithRewardScript{
	public TextMeshProUGUI textNewLevel;
	public CostLevelUp rewardForLevelUp;
	public RewardUIControllerScript rewardController;
	Reward reward = null;
	public override void Open(){
		int newLevel = PlayerScript.Instance.player.GetPlayerInfo.Level; 
		textNewLevel.text = newLevel.ToString();
		ListResource resources = rewardForLevelUp.GetCostForLevelUp(newLevel);
		if(reward == null){
			reward = new Reward(resources);
		}else{
			reward.AddResources(resources);		
		}
		rewardController.ShowReward(reward);
		base.Open();
	}
	protected override void OnClose(){
		PlayerScript.Instance.AddReward(reward);
		reward = null;
	}
}
