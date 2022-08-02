using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionEffect{
	public SideTarget sideTarget;
	public TypeProperties property;
	public string data;
}

public enum TypeProperties{
	ID,
	AllDebuff,
	AllBuff,
	CountWarrior,
	CountWizard,
	CountArcher,
	CountPastor,
	CountSlayer,
	CountTank,
	CountSupport,
	CountStun,
	CountPetrification,
	CountFreezing,
	CountAstral,
	CountDumb,
	CountPoison,
	CountBleeding,
	CountHellishMark,
	CountMelleeHeroes,
	CountNotMelleeHeroes,
	CountAliveHeroes,
	CountDeadHeroes,
	CountHeroesHPLess50,
	CountHeroesHPLess30,
	CountPeople,
	CountElf,
    CountUndead,
    CountMechanic,
    CountInquisition,
    CountDemon,
    CountGod,
	HellishMark}
