using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public partial class ActionEffect{
	public TypeEffect typeAction;
	public EffectSimpleAction simpleAction;
	public EffectBuff effectBuff;
	public EffectDebuff effectDebuff;
	public EffectDots effectDots;
	public EffectChangeCharacteristic effectChangeCharacteristic;
	public EffectStatus effectStatus;
	public EffectMark effectMark;
	public EffectOther effectOther;
	public EffectSpecial effectSpecial;
	
	public SideTarget sideTarget;
	public TypeSelect typeSelect        = TypeSelect.Order;
	public int        countTarget;
	public RecalculateMethodTarget recalculateTarget;
	public float chance = 100f;
	public float      amount; 
	public TypeNumber typeNumber;
	public DropOrSum RepeatCall;
	public List<Round> rounds = new List<Round>();
	[Space]
	private List<HeroControllerScript> listTarget = new List<HeroControllerScript>();
	private HeroControllerScript master;
	public HeroControllerScript Master{get => master; set => master = value;}
	public void SetNewTarget(List<HeroControllerScript> listTarget){
		this.listTarget.Clear();
		if((listTarget.Count == 0) || (recalculateTarget != RecalculateMethodTarget.OldTargets)){
			Side side = master.side;
			if(sideTarget != SideTarget.I){
				if(sideTarget == SideTarget.Friend) {if(side == Side.Left){side = Side.Right;}else{side = Side.Left;}}
				FightControllerScript.Instance.ChooseEnemies(side, countTarget, this.listTarget, typeSelect);
			}else{
				this.listTarget.Add(master);		
			}
			switch (recalculateTarget){
				case RecalculateMethodTarget.AddTargets:
					listTarget.AddRange(this.listTarget);
					break;
				case RecalculateMethodTarget.NewTargets:
					listTarget = this.listTarget;
					break;
				case RecalculateMethodTarget.OldTargets:
					listTarget = this.listTarget;
					break;
			}
		}else{
			this.listTarget = listTarget;
		}
	} 

	public void ExecuteAction(){
		switch(typeAction){
			case TypeEffect.SimpleAction:
				ExecuteSimpleAction();
				break;
			case TypeEffect.Buff:
				ExecuteBuff();
				break;
			case TypeEffect.Debuff:
				ExecuteDebuff();
				break;	
			case TypeEffect.Dots:
				ExecuteDots();
				break;
			case TypeEffect.Mark:
				ExecuteMark();
				break;
			case TypeEffect.ChangeCharacteristic:
				ExecuteChangeCharacteristic();
				break;
			case TypeEffect.StatusHero:
				ExecuteStatusHero();
				break;
			case TypeEffect.Special:
				ExecuteSpecial();
				break;
			case TypeEffect.Other:
				ExecuteOther();
				break;							
		}
	}

	public void GetListForSpell(List<HeroControllerScript> listTarget){
		SetNewTarget(listTarget);
		if(sideTarget != SideTarget.I){
			master.listTarget = this.listTarget;
		}
	} 

}



public enum TypeEffect{
	SimpleAction = 0,
	Buff = 1,
	Dots = 2,
	Mark = 3,
	ChangeCharacteristic = 4,
	StatusHero = 5,
	Special = 6,
	Debuff = 7,
	Other = 20	
}


public enum TypeSelect{
	Random = 0,
	Order = 1,
	FirstLine = 2,
	SecondLine = 3,
	ID = 4,
	LeastHP = 5,
	GreatestHP = 6,
	LeastAttack = 7,
	GreatestAttack = 8,
	LeastArmor = 9,
	GreatestArmor = 10,
	LeastInitiative = 11,
	GreatestInitiative = 12,
	IsAlive = 13,
	IsNotAlive = 14,
	All = 15,
	People = 16,
	Elf = 17,
    Undead = 18,
    Daemon = 19,
    God = 20,
    DarkGod = 28,
    Warrior = 21,
	Wizard = 22,
	Archer = 23,
	Pastor = 24,
	Slayer = 25,
	Tank = 26,
	Support = 27,
	Select = 100
	}

    public enum SideTarget{
	Enemy,
	Friend,
	I,
	All}

	public enum RecalculateMethodTarget{
		NewTargets,
		AddTargets,
		OldTargets,
		SelectFromOldTargets
	}

	public enum DropOrSum{
		Drop,
		Sum
	}