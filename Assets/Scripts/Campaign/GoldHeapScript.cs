using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using HelpFuction;
using DG.Tweening;

public class GoldHeapScript : MonoBehaviour{
	[SerializeField] private AutoReward autoReward;
	[SerializeField] private Reward calculatedReward;
	public DateTime previousDateTime;
	[SerializeField] private RectTransform imageGoldRectTransform;
	[SerializeField] private Image imageHeap;
	void Start(){ Timer = TimerScript.Timer; }

	public void SetNewReward(AutoReward newAutoReward){ 
		if(newAutoReward != null)
			this.autoReward = newAutoReward;
	} 
	public void OnClickHeap(){
		previousDateTime = CampaignScript.Instance.GetAutoFightPreviousDate;
		CalculateReward();
		if(autoReward != null)
 			MessageControllerScript.Instance.OpenAutoReward(autoReward, calculatedReward, previousDateTime);
	}
	public void GetReward(){
		CalculateReward();
		previousDateTime = Client.Instance.GetServerTime();
		CampaignScript.Instance.SaveAutoFight(previousDateTime);
		OffGoldHeap();
	}
	private void CalculateReward(){
		if(autoReward != null)
			calculatedReward = autoReward.GetCaculateReward(FunctionHelp.CalculateCountTact(previousDateTime)); 
	}
	public void OnOpenSheet(){
		if(autoReward != null){
			previousDateTime = CampaignScript.Instance.GetAutoFightPreviousDate;
			CheckSprite();
		}
	}
	public void OnCloseSheet(){ Timer.StopTimer(timerChangeSprite); }
	private void CheckSprite(){
    	int tact = FunctionHelp.CalculateCountTact(previousDateTime);
    	if((tact >= 2) && (imageHeap.enabled == false)){
    		imageHeap.enabled = true;
			imageGoldRectTransform.DOScale(Vector2.one, 0.25f);
		}
    	Debug.Log("previousDateTime: " + previousDateTime.ToString() + " and tact = " + tact.ToString());
    	imageHeap.sprite = listSpriteGoldHeap.GetSprite(tact);
    	timerChangeSprite = Timer.StartTimer(5f, CheckSprite);
	}
	void OffGoldHeap(){ imageGoldRectTransform.DOScale(Vector2.zero, 0.25f).OnComplete(OffSprite); }
	void OffSprite(){ imageHeap.enabled = false; }
	GameTimer timerChangeSprite;
    TimerScript Timer;
    [SerializeField] private ListSpriteDependFromCount listSpriteGoldHeap = new ListSpriteDependFromCount();
}
