using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AndroidPlugin;
using System;
public class MessageControllerScript : MonoBehaviour{

	private static MessageControllerScript instance;
	public  static MessageControllerScript Instance{get => instance;}
	private Canvas canvas;
	void Awake(){
		canvas = GetComponent<Canvas>();
		instance = this;
	}
//	Result fight
	[Header("Fight result")]
	public RewardPanelScript rewardPanel;
	public ResultPanelWithRewardScript winPanelScript, losePanelScript;
	[Header("Auto reward panel")]
	public AutoRewardPanelScript autoRewardPanel;
	[Header("Other panels")]
	public PanelNewLevelPlayer panelNewLevelPlayer; 
	public void OpenWin(Reward reward = null){
		winPanelScript.Open();
		OpenRewardPanel(reward);
	}
	public void OpenDefeat(Reward reward = null){
		losePanelScript.Open();
		OpenRewardPanel(reward);
	}
	public void OpenAutoReward(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime){
		autoRewardPanel.Open(autoReward, previousDateTime);
		OpenRewardPanel(calculatedReward);
	}
	private void OpenRewardPanel(Reward reward){
		rewardPanel.SetReward(reward);
	}
	public void Close(){
		if(observerClose != null){
			observerClose();
			observerClose = null;
		}
	}
	private Action observerClose;
	public void RegisterOnClose(Action d){observerClose += d;}

	public void AddMessage(string newMessage, bool isLong = false){
		Debug.Log(newMessage);
		AndroidPlugin.PluginControllerScript.ToastPlugin.Show(newMessage, isLong: isLong);
	}
}
