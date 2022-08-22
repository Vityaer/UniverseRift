using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum TypeResource{
	Gold = 0,
	Diamond = 1,
	ContinuumStone = 2,
	SimpleHireCard = 3,
	SpecialHireCard = 4,
	ForceStone = 5,
	TicketOnArena = 6,
	GenePet = 7,
	DopePet = 8,
	CoinFortune = 9,
	Exp = 10,
	SimpleTask = 11,
	SpecialTask = 12,
	TicketGrievance = 13,
	SimpleStoneReplacement = 14,
	RaceHireCard = 15,
	FriendHeart = 16,
	MineStone = 17,
	RedDust = 18,
	EventAgentMonet = 19,
	FeastCoint = 20,
	VoyageToken = 21
}


[System.Serializable]
public class Resource : BaseObject, ICloneable, VisualAPI{
	public TypeResource Name;
	[SerializeField] protected BigDigit amount;
	public BigDigit Amount{get => amount; set => amount = value;}
	public float Count{get => Amount.Count; set => Amount.Count = value;}
	public int E10{get => Amount.E10; set => Amount.E10 = value;}
	public Resource(){
		Name  = TypeResource.Gold;
		Amount = new BigDigit();
	}
	public Resource(TypeResource name, float count = 0f, int e10 = 0){
		this.Name  = name;
		Amount = new BigDigit(count, e10);
	}
	public Resource(TypeResource name, BigDigit amount){
		this.Name = name;
		this.Amount = amount;
	}
	
//API
	public override string GetTextAmount(){return Amount.ToString();}
	public override string ToString(){ return Amount.ToString(); }
	public bool CheckCount(int count, int e10){ return Amount.CheckCount(count, e10);}
	public bool CheckCount(Resource res){ return Amount.CheckCount(res.Count, res.E10); }
	public void AddResource(float count, float e10 = 0){ Amount.Add(count, e10); }
	public void AddResource(Resource res){ this.Amount.Add(res.Count, res.E10); }
	public void SubtractResource(float count, float e10 = 0){ Amount.Subtract(count, e10); }
	public void SubtractResource(Resource res){ this.Amount.Subtract(res.Count, res.E10); }
	public void Clear(){ Amount.Clear(); }

	public void ClearUI(){
		this.UI = null;
	}
	public VisualAPI GetVisual(){
		return (this as VisualAPI);
	}

//Operators
	public static Resource operator* (Resource res, float k){
		Resource result = new Resource(res.Name, Mathf.Ceil(res.Count * k), res.E10);
		return result;
	}
	public static bool operator< (Resource a, Resource b){ return b.CheckCount(a); }		
	public static bool operator> (Resource a, Resource b){ return a.CheckCount(b); }		
//Image
	private static Sprite[] spriteAtlas;   
	public Sprite Image{ 
							get{ 
								if(sprite == null){
									if(spriteAtlas == null) spriteAtlas = Resources.LoadAll<Sprite>("UI/GameImageResource/Resources");
									for(int i=0; i < spriteAtlas.Length; i++){
										if((Name.ToString()).Equals(spriteAtlas[i].name)){
											sprite = spriteAtlas[i];
											break;
										}
									}
								}
								return sprite;
							}
						}
	public object Clone(){
        return new Resource  { 
        	Name = this.Name,
        	Amount = this.Amount
        };
    }   
//UI
	public override string GetName(){return Name.ToString();}
	public void ClickOnItem(){ InventoryControllerScript.Instance.OpenInfoItem(this); }
	public void SetUI(ThingUIScript UI){
		this.UI = UI;
		UpdateUI();
	}
	public void UpdateUI(){
		UI?.UpdateUI(Image, Rare.C, Amount.ToString());
	}
	public int ConvertToInt(){
		return (int) (Count * Mathf.Pow(10, E10));
	}

}

public class ObserverResource{
	public TypeResource typeResource;
	public Action<Resource> delObserverResource;
	public ObserverResource(TypeResource type){
		typeResource = type;
	}

	public void RegisterOnChangeResource(Action<Resource> d){
		delObserverResource += d;
	}
	public void UnRegisterOnChangeResource(Action<Resource> d){
		delObserverResource -= d;
	}
	public void ChangeResource(Resource res){
		if(delObserverResource != null)
			delObserverResource(res);
	}
}
