using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[System.Serializable]
public class Reward{
    [SerializeField] private ListResource resources = new ListResource();
    [SerializeField] private List<RewardItem> items = new List<RewardItem>();
    [SerializeField] private List<RewardSplinter> splinters = new List<RewardSplinter>();

	public int Count{
        get{
            if(generalList.Count == 0) PrepareRewardForShow();
            return generalList.Count;
        }
    }
    private List<BaseObject> generalList = new List<BaseObject>();
    public List<BaseObject> GetList{
        get{
            if(generalList.Count == 0) PrepareRewardForShow();
            return generalList;
        }
    }
	public Reward Clone(){
        ListResource listRes = (ListResource) resources.Clone();
        List<RewardItem> cloneItems = new List<RewardItem>();
        List<RewardSplinter> cloneSplinters = new List<RewardSplinter>();
        foreach(RewardItem item in this.items){ cloneItems.Add(item.Clone());}
        foreach(RewardSplinter splinter in this.splinters){ cloneSplinters.Add(splinter.Clone());}
        return new Reward(listRes, cloneItems, cloneSplinters);			
    }
    public Reward(ListResource resources, List<RewardItem> items, List<RewardSplinter> splinters){
        this.resources = resources;
        this.items = items;
        this.splinters = splinters;
    }
    public Reward(ListResource resources){
        this.resources = resources;
    }
    public Reward(){}
    private void PrepareRewardForShow(){
        generalList.Clear();
        for(int i = 0; i < resources.List.Count; i++) generalList.Add(resources.List[i]);
        generalList.AddRange(GetItems);
        generalList.AddRange(GetSplinters);

    }
    public ListResource GetListResource{get => resources;}
	public List<Item> GetItems{
        get{
            List<Item> result = new List<Item>();
            foreach(RewardItem rewardItem in items)
                result.Add(rewardItem.GetItem);
            return result;
        }
    }
	public List<Splinter> GetSplinters{
        get{
            List<Splinter> result = new List<Splinter>();
            foreach(RewardSplinter rewardSplinter in splinters)
                result.Add(rewardSplinter.GetSplinter);
            return result;
        }
    }
    public void AddResources(ListResource newResources){resources.AddResource(newResources);}
    public void AddItem(Item item){
        RewardItem work = items.Find(x => ( ((int) x.ID) == item.ID) );  
        if(work != null){
            work.amount += 1;
        }else{
            items.Add(new RewardItem(item));
        }
    }
    public void AddSplinter(Splinter splinter){
        RewardSplinter work = splinters.Find(x => ( ((int) x.ID) == splinter.ID) );  
        if(work != null){
            work.amount += 1;
        }else{
            splinters.Add(new RewardSplinter(splinter));
        }
    }
    public void AddResource(Resource res){resources.AddResource(res); }
}
[System.Serializable]
public class RewardItem{
    public ItemName ID = ItemName.Stick;
    public int amount = 1;
    private Item item = null;
    public Item GetItem{ get{ if(item == null) item =  new Item(System.Convert.ToInt32(ID), amount);  return item; } }
    public RewardItem Clone(){ return new RewardItem(this.ID, this.amount); }
    public RewardItem(ItemName ID, int amount){
        this.ID = ID;
        this.amount = amount;
    }
    public RewardItem(Item item){
        ID = ItemName.Stick;
        this.item = item;
    }
}
[System.Serializable]
public class RewardSplinter{
    public SplinterName ID = SplinterName.OneStarPeople;
    public int amount = 1;
    private Splinter splinter = null;
    public Splinter GetSplinter{get{ if(splinter == null){splinter = new Splinter(System.Convert.ToInt32(ID), amount);} return splinter;}}
    public RewardSplinter Clone(){ return new RewardSplinter(this.ID, this.amount); }
    public RewardSplinter(SplinterName ID, int amount){
        this.ID = ID;
        this.amount = amount;
    }
    public RewardSplinter(Splinter splinter){
        ID = SplinterName.OneStarPeople;
        this.splinter = splinter;
    }
}



