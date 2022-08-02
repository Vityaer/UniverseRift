using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelScript : MonoBehaviour{
	private Reward reward;
    public GameObject panel;
    public RewardUIControllerScript rewardController;
	public void SetReward(Reward reward){
		this.reward = reward.Clone();
		MessageControllerScript.Instance.RegisterOnClose(Close);
		rewardController.ShowAllReward(reward);
		Open();
	}
	public void GetReward(){
		PlayerScript.Instance.AddReward(reward);
	}
	public void Open(){
		panel.SetActive(true);
	}
	public void Close(){
		GetReward();
		panel.SetActive(false);
	}
}
