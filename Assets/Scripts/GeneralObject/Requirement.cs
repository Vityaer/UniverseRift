using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Requirement{
	[Header("Info")]
	[SerializeField] protected int id;
	public TypeRequirement type = TypeRequirement.GetLevel;
	public string description;
	[SerializeField]protected List<RequirementStage> stages = new List<RequirementStage>();
	public ProgressType progressType;

	[SerializeField]protected BigDigit progress = new BigDigit(0, 0);
	[SerializeField]protected int currentStage = 0;

	public int ID{get => id;}
	public int CountStage{get => stages.Count;}
	public BigDigit Progress{get => progress;}
	public int CurrentStage{get => currentStage;}
	bool isComplete = false;
	public bool IsComplete{get => isComplete;}
	public void AddProgress(BigDigit amount){
		if(isComplete == false){
			switch(progressType){
				case ProgressType.StorageAmount:
					progress.Add(amount);
					break;
				case ProgressType.MaxAmount:
					if(progress > amount)
						progress = amount;
					break;
				case ProgressType.CurrentAmount:
					progress = amount;
					break;	
			}
			if(progress.CheckCount(GetRequireFinishCount())){
				progress = new BigDigit(GetRequireFinishCount().Count, GetRequireFinishCount().E10);	
				isComplete = true;
			}	
		}
	}
	public void SetProgress(int newCurrentStage, BigDigit newProgress){
		this.currentStage = newCurrentStage;
		this.progress     = newProgress; 
		if(progress.CheckCount(GetRequireFinishCount())){
			isComplete = true;
		}
	}
	private BigDigit GetRequireFinishCount(){return stages[stages.Count - 1].RequireCount;}
	public bool CheckCount(){return progress.CheckCount(GetRequireCount()); }
	public BigDigit GetRequireCount(){ return stages[currentStage].RequireCount; }
	public void GetReward(){PlayerScript.Instance.AddReward(stages[currentStage].reward); currentStage++; }
	public Reward GetRewardInfo(){
		return stages[currentStage < CountStage ? currentStage : CountStage - 1].reward.Clone();
	}

	public Requirement(int ID, int currentStage, BigDigit progress){
		this.id = ID;
		this.currentStage = currentStage;
		this.progress = progress;
	}
	public Requirement Clone(){
		return new Requirement(this.ID,this.currentStage, this.progress);
	}
	public void ClearProgress(){
		currentStage = 0;
		progress = new BigDigit(0, 0);
	}
}
public enum TypeRequirement{
	GetLevel,
	DoneChapter,
	DoneMission,
	GetHeroes,
	GetHeroesWithLevel,
	GetHeroesWithRating,
	GetHeroesCount,
	SimpleSpin,
	SpecialSpin,
	SynthesItem,
	SynthesCount,
	BuyItem,
	SpendResource,
	BuyItemCount,
	DestroyHeroCount,
	CountWin,
	CountDefeat,
	CountPointsOnSimpleArena,
	CountPointsOnTournament,
	RaceHireCount,
	SimpleHireCount,
	SpecialHireCount,
	TryCompleteChallangeTower,
	CompleteChallengeTower,
	SendFriendHeart
}
public enum TypeReward{
	Resource,
	Item,
	Hero,
	Splinter
}
public enum ProgressType{
	StorageAmount,
	MaxAmount,
	CurrentAmount
}
[System.Serializable]
public class RequirementStage{
	[Header("Requirement")]
	[SerializeField] private BigDigit requireCount;
	[Header("Reward")]
	public Reward reward;
	public BigDigit RequireCount{ get =>  requireCount;}
}
