using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Task : ICloneable{
	public string name;
	public int ID;
	public int rating;
	public List<InfoHero> heroes = new List<InfoHero>();
	public int requireHour;
	private DateTime _requireTime = new DateTime();
	public DateTime requireTime{get 
						{if(_requireTime.Hour == 0){_requireTime = new DateTime().AddHours(requireHour);}
							 return _requireTime;} set {_requireTime = value;}}
	private DateTime _timeStartTask = new DateTime();
	public DateTime timeStartTask{
								get {
									if(DateTime.Equals(_timeStartTask, new DateTime())){
										if(strTimeStartTask != 0){
											_timeStartTask = DateTime.FromBinary(strTimeStartTask);
										}
									}
									return _timeStartTask;
								}
								set{
									if(DateTime.Equals(_timeStartTask, new DateTime())){
										_timeStartTask = value;
										strTimeStartTask = _timeStartTask.ToBinary();
									}	
								}
							}

	public long strTimeStartTask;
	public StatusTask status = StatusTask.NotStart;
	[HideInInspector][SerializeField]private Resource reward;
	public Resource Reward{get => reward; set => reward = value;}
	public void Start(){
		status           = StatusTask.InWork;
		timeStartTask    = DateTime.Now;
	}
//API
	public TimeSpan GetRequireTime(){
		return new TimeSpan(_requireTime.Hour, 0, 0);
	}
	public void GetReward(){
		PlayerScript.Instance.AddResource( reward );
	}

	public object Clone(){
	    return new Task  { 	ID = this.ID,
							name = this.name,
						 	rating = this.rating,
						 	heroes     = this.heroes,
						 	requireHour  = this.requireHour,
						 	strTimeStartTask = this.strTimeStartTask,
						 	reward = this.reward,
						 	status = this.status
						};				
    }
}

public enum StatusTask{
	NotStart,
	InWork,
	Done
}
public enum TypeTask{
	Simple,
	Special
}