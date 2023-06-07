using City.Buildings.Tavern;
using Common;
using Models.Heroes;
using System;
using UIController.GameSystems;
using UIController.ItemVisual;
using UnityEngine;

namespace UIController.Inventory
{
    [Serializable]
    public class SplinterModel : PlayerModel
    {
        public TypeSplinter typeSplinter;
        public RaceSpliter race;
        [SerializeField] private int requireAmount;
        [Header("rewards")]
        public PosibleReward reward = new PosibleReward();
        public int CountReward { get => reward.posibilityObjectRewards.Count; }
        public string Rarity;

        public bool IsCanUse { get => Amount >= RequireAmount; }
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

        public SplinterModel() { }
        //Constructors
        public SplinterModel(string ID, int count = 0)
        {
            Id = ID;
            GetDefaultData();
            Amount = count > 0 ? count : requireAmount;
        }

        public SplinterModel(HeroModel hero)
        {
            typeSplinter = TypeSplinter.Hero;
            sprite = hero.General.ImageHero;
            Rarity = hero.General.Rarity;
            Id = hero.General.ViewId;
            reward = new PosibleReward();
            reward.Add(Id);
            requireAmount = CalculateRequire();
        }

        private void GetDefaultData()
        {
            SplinterModel data = SplinterSystem.Instance.GetSplinter(Id);
            typeSplinter = data.typeSplinter;
            sprite = data.Image;
            reward = data.reward;
            Rarity = data.Rarity;
            requireAmount = data.RequireAmount;
        }

        public override BaseObject Clone()
        {
            return new SplinterModel(Id, Amount);
        }
        //Visual API

        public override void ClickOnItem()
        {
            InventoryController.Instance.OpenInfoItem(this);
        }

        public override void SetUI(ThingUI UI)
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
        private HeroModel GetRandomHero()
        {
            HeroModel hero = null;
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
        private void AddHero(HeroModel newHero)
        {
            if (newHero != null)
            {
                newHero.General.Name = newHero.General.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
                MessageController.Instance.AddMessage("Новый герой! Это - " + newHero.General.Name);
                GameController.Instance.AddHero(newHero);
            }
            else
            {
                Debug.Log("newHero null");
            }
        }
        //Operators
        public static SplinterModel operator *(SplinterModel item, int k)
        {
            return new SplinterModel(item.Id, k);
        }
    }
}