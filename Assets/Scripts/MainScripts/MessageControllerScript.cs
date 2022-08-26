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

//	Messages
	[SerializeField] private ErrorMessageController errorMessageController;
	public void ShowErrorMessage(string errorText){ errorMessageController.ShowMessage(errorText); }	
//	Result fight
	[SerializeField] private RewardPanelScript rewardPanel, winPanel, losePanel;
	[SerializeField] private AutoRewardPanelScript autoRewardPanel;
	[SerializeField] private PanelNewLevelPlayer panelNewLevelPlayer; 
	public void OpenWin(Reward reward, Action delOnClose){
		AddQueue(winPanel, () => winPanel.Open(reward), delOnClose );
	}
	public void OpenDefeat(Reward reward, Action delOnClose){ 
		AddQueue(losePanel, () => losePanel.Open(reward) , delOnClose);
	}	
	public void OpenAutoReward(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime){
		AddQueue(autoRewardPanel, () => autoRewardPanel.Open(autoReward, calculatedReward, previousDateTime) );
	}
	public void OpenPanelNewLevel(Reward reward){
		AddQueue(panelNewLevelPlayer, () => panelNewLevelPlayer.Open(reward) );
	}
	private void OpenSimpleRewardPanel(Reward reward){
		AddQueue(rewardPanel, () => rewardPanel.Open(reward) );
	}

	public void AddMessage(string newMessage, bool isLong = false){
		Debug.Log(newMessage);
		AndroidPlugin.PluginControllerScript.ToastPlugin.Show(newMessage, isLong: isLong);
	}
//Queue panel reward
	private Queue<PanelRecord> queuePanels = new Queue<PanelRecord>();
	PanelRecord currentPanel = null;
	private void AddQueue(RewardPanelScript panel, Action delOnpen, Action delOnClose = null){
		queuePanels.Enqueue(new PanelRecord(panel, delOnpen, delOnClose));
		if(queuePanels.Count == 1) OpenNextPanel();
	}
	public void OpenNextPanel(){
		if(currentPanel != null) currentPanel.OnClose(); 
		if(queuePanels.Count > 0){
			currentPanel = queuePanels.Dequeue();
			currentPanel.Open();
		}
	}
	[System.Serializable]
	public class PanelRecord{
		private RewardPanelScript panel;
		private Action delOnpen, delOnClose;
		public PanelRecord(RewardPanelScript panel, Action delOnpen, Action delOnClose = null){
			this.panel      = panel;
			this.delOnpen   = delOnpen;
			this.delOnClose = delOnClose;
		}
		public void Open(){ delOnpen(); }
		public void OnClose(){
			if(delOnClose != null)
				delOnClose();
		}
	}
}