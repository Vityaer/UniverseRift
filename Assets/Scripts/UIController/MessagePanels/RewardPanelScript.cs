using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelScript : MonoBehaviour
{
	protected Reward reward;
    public GameObject panel;
    public RewardUIControllerScript rewardController;

	public void Open(Reward reward)
	{
		if(reward != null)
			SetReward(reward);
			
		panel.SetActive(true);
	}

	protected virtual void SetReward(Reward reward)
	{
		this.reward = reward.Clone();
		rewardController.ShowAllReward(reward);
	}

	private void GetReward()
	{
		PlayerScript.Instance.AddReward(reward);
		reward = null;
	}

	public void Close()
	{
		MessageControllerScript.Instance.OpenNextPanel();
		
		if(reward != null)
			GetReward();
		
		OnClose();
		panel.SetActive(false);
	}

	protected virtual void OnClose(){}
}
