using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionWithSmashReward : Mission{
	[SerializeField] protected Reward smashReward;
	public Reward SmashReward{get => smashReward;}
	public MissionWithSmashReward Clone(){
		 return new MissionWithSmashReward  { 	Name = this.Name,
        							 	ListEnemy = this.listEnemy,
        							 	WinReward     = (Reward) this.WinReward.Clone(),
        							 	smashReward     = (Reward) this.smashReward.Clone(),
        								Location = this.Location
        							};
	}
}
