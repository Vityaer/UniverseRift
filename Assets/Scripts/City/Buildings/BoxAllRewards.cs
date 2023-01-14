using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BoxAllRewards : BasePanelScript{
    public RewardUIControllerScript rewardController;
    public void ShowAll(Reward reward){
        if(reward != null) rewardController.ShowAllReward(reward);
    	Open();
    }

    private static BoxAllRewards instance;
    public static BoxAllRewards Instance{ get => instance;}
    void Awake(){
    	instance = this;
    }
}
