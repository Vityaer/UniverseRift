using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ObjectSave;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
public class DailyControllerScript : Building {

	private const char SYMBOL_1 = '1';
	
	private List<int> idReceivedReward = new List<int>();

	private SimpleBuildingSave dailyReward = null;
	[OdinSerialize] private List<MarketProduct> listRewards = new List<MarketProduct>();
	public List<DailyRewardScript> dailyRewardUI = new List<DailyRewardScript>();
	public SliderTimeScript sliderTime;
	public MyScrollRect scrollRectController;
	private static string SUM_RECEIVIED_REWARD = "SumReward";
	private static string ID_CURRENT_REWARD = "IDCurReward";
	private static string DATA_LAST_CHECK = "DataLastCheck";
	int sumReward = 0, IDCurReward = 0;
	DateTime dateLastCheck;
	TimeSpan timeForNextOpenReward = new TimeSpan(1, 0, 0, 0);

	void Awake(){
		instance = this;
	}

	protected override void OnLoadGame(){
		dailyReward = PlayerScript.GetCitySave.dailyReward;
		this.sumReward     = dailyReward.GetRecordInt(SUM_RECEIVIED_REWARD);
		this.IDCurReward   = dailyReward.GetRecordInt(ID_CURRENT_REWARD);
		this.dateLastCheck = dailyReward.GetRecordDate(DATA_LAST_CHECK);  
		RepackReceivedReward(this.sumReward);
		TimeControllerSystem.Instance.RegisterOnNewCycle(OpenNextReward, CycleRecover.Day);
		sliderTime.SetData(dateLastCheck, timeForNextOpenReward);
		SetAllRewardsAvailable();
	}

	private void SetAllRewardsAvailable(){
		for(int i = 0; i <= IDCurReward; i++){
			dailyRewardUI[i].SetStatus(EventAgentRewardStatus.Open);
		}
		for(int i = 0; i < idReceivedReward.Count; i++){
			dailyRewardUI[idReceivedReward[i]].SetStatus(EventAgentRewardStatus.Received);
		}
	}

	public void OpenNextReward(){
		IDCurReward += 1;
		dateLastCheck = TimeControllerSystem.Instance.GetDayCycle;
		dailyReward.SetRecordInt(ID_CURRENT_REWARD, IDCurReward);
		dailyReward.SetRecordDate(DATA_LAST_CHECK, dateLastCheck);
		SaveData();
	}


	private void RepackReceivedReward(int sum){
		string binaryCode = Convert.ToString(sum, 2);

		for(int i = 0; i < binaryCode.Length; i++)
			if(binaryCode[i].Equals(SYMBOL_1))
				idReceivedReward.Add(i);
	}
//API
	public void OnGetReward(int ID){
		Debug.Log("Id: "+ ID.ToString());
		idReceivedReward.Add(ID);
		sumReward += (int) Math.Pow(2, ID);
		dailyReward.SetRecordInt(SUM_RECEIVIED_REWARD, sumReward);
		SaveData();
	}
	bool isFillData = false;
	protected override void OpenPage(){
		if(isFillData == false){
			for(int i = 0; i < listRewards.Count; i++){
				dailyRewardUI[i].SetData(listRewards[i]);
			}
			isFillData = true;
		}
	}
	[Button] public void AddResource(){ listRewards.Add(new MarketProduct<Resource>()); }
	[Button] public void AddSplinter(){ listRewards.Add(new MarketProduct<Splinter>()); }
	[Button] public void AddItem()    { listRewards.Add(new MarketProduct<Item>()); }

//Save
	public void SaveData(){
		PlayerScript.Instance.SaveGame();
	}


	private static DailyControllerScript instance;
	public static DailyControllerScript Instance{get => instance;}
}