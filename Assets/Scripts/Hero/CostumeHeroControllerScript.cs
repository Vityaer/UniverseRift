using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CostumeHeroControllerScript{
    public List<Item> items = new List<Item>();

//API
    public void TakeOn(Item newItem){
    	bool  flagFind = false;
    	for(int i=0; i<items.Count; i++){
    		if(items[i].Type == newItem.Type){
    			items[i] = newItem;
    			flagFind = true;
    			break;
    		}
    	}
    	if(flagFind == false){
    		items.Add(newItem);
    	}
    }
    public void TakeOff(Item item){
    	items.Remove(item);
    }
    public void TakeOffAll(){
    	items.Clear();
    }
    public Item GetItem(TypeItem typeItem){
    	Item result = null;
    	for(int i=0; i<items.Count; i++){
    		if(items[i].Type == typeItem){
    			result = items[i];
    			break;
    		}
    	}
    	return result;
    }
    public float GetBonus(TypeCharacteristic type){
    	float result = 0f;
    	foreach(Item item in items)
			result += item.GetBonus(type); 
		return result;
    }
    private static ItemsList itemsList;
    public void SetData(List<int> listIDItems){
        if(itemsList == null) itemsList = Resources.Load<ItemsList>("Items/ListItems"); 
        foreach(int ID in listIDItems){
            items.Add(itemsList.GetItem(ID));
        }
    }
    public CostumeHeroControllerScript Clone(){
        return new CostumeHeroControllerScript(this.items);
    }
    public CostumeHeroControllerScript(List<Item> newItems){
        for(int i = 0; i < newItems.Count; i++){
            this.items.Add((Item) newItems[i].Clone());
        }
    }
    public CostumeHeroControllerScript(){}
}
