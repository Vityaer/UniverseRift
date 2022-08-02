using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission : ICloneable{
	[SerializeField] protected string name;
	public TypeLocation location;
	[Header("Enemy")]
	[SerializeField] protected List<MissionEnemy> _listEnemy = new List<MissionEnemy>();

	public string Name{get => name; set => name = value;}
	public List<MissionEnemy> listEnemy{get => _listEnemy;}
	[Header("Win reward")]
	[SerializeField] protected Reward winReward;
	public Reward WinReward{get => winReward;}
	
	public object Clone(){
        return new Mission  { 	Name = this.Name,
        							 	_listEnemy = this.listEnemy,
        								location = this.location,
        								winReward = (Reward) this.WinReward.Clone()
        							};				
    }
    public virtual void OnFinishFight(FightResult fightResult){
    	if(fightResult == FightResult.Win){
    		MessageControllerScript.Instance.OpenWin(winReward);
    	}else{
    		MessageControllerScript.Instance.OpenDefeat(null);
    	}
    } 
}