using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Splinter : PlayerObject
{
    public TypeSplinter typeSplinter;
    public RaceSpliter race;
    [SerializeField] private int requireAmount;
    [Header("rewards")]
    public PosibleReward reward = new PosibleReward();
    public int CountReward { get => reward.posibilityObjectRewards.Count; }
    public string Rarity;

    public bool IsCanUse { get => (Amount >= RequireAmount); }
    public string GetTextType { get => typeSplinter.ToString(); }
    public string GetTextDescription { get => string.Empty; }

    public int RequireAmount
    {
        get
        {
            if (requireAmount <= 0) requireAmount = CalculateRequire();
            return requireAmount;
        }
    }

    private int CalculateRequire()
    {
        //return (20 + ((int)this.rare * 10));
        return 20;
    }

    public override Sprite Image
    {
        get
        {
            if (sprite == null)
            {
                Debug.Log("ID: " + Id.ToString() + " splinter is founding image...");
                switch (typeSplinter)
                {
                    case TypeSplinter.Hero:
                        sprite = SystemSprites.Instance.GetSprite((SpriteName)Enum.Parse(typeof(SpriteName), Id));
                        break;
                }
            }
            // Debug.Log("splinter was founded: " + (sprite == null).ToString() );
            return sprite;
        }
    }

    public void GetReward(int countReward)
    {
        for (int i = 0; i < countReward; i++)
        {
            switch (typeSplinter)
            {
                case TypeSplinter.Hero:
                    AddHero(GetRandomHero());
                    break;
            }
        }
        Amount -= requireAmount * countReward;
        if (Amount > 0)
        {
            UpdateUI();
        }
        else
        {
            ClearData();
        }
        InventoryController.Instance.Refresh();
    }

    public void SetAmount(int amount)
    {
        Amount = amount;
        requireAmount = CalculateRequire();
    }

    public void AddAmount(int count)
    {
        Amount = Amount + count;
    }


    //Constructors
    public Splinter(string ID, int count = 0)
    {
        this.Id = ID;
        GetDefaultData();
        Amount = count > 0 ? count : requireAmount;
    }

    public Splinter(InfoHero hero)
    {
        this.typeSplinter = TypeSplinter.Hero;
        this.sprite = hero.generalInfo.ImageHero;
        this.Rarity = hero.generalInfo.Rarity;
        Id = hero.generalInfo.ViewId;
        reward = new PosibleReward();
        reward.Add(Id);
        requireAmount = CalculateRequire();
    }

    private void GetDefaultData()
    {
        Splinter data = SplinterSystem.Instance.GetSplinter(Id);
        this.typeSplinter = data.typeSplinter;
        this.sprite = data.Image;
        this.reward = data.reward;
        this.Rarity = data.Rarity;
        this.requireAmount = data.RequireAmount;
    }

    public override BaseObject Clone()
    {
        return new Splinter(this.Id, Amount);
    }
    //Visual API

    public override void ClickOnItem()
    {
        InventoryController.Instance.OpenInfoItem(this);
    }

    public override void SetUI(ThingUIScript UI)
    {
        this.UI = UI;
        UpdateUI();
    }

    public override void UpdateUI()
    {
        UI?.UpdateUI(Image, Amount);
    }

    private void ClearData()
    {
        UI?.Clear();
        InventoryController.Instance.RemoveSplinter(this);
    }

    private SpriteName GetSpriteName()
    {
        SpriteName result = SpriteName.BaseSplinterHero;
        switch (typeSplinter)
        {
            case TypeSplinter.Hero:
                result = SpriteName.BaseSplinterHero;
                break;
        }
        return result;
    }

    //Rewards
    private InfoHero GetRandomHero()
    {
        InfoHero hero = null;
        int selectNumber = 0;
        float rand = UnityEngine.Random.Range(0, reward.GetAllSum);
        for (int i = 0; i < CountReward; i++)
        {
            rand -= reward.posibilityObjectRewards[i].posibility;
            if (rand <= 0)
            {
                selectNumber = i;
                break;
            }
        }
        Debug.Log("selectNumber: " + selectNumber);
        hero = Tavern.Instance.GetInfoHero(reward.posibilityObjectRewards[selectNumber].ID);
        return hero;
    }
    private void AddHero(InfoHero newHero)
    {
        if (newHero != null)
        {
            newHero.generalInfo.Name = newHero.generalInfo.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
            MessageController.Instance.AddMessage("Новый герой! Это - " + newHero.generalInfo.Name);
            GameController.Instance.AddHero(newHero);
        }
        else
        {
            Debug.Log("newHero null");
        }
    }
    //Operators
    public static Splinter operator *(Splinter item, int k)
    {
        return new Splinter(item.Id, k);
    }
}

public enum TypeSplinter
{
    Hero,
    Artifact,
    Costume,
    Other
}

public enum RaceSpliter
{
    People,
    Elf,
    Undead,
    Mechanic,
    Inquisition,
    Demon,
    God,
    RandomAll,
    RandomWithoutGod
}

[System.Serializable]
public class SplinerPosibilityObject
{
    public string ID;
    public float posibility = 1f;
    public SplinerPosibilityObject(string ID, float percent = 1f)
    {
        this.ID = ID;
        this.posibility = percent;
    }
}
[System.Serializable]
public class PosibleReward
{
    public List<SplinerPosibilityObject> posibilityObjectRewards = new List<SplinerPosibilityObject>();
    float sumAll = 0;
    public float GetAllSum
    {
        get
        {
            if (sumAll > 0) { return sumAll; }
            else
            {
                for (int i = 0; i < posibilityObjectRewards.Count; i++) sumAll += posibilityObjectRewards[i].posibility;
                return sumAll;
            }

        }
    }
    public float PosibleNumObject(int num)
    {
        if (sumAll <= 0f) sumAll = GetAllSum;
        return (posibilityObjectRewards[num].posibility / sumAll * 100f);
    }

    public void Add(string ID, float percent = 1f)
    {
        posibilityObjectRewards.Add(new SplinerPosibilityObject(ID, percent));
    }
}