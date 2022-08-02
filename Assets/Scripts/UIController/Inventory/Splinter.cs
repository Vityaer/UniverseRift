using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Splinter : PlayerObject{
	public TypeSplinter typeSplinter;
	public RaceSpliter race;
	[SerializeField] private int requireAmount;
	[Header("rewards")]
	public PosibleReward reward = new PosibleReward();
	public int CountReward{get => reward.posibilityObjectRewards.Count;}
	public Rare rare = Rare.C;	

	public Rare GetRare{get => rare;}
	public int RequireAmount{
		get{ 
			if(requireAmount <= 0) requireAmount = CalculateRequire();
			return requireAmount;
		}
	}	
	private int CalculateRequire(){return (20 + ((int)this.rare * 10));}
	public override Sprite Image {
		get{ 
			if(sprite == null){
				switch(typeSplinter){
					case TypeSplinter.Hero:
						if(CountReward > 1){
							sprite = SystemSprites.Instance.GetSprite( GetSpriteName() );
						}else{
							GetDefaultData();
						}
						break;
				}
			}
			return sprite;
		}
	}
	public void GetReward(int countReward){
		for(int i = 0; i < countReward; i++){
			switch(typeSplinter){
				case TypeSplinter.Hero:
					AddHero(GetRandomHero());
					break;
			}
		}
		Amount -= requireAmount * countReward;
		if(Amount > 0){
			UpdateUI();
		}else{
			ClearData();
		}
		InventoryControllerScript.Instance.Refresh();
	}
	public void SetAmount(int amount){
		Amount = amount;
		requireAmount = CalculateRequire();
	}
	public void AddAmount(int count){Debug.Log("Before: " + Amount.ToString()); Amount = Amount + count; Debug.Log("After: " + Amount.ToString()); }
	public bool IsCanUse{ get => (Amount >= RequireAmount);}
	public string GetTextType{get => typeSplinter.ToString();}
	public string GetTextDescription{get => string.Empty;}
//Constructors
	public Splinter(int ID, int count = 0){
		this.id = ID;
		GetDefaultData();
		Amount = count > 0 ? count : requireAmount;
	}
	public Splinter(InfoHero hero){
		this.typeSplinter = TypeSplinter.Hero;
		this.sprite = hero.generalInfo.ImageHero;
		this.rare = hero.generalInfo.rare;
		this.id = hero.generalInfo.idHero;
		reward = new PosibleReward(); 
		reward.Add(ID);
		requireAmount = CalculateRequire();
	}
	private void GetDefaultData(){
		Splinter data = SplinterSystem.Instance.GetSplinter(this.ID);
		this.typeSplinter = data.typeSplinter;
		this.sprite = data.Image;
		this.reward = data.reward;
		this.rare = data.rare;
		this.requireAmount = data.RequireAmount;
	}
	public override BaseObject Clone(){return new Splinter(this.ID, Amount);}
//Visual API
	public override void ClickOnItem(){ InventoryControllerScript.Instance.OpenInfoItem(this); }
	public override void SetUI(ThingUIScript UI){
		this.UI = UI;
		UpdateUI();
	}
	public override void UpdateUI(){
		UI?.UpdateUI(Image, rare, GetTextAmount());
	}
	private void ClearData(){
		UI?.Clear();
		InventoryControllerScript.Instance.RemoveSplinter(this);
	}
	private SpriteName GetSpriteName(){
		SpriteName result = SpriteName.BaseSplinterHero;
		switch(typeSplinter){
			case TypeSplinter.Hero:
				result = SpriteName.BaseSplinterHero;
				break;
		}
		return result;
	}
//Rewards
	private InfoHero GetRandomHero(){
		int selectNumber = 0;
		if(CountReward > 1){
			float rand = UnityEngine.Random.Range(0, reward.GetAllSum);
			for(int i = 0; i < CountReward; i++){
				rand -= reward.posibilityObjectRewards[i].posibility;
				if(rand <= 0){
					selectNumber = i;
					break;
				}
			} 
		}
		Debug.Log("selectNumber: " + selectNumber);
		InfoHero hero = TavernScript.Instance.GetInfoHero(reward.posibilityObjectRewards[selectNumber].ID);
		return hero;
	}			
	private void AddHero(InfoHero newHero){
		if(newHero != null){
			newHero.generalInfo.Name = newHero.generalInfo.Name + " №" + Random.Range(0, 1000).ToString();
			MessageControllerScript.Instance.AddMessage("Новый герой! Это - " + newHero.generalInfo.Name);
			PlayerScript.Instance.AddHero(newHero);
		}else{
			Debug.Log("newHero null");
		}
	}
//Operators
	public static Splinter operator* (Splinter item, int k){
		return new Splinter(item.ID, k);
	}
}

public enum TypeSplinter{
		Hero,
		Artifact,
		Costume,
		Other}

public enum RaceSpliter{
	People,
	Elf,
    Undead,
    Mechanic,
    Inquisition,
    Demon,
    God,
    RandomAll,
    RandomWithoutGod}
    
[System.Serializable]
public class SplinerPosibilityObject{
	public int ID;
	public float posibility = 1f; 
	public SplinerPosibilityObject(int ID, float percent = 1f){
		this.ID = ID;
		this.posibility = percent;
	}
}  
[System.Serializable]
public class PosibleReward{
	public List<SplinerPosibilityObject> posibilityObjectRewards = new List<SplinerPosibilityObject>();
	float sumAll = 0;
	public float GetAllSum{ 
		get{
			if(sumAll > 0){ return sumAll;}else{
				for(int i = 0; i < posibilityObjectRewards.Count; i++) sumAll += posibilityObjectRewards[i].posibility;
				return sumAll;
			}

		}
	}
	public float PosibleNumObject(int num){
		if(sumAll <= 0f) sumAll = GetAllSum; 
		return (posibilityObjectRewards[num].posibility/sumAll * 100f);
	}
	public void Add(int ID, float percent = 1f){
		posibilityObjectRewards.Add(new SplinerPosibilityObject(ID, percent));
	}
} 