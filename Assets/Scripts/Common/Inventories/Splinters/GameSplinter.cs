﻿using City.Buildings.Tavern;
using City.Panels.Messages;
using Hero;
using Models.Common.BigDigits;
using Models.Heroes;
using System;
using UIController.GameSystems;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards.PosibleRewards;
using UnityEngine;

namespace Common.Inventories.Splinters
{
    [Serializable]
    public class GameSplinter : BaseObject
    {
        public TypeSplinter typeSplinter;
        public RaceSplinter race;
        [SerializeField] private int requireAmount;
        [Header("rewards")]
        public PosibleRewardData reward = new PosibleRewardData();
        public string Rarity;

        //public int CountReward => reward.PosibilityObjectRewards.Count;
        public int CountReward => 1;
        public bool IsCanUse => Amount >= RequireAmount;
        public string GetTextType  => typeSplinter.ToString();
        public string GetTextDescription  => string.Empty;

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
            //Amount -= requireAmount * countReward;
            //if (Amount > 0)
            //{
            //    UpdateUI();
            //}
            //else
            //{
            //    ClearData();
            //}
            //InventoryController.Instance.Refresh();
        }

        public void SetAmount(int amount)
        {
            //Amount = amount;
            //requireAmount = CalculateRequire();
        }

        public void AddAmount(int count)
        {
            //Amount = Amount + count;
        }

        public GameSplinter() { }
        //Constructors
        public GameSplinter(string ID, int count = 0)
        {
            Id = ID;
            GetDefaultData();
            //Amount = count > 0 ? count : requireAmount;
        }

        public GameSplinter(HeroModel hero)
        {
            typeSplinter = TypeSplinter.Hero;
            //sprite = hero.General.ImageHero;
            Rarity = hero.General.Rarity;
            Id = hero.General.ViewId;
            reward = new PosibleRewardData();
            //reward.Add<GameHero>(Id);
            requireAmount = CalculateRequire();
        }

        private void GetDefaultData()
        {
            //GameSplinter data = SplinterSystem.Instance.GetSplinter(Id);
            //typeSplinter = data.typeSplinter;
            //sprite = data.Image;
            //reward = data.reward;
            //Rarity = data.Rarity;
            //requireAmount = data.RequireAmount;
        }

        //public override BaseObject Clone()
        //{
        //    return new GameSplinter(Id, Amount);
        //}

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
            //float rand = UnityEngine.Random.Range(0, reward.GetAllSum);
            //for (int i = 0; i < CountReward; i++)
            //{
                //rand -= reward.PosibilityObjectRewards[i].Posibility;
                //if (rand <= 0)
                //{
                    //selectNumber = i;
                    //break;
                //}
            //}
            Debug.Log("selectNumber: " + selectNumber);
            //hero = TavernController.Instance.GetInfoHero(reward.posibilityObjectRewards[selectNumber].ID);
            return hero;
        }
        private void AddHero(HeroModel newHero)
        {
            if (newHero != null)
            {
                newHero.General.Name = newHero.General.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
                //MessageController.Instance.AddMessage("Новый герой! Это - " + newHero.General.Name);
                //GameController.Instance.AddHero(newHero);
            }
            else
            {
                Debug.Log("newHero null");
            }
        }

        public static GameSplinter operator *(GameSplinter item, int k)
        {
            return new GameSplinter(item.Id, k);
        }
    }
}