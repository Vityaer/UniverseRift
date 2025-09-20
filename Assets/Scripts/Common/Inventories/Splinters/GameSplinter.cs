using Fight.Common.HeroControllers.Generals;
using Models;
using Models.Heroes;
using Models.Inventory.Splinters;
using System;
using System.Linq;
using Common.Db.CommonDictionaries;
using UIController.Inventory;
using UIController.Rewards.PosibleRewards;
using UnityEngine;

namespace Common.Inventories.Splinters
{
    [Serializable]
    public class GameSplinter : BaseObject
    {
        public SplinterType typeSplinter;
        public RaceSplinter race;
        [SerializeField] private int requireCount;
        [Header("rewards")]
        public PosibleRewardData reward = new PosibleRewardData();
        public Rare Rare;

        private SplinterModel _model;
        private CommonDictionaries _commonDictionaries;
        private BaseModel _dataModel;

        //public int CountReward => reward.PosibilityObjectRewards.Count;
        public int CountReward => 1;
        public bool IsCanUse => Amount >= RequireAmount;
        public string GetTextType => typeSplinter.ToString();
        public string GetTextDescription => string.Empty;
        public SplinterModel Model => _model;
        public int RequireAmount
        {
            get
            {
                if (requireCount <= 0) requireCount = CalculateRequire();
                return requireCount;
            }
        }

        public override Sprite Image
        {
            get
            {
                if (sprite == null)
                {
                    //Debug.Log("ID: " + Id.ToString() + " splinter is founding image...");
                    //switch (typeSplinter)
                    //{
                    //    case TypeSplinter.Hero:
                    //        //sprite = SystemSprites.Instance.GetSprite((SpriteName)Enum.Parse(typeof(SpriteName), Id));
                    //        break;
                    //}
                }
                // Debug.Log("splinter was founded: " + (sprite == null).ToString() );
                return sprite;
            }
        }
        public GameSplinter() { }

        public GameSplinter(SplinterModel model, CommonDictionaries commonDictionaries, int count = 0)
        {
            _commonDictionaries = commonDictionaries;
            _model = model;
            Id = model.Id;
            GetDefaultData();
            Amount = count > 0 ? count : requireCount;
        }

        public GameSplinter(string id, int count = 0)
        {
            Id = id;
            Amount = count > 0 ? count : requireCount;
        }

        public GameSplinter(HeroModel hero)
        {
            typeSplinter = SplinterType.Hero;
            //sprite = hero.General.ImageHero;
            Rare = hero.General.Rare;
            Id = hero.General.HeroId;
            reward = new PosibleRewardData();
            //reward.Add<GameHero>(Id);
            requireCount = CalculateRequire();
        }

        public GameSplinter(SplinterModel splinterModel, int amount)
        {
            _model = splinterModel;
            Amount = amount;
            GetDefaultData();
        }

        private int CalculateRequire()
        {
            //return (20 + ((int)this.rare * 10));
            return 20;
        }

        public void SetCommonDictionaries(CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            _model = _commonDictionaries.Splinters[Id];
            GetDefaultData();
        }

        private void GetDefaultData()
        {
            if (_model == null)
            {
                Debug.LogError("_model null");
                return;
            }    
            switch (_model.SplinterType)
            {
                case SplinterType.Hero:
                    var path = $"{Constants.ResourcesPath.HEROES_PATH}{_model.ModelId}";
                    var prefab = Resources.Load<HeroController>(path);
                    sprite = prefab.Stages[0].Avatar;
                    break;
                case SplinterType.Item:
                    var itemModel = _commonDictionaries.Items[_model.ModelId];
                    var spriteAtlas = Resources.LoadAll<Sprite>("Items/Items");
                    sprite = spriteAtlas.ToList().Find(icon => icon.name.Equals(itemModel.SetName));
                    break;
            }
            typeSplinter = _model.SplinterType;
            requireCount = _model.RequireCount;
        }
    }
}