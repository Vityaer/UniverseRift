using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BoxAllRewards : BasePanelScript
{
    public RewardUIController rewardController;
    private static BoxAllRewards instance;
    public static BoxAllRewards Instance{ get => instance;}
    
    void Awake()
    {
        instance = this;
    }
    
    public void ShowAll(Reward reward)
    {
        if(reward != null) rewardController.ShowAllReward(reward);
    	Open();
    }


}
