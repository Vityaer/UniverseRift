using Models.Heroes.Characteristics;
using System.Collections.Generic;
using UIController.Inventory;
using UnityEngine;


[System.Serializable]
public class CostumeHeroController
{
    public List<Item> items = new List<Item>();
    private static ItemsList itemsList;

    //API
    public void TakeOn(Item newItem)
    {
        bool flagFind = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Type == newItem.Type)
            {
                items[i] = newItem;
                flagFind = true;
                break;
            }
        }
        if (flagFind == false)
        {
            items.Add(newItem);
        }
    }
    public void TakeOff(Item item)
    {
        items.Remove(item);
    }
    public void TakeOffAll()
    {
        items.Clear();
    }
    public Item GetItem(string typeItem)
    {
        Item result = null;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Type == typeItem)
            {
                result = items[i];
                break;
            }
        }
        return result;
    }
    public float GetBonus(TypeCharacteristic type)
    {
        float result = 0f;
        foreach (Item item in items)
            result += item.GetBonus(type);
        return result;
    }
    public void SetData(List<string> listIDItems)
    {
        if (itemsList == null) itemsList = Resources.Load<ItemsList>("Items/ListItems");
        foreach (var ID in listIDItems)
        {
            items.Add(itemsList.GetItem(ID));
        }
    }
    public CostumeHeroController Clone()
    {
        return new CostumeHeroController(this.items);
    }
    public CostumeHeroController(List<Item> newItems)
    {
        for (int i = 0; i < newItems.Count; i++)
        {
            this.items.Add((Item)newItems[i].Clone());
        }
    }
    public CostumeHeroController() { }
}
