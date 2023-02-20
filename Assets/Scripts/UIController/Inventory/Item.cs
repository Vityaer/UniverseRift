using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



[System.Serializable]
public class Item : PlayerObject, VisualAPI
{
	private static Sprite[] spriteAtlas;   
    private static ItemsList itemsList;
	public override Sprite Image{ 
		get{ 	
			if(sprite == null || !Name.Equals(sprite.name))
			{
				if(spriteAtlas == null) spriteAtlas = Resources.LoadAll<Sprite>("Items/Items");
				LoadData();
				for(int i=0; i < spriteAtlas.Length; i++){
					if(Name.Equals(spriteAtlas[i].name)){
						sprite = spriteAtlas[i];
						break;
					}
				}
			}
			return sprite;
		}
	}
	
	[SerializeField]private TypeItem type;
	[SerializeField]public SetItems set;
	[SerializeField] public List<Bonus> ListBonuses;
	[SerializeField] private Rare rare;

	public TypeItem Type{get => type;}
	public SetItems Set{get => set;}
	public Rare GetRare{get => rare;}

	public void LoadData()
	{
		if(ID == 0) {
			Debug.Log("ID item = 0");
			id = 101;
		}
		sprite = null;
		Item loadedItem = GetItem(ID);
		name = loadedItem.Name;
		this.type = loadedItem.Type;
		this.set = loadedItem.Set;
		this.ListBonuses = loadedItem.ListBonuses;
	}

	public static Item GetItem(int ID)
	{
    	if(itemsList == null) itemsList = Resources.Load<ItemsList>("Items/ListItems"); 
		return itemsList.GetItem(ID);
	}
//API

	public string GetTextBonuses()
	{
		string result = string.Empty;
		foreach (Bonus bonus in ListBonuses)
			result = string.Concat(result, GetText(bonus.Count, Name.ToString()));
		return result;
	}

	private string GetText(float bonus, string who)
	{
		string result = "";
		result = string.Concat(result, (bonus > 0) ? "<color=green>+" : "<color=red>", Math.Round(bonus, 1).ToString(), "</color> ", who,"\n");
		return result;
	}

	public float GetBonus(TypeCharacteristic typeBonus)
	{
		float result = 0;
		Bonus bonus = ListBonuses.Find(x => x.Name == typeBonus);
		if(bonus != null) result = bonus.Count;
		return result;
	}

//Visial API
	public override void ClickOnItem()
	{
		InventoryControllerScript.Instance.OpenInfoItem(this);
	}

	public override void SetUI(ThingUIScript UI)
	{
		this.UI = UI;
		UpdateUI();
	}

	public override void UpdateUI()
	{
		UI?.UpdateUI(Image, rare, Amount);
	}	

//Constructors
	public Item(int ID, int amount = 1)
	{
		Amount = amount;
		base.id = ID;
		LoadData();
	}	

	public Item()
	{
		base.id = 0;
	}

	public override BaseObject Clone()
	{
		return new Item(this.ID, this.Amount);
	}

//Operators
	public static Item operator* (Item item, int k){
		return new Item(item.ID, k);
	}
}




public enum TypeItem{
		Weapon,
		Shield,
		Helmet,
		Mittens,
		Armor,
		Boots,
		Amulet,
		Artifact,
		Stone,
		Resource,
		Splinter}

public enum SetItems
{
	Rubbish = 0,
	Pupil = 1,
	Peasant = 2,
	Militiaman = 3,
	Monk = 4,
	Warrior = 5,
	Feller = 6,
	Soldier = 7,
	Minotaur = 8,
	Demon = 9,
	Druid = 10,
	Obedient = 11,
	Devil = 12,
	Destiny = 13,
	Archangel = 14,
	Titan = 15,
	God = 16
}