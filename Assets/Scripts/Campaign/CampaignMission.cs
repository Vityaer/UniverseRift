using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[System.Serializable]
public class CampaignMission : Mission, ICloneable{


	[Header("Auto fight reward")]
	[SerializeField] private AutoReward autoFightReward;
	public AutoReward AutoFightReward{get => autoFightReward;}
	public object Clone(){
        return new CampaignMission  { 	Name = this.Name,
        							 	_listEnemy = this.listEnemy,
        							 	winReward     = (Reward) this.WinReward.Clone(),
        							 	autoFightReward  = this.autoFightReward,
        								location = this.location
        							};				
    }
}
