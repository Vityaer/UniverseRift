using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Common;
using UIController.MessagePanels;
using UIController.Reward;

public class PanelNewLevelPlayer : RewardPanel{
	public TextMeshProUGUI textNewLevel;
	public void Open(Reward reward){
		int newLevel = GameController.Instance.player.GetPlayerInfo.Level; 
		textNewLevel.text = newLevel.ToString();
		base.Open(reward);
	}
}
