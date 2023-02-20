using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Custom ScriptableObject/Item", order = 53)]
[System.Serializable]
public class ItemsList : ScriptableObject
{
	[Header("Forge")]
	[Header("Weapons")]
	[SerializeField] private List<Item> weapons = new List<Item>();
	[Header("Boots")]
	[SerializeField] private List<Item> boots = new List<Item>();
	[Header("Armors")]
	[SerializeField] private List<Item> armors = new List<Item>();
	[Header("Amulet")]
	[SerializeField] private List<Item> amulets = new List<Item>();
	[Header("Artifact")]
	[SerializeField] private List<Item> artifacts = new List<Item>();

	public Item GetItem(int ID)
	{
		Item result = GetItemFromList( ID );
		if(result == null) Debug.Log("не нашли такого предмета "+ ID.ToString()); 
		return result;
	}

	private Item GetItemFromList(int ID)
	{
		Item result = null;
		int numList = ID / 100;
		switch(numList){
			case 1:
				result = weapons.Find(x => (x.ID == ID));
				break;
			case 2:
				result = boots.Find(x => (x.ID == ID));
				break;
			case 3:
				result = armors.Find(x => (x.ID == ID));
				break;
			case 6:
				result = amulets.Find(x => (x.ID == ID));
				break;
		}
		if(result == null) result = FindItemWithoutID(ID);
		return result;
	}

	private Item FindItemWithoutID(int ID)
	{
		Item result = null;
		int numList = ID / 100;
		int num = ID - 100 * numList;
		switch(numList){
			case 1:
				result = weapons[num];
				break;
			case 2:
				result = boots[num];
				break;
			case 3:
				result = armors[num];
				break;
			case 6:
				result = amulets[num];
				break;
		}
		return result;
	}

	public Item GetArtifact(int ID)
	{
		return artifacts.Find(x => (x.ID == ID));
	}
}
