using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Reward
{
    [SerializeField] private ListResource resources = new ListResource();
    [SerializeField] private List<RewardItem> items = new List<RewardItem>();
    [SerializeField] private List<RewardSplinter> splinters = new List<RewardSplinter>();

    public int Count
    {
        get
        {
            if (generalList.Count == 0) PrepareRewardForShow();
            return generalList.Count;
        }
    }
    private List<BaseObject> generalList = new List<BaseObject>();
    public List<BaseObject> GetList
    {
        get
        {
            if (generalList.Count == 0) PrepareRewardForShow();
            return generalList;
        }
    }
    public Reward Clone()
    {
        ListResource listRes = (ListResource)resources.Clone();
        List<RewardItem> cloneItems = new List<RewardItem>();
        List<RewardSplinter> cloneSplinters = new List<RewardSplinter>();
        foreach (RewardItem item in this.items) { cloneItems.Add(item.Clone()); }
        foreach (RewardSplinter splinter in this.splinters) { cloneSplinters.Add(splinter.Clone()); }
        return new Reward(listRes, cloneItems, cloneSplinters);
    }
    public Reward(ListResource resources, List<RewardItem> items, List<RewardSplinter> splinters)
    {
        this.resources = resources;
        this.items = items;
        this.splinters = splinters;
    }
    public Reward(ListResource resources)
    {
        this.resources = resources;
    }
    public Reward() { }
    private void PrepareRewardForShow()
    {
        generalList.Clear();
        for (int i = 0; i < resources.List.Count; i++) generalList.Add(resources.List[i]);
        generalList.AddRange(GetItems);
        generalList.AddRange(GetSplinters);

    }
    public ListResource GetListResource { get => resources; }
    public List<Item> GetItems
    {
        get
        {
            List<Item> result = new List<Item>();
            foreach (RewardItem rewardItem in items)
                result.Add(rewardItem.GetItem);
            return result;
        }
    }
    public List<SplinterModel> GetSplinters
    {
        get
        {
            List<SplinterModel> result = new List<SplinterModel>();
            foreach (RewardSplinter rewardSplinter in splinters)
                result.Add(rewardSplinter.GetSplinter);
            return result;
        }
    }
    public void AddResources(ListResource newResources) { resources.AddResource(newResources); }
    public void AddItem(Item item)
    {
        RewardItem work = items.Find(x => (x.ID.ToString() == item.Id));
        if (work != null)
        {
            work.amount += 1;
        }
        else
        {
            items.Add(new RewardItem(item));
        }
    }
    public void AddSplinter(SplinterModel splinter)
    {
        RewardSplinter work = splinters.Find(x => (x.ID.ToString() == splinter.Id));
        if (work != null)
        {
            work.amount += 1;
        }
        else
        {
            splinters.Add(new RewardSplinter(splinter));
        }
    }
    public void AddResource(Resource res) { resources.AddResource(res); }
}



