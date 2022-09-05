using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObjectSave;
using System;
public class EventAgentControllerScript : Building{

	[Header("Data")]
	private int maxCount = 3000;
	public int count = 0, sumReward = 0;
	public List<Reward> listRewards = new List<Reward>();
	private List<int> idReceivedReward = new List<int>();
	[Header("UI")]
	public List<EventAgentRewardScript> listAgentRewardPanel = new List<EventAgentRewardScript>();
	public Transform contentScrollRect;
	public  ItemSliderControllerScript miniSliderAmount;
	public EventAgentMainSlider mainSliderController;
	public MyScrollRect scrollRectController;
	private static string SUM_RECEIVIED_REWARD = "SumReward";
	public void OnGetMonet(Resource res){
		count = res.ConvertToInt();
		miniSliderAmount.SetAmount(OverMonet(), 100);
	}
	protected override void OnStart(){
		PlayerScript.Instance.RegisterOnChangeResource(OnGetMonet, TypeResource.EventAgentMonet);
	}
	SimpleBuildingSave progressObjectSave;
	protected override void OnLoadGame(){
		Debug.Log("onloadGame");
		progressObjectSave = PlayerScript.GetPlayerSave.allRequirement.eventAgentProgress;
		this.sumReward = progressObjectSave.GetRecordInt(SUM_RECEIVIED_REWARD);
		RepackReceivedReward(this.sumReward);
		EventAgentRewardStatus statusReward = EventAgentRewardStatus.Close;
		int countOpenLevel = GetOpenLevel();
		for(int i = 0; i < listRewards.Count; i++){
			if(i < countOpenLevel){
				if(idReceivedReward.Contains(i)){
					statusReward = EventAgentRewardStatus.Received;
				}else{
					statusReward = EventAgentRewardStatus.Open;
				}
			}else{
				statusReward = EventAgentRewardStatus.Close;
			}
			listAgentRewardPanel[i].SetStatus(statusReward);
		}
		OnGetMonet(PlayerScript.Instance.GetResource(TypeResource.EventAgentMonet));
		mainSliderController.SetValue(GetNewValueSlider(this.count)); 
	}
	private static char SYMBOL_1 = '1';
	private void RepackReceivedReward(int sum){
		string binaryCode = Convert.ToString(sum, 2);
		for(int i = 0; i < binaryCode.Length; i++)
			if(binaryCode[i].Equals(SYMBOL_1))
				idReceivedReward.Add(i);
	}
	private int GetOpenLevel(){return count / 100;}
	float GetNewValueSlider(int count){return (float)(count /(float) (listRewards.Count * 100));}

	bool isFillData = false;
	protected override void OpenPage(){
		mainSliderController.SetNewValueWithAnim(GetNewValueSlider(this.count));
		if(isFillData == false){
			for(int i = 0; i < listRewards.Count; i++)
				listAgentRewardPanel[i].SetData(listRewards[i]);
			isFillData = true;
		}
	}
	
	public int GetMaxLevelReceivedReward(){
		int result = 0;
		for(int i = 0; i < idReceivedReward.Count; i++){
			if(idReceivedReward[i] > result){
				result = idReceivedReward[i];
			}
		}
		return result;
	}
	public int OverMonet(){
		int maxLevel = GetMaxLevelReceivedReward();
		return count - maxLevel * 100;
	}
//API
	public void OnGetReward(int ID){
		idReceivedReward.Add(ID);
		sumReward += (int) Math.Pow(2, ID);
		SaveData();
	}

//Save
	public void SaveData(){
		progressObjectSave.SetRecordInt(SUM_RECEIVIED_REWARD, sumReward);
		PlayerScript.Instance.SaveGame();
	}	
	void Awake(){
		instance = this;
	}
	private static EventAgentControllerScript instance;
	public static EventAgentControllerScript Instance{get => instance;}
}
