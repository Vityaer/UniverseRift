using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class EventAgentRewardScript : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler{
	[Header("Data")]
	public RewardUIController rewardController;

	private Reward reward;
	public int ID;
	public void SetData(Reward reward){
		this.reward = reward;
		rewardController.ShowReward(reward);
	}
	private EventAgentRewardStatus statusReward = EventAgentRewardStatus.Close;
	public void SetStatus(EventAgentRewardStatus newStatusReward){
		statusReward = newStatusReward;
		UpdateUI();
	}
	public void GetReward(){
		switch(statusReward){
			case EventAgentRewardStatus.Close:
				MessageController.Instance.AddMessage("Награду ещё нужно заслужить, приходите позже");		
				break;
			case EventAgentRewardStatus.Received:
				MessageController.Instance.AddMessage("Вы уже получали эту награду");		
				break;
			case EventAgentRewardStatus.Open:
				GameController.Instance.AddReward(this.reward);
				EventAgentControllerScript.Instance.OnGetReward(ID);
				SetStatus(EventAgentRewardStatus.Received);
				break;
		}
	}
    public void OnBeginDrag(PointerEventData eventData){
    	EventAgentControllerScript.Instance.scrollRectController.OnBeginDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData){
    	EventAgentControllerScript.Instance.scrollRectController.OnDrag(eventData);
    }
    public void OnEndDrag(PointerEventData eventData){
    	EventAgentControllerScript.Instance.scrollRectController.OnEndDrag(eventData);
    }
    [Header("UI")]
    public Image background;
    public Image blockPanel;
    public Color colorClose, colorOpen, colorReceive;
    private void UpdateUI(){
    	switch(statusReward){
			case EventAgentRewardStatus.Close:
				blockPanel.color = colorClose;
				break;
			case EventAgentRewardStatus.Received:
				blockPanel.color = colorReceive;
				break;
			case EventAgentRewardStatus.Open:
				blockPanel.color = colorOpen;
				break;
		}
    }
}
public enum EventAgentRewardStatus{
	Close = 0,
	Open = 1,
	Received = 2
}