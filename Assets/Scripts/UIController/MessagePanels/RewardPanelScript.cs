using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelScript : MonoBehaviour{
	protected Reward reward;
    public GameObject panel;
    public RewardUIControllerScript rewardController;
	protected void SetReward(Reward reward){
		if(reward != null){
			this.reward = reward.Clone();
			rewardController.ShowAllReward(reward);
		}
	}
	public void GetReward(){
		if(reward != null) PlayerScript.Instance.AddReward(reward);
	}
	public void Open(Reward reward){
		if(reward != null) SetReward(reward);
		panel.SetActive(true);
	}
	public void Close(){
		MessageControllerScript.Instance.OpenNextPanel();
		reward = null;
		GetReward();
		OnClose();
		panel.SetActive(false);
	}
	protected virtual void OnClose(){}
}
