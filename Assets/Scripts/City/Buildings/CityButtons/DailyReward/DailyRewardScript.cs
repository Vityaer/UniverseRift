using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DailyRewardScript : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler{
    public int ID;
    public GameObject blockPanel, readyForGet;
    [SerializeField] private MyScrollRect scrollParent;
	public SubjectCellControllerScript rewardController;
	private MarketProduct reward;
	private EventAgentRewardStatus statusReward = EventAgentRewardStatus.Close;
	
	void Start()
	{
		ID = transform.GetSiblingIndex();
	}

	public void SetData(MarketProduct newProduct)
	{
		switch(newProduct){
			case MarketProduct<Resource> product:
				this.reward = product;
				rewardController.SetItem(product.subject);
				break;
			case MarketProduct<Item> product:
				this.reward = product;
				rewardController.SetItem(product.subject);
				break;
			case MarketProduct<Splinter> product:
				this.reward = product;
				rewardController.SetItem(product.subject);
				break;		
		}
	}

	public void SetStatus(EventAgentRewardStatus newStatusReward){
		statusReward = newStatusReward;
		UpdateUI();
	}

	private void UpdateUI(){
    	switch(statusReward){
			case EventAgentRewardStatus.Received:
				blockPanel.SetActive(true);
				readyForGet.SetActive(false);
				break;
			case EventAgentRewardStatus.Close:
				blockPanel.SetActive(false);
				readyForGet.SetActive(false);
				break;
			case EventAgentRewardStatus.Open:
				blockPanel.SetActive(false);
				readyForGet.SetActive(true);
				break;
		}
    }

	public void GetReward(){
		switch(statusReward){
			case EventAgentRewardStatus.Close:
				MessageControllerScript.Instance.AddMessage("Награда не открыта, приходите позже");		
				break;
			case EventAgentRewardStatus.Received:
				MessageControllerScript.Instance.AddMessage("Вы уже получали эту награду");		
				break;
			case EventAgentRewardStatus.Open:
				reward.GetProduct(1);
				DailyControllerScript.Instance.OnGetReward(transform.GetSiblingIndex());
				SetStatus(EventAgentRewardStatus.Received);
				break;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		scrollParent.OnBeginDrag(eventData);
	}

    public void OnDrag(PointerEventData eventData)
    { 
    	scrollParent.OnDrag(eventData); 
    }

    public void OnEndDrag(PointerEventData eventData)
    { 
    	scrollParent.OnEndDrag(eventData); 
    }
}