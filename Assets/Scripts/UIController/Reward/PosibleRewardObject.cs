using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
[System.Serializable]
public class PosibleRewardObject{
	[SerializeField]protected float posibility = 100f;
	public float Posibility{get => posibility;}
}




[System.Serializable]
public class PosibleRewardResource: PosibleRewardObject{
	public TypeResource subject = TypeResource.Diamond;
	public Resource GetResource{get{return new Resource(subject); }}
}
[System.Serializable]
public class PosibleRewardItem: PosibleRewardObject{
	public ItemName ID = ItemName.Stick;
	public Item GetItem{get{return new Item($"{ID}", 1);}}
}
[System.Serializable]
public class PosibleRewardSplinter: PosibleRewardObject{
	public SplinterName ID = SplinterName.OneStarPeople;
	public SplinterModel GetSplinter{get{ return new SplinterModel($"{ID}", 1); }}
	public SplinterName GetID{get => ID;}

}
public enum ItemName{
	Stick = 101,
	Pole = 102,
	RustySword = 103,
	OrdinarySword = 104,
	WizardStuff = 105,
	Axe = 106,
	Mace = 107,
	hammer = 108,
	PupilBoot = 201,
	PeasantBoot = 202,
	MilitiamanBoot = 203

}
public enum SplinterName{
	OneStarPeople = 11,
	OneStarElf = 12
}