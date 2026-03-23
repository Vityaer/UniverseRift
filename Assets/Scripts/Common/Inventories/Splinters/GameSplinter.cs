using Models;
using Models.Heroes;
using Models.Inventory.Splinters;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Models.Data.Inventories;
using UIController.Rewards.PosibleRewards;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.DataCheckUtils;
using HeroController = Fight.HeroControllers.Generals.HeroController;

namespace Common.Inventories.Splinters
{
    [Serializable]
    public class GameSplinter : BaseObject
    {
        public SplinterType typeSplinter;
        [SerializeField] private int requireCount;
        public Rare Rare;

        private SplinterModel m_model;
        private CommonDictionaries m_commonDictionaries;
        private BaseModel m_dataModel;
        private List<RandomSplinterModel> m_posibleRewardData;

        public List<RandomSplinterModel> Reward => m_posibleRewardData;
        //public int CountReward => reward.PosibilityObjectRewards.Count;
        public int CountReward => 1;
        public bool IsCanUse => Amount >= RequireAmount;
        public string GetTextType => typeSplinter.ToString();
        public string GetTextDescription => string.Empty;
        public SplinterModel Model => m_model;
        public int RequireAmount
        {
            get
            {
                if (requireCount <= 0) requireCount = CalculateRequire();
                return requireCount;
            }
        }

        public override Sprite Image => sprite;
        public GameSplinter() { }

        public GameSplinter(SplinterModel model, CommonDictionaries commonDictionaries, int count = 0)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_model = model.ThrowIfArgumentNull();
            Id = model.Id;
            GetDefaultData();
            Amount = count > 0 ? count : requireCount;
        }

        public GameSplinter(string id, int count = 0)
        {
            Id = id;
            Amount = count > 0 ? count : requireCount;
        }

        public GameSplinter(SplinterModel splinterModel, int amount)
        {
            m_model = splinterModel;
            Amount = amount;
            GetDefaultData();
        }

        public void SetCommonDictionaries(CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries;
            m_model = m_commonDictionaries.Splinters[Id];
            GetDefaultData();
        }

        private void GetDefaultData()
        {
            if (m_model == null)
            {
                Debug.LogError("_model null");
                return;
            }   
            
            switch (m_model.SplinterType)
            {
                case SplinterType.Hero:
                    var path = $"{Constants.ResourcesPath.HEROES_PATH}{m_model.ModelIds[0].ModelId}";
                    var prefab = Resources.Load<HeroController>(path);
                    sprite = prefab.Stages[0].Avatar;
                    break;
                case SplinterType.Item:
                    var itemModel = m_commonDictionaries.Items[m_model.ModelIds[0].ModelId];
                    var spriteAtlas = Resources.LoadAll<Sprite>("Items/Items");
                    sprite = spriteAtlas.ToList().Find(icon => icon.name.Equals(itemModel.SetName));
                    break;
            }
            typeSplinter = m_model.SplinterType;
            requireCount = m_model.RequireCount;
        }
        
        
        private int CalculateRequire()
        {
            //return (20 + ((int)this.rare * 10));
            return 20;
        }
    }
}