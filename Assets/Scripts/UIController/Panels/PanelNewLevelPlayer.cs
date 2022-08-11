using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelNewLevelPlayer : RewardPanelScript{
	public TextMeshProUGUI textNewLevel;
	public void Open(Reward reward){
		int newLevel = PlayerScript.Instance.player.GetPlayerInfo.Level; 
		textNewLevel.text = newLevel.ToString();
		base.Open(reward);
	}
}
