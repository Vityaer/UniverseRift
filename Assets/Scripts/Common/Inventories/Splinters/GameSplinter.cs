using Db.CommonDictionaries;
using Fight.HeroControllers.Generals;
using Models;
using Models.Heroes;
using Models.Inventory.Splinters;
using System;
using System.Linq;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards.PosibleRewards;
using UnityEngine;
using Utils;

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
        public string Rarity;

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
        //Constructors
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
            GetDefaultData();
            Amount = count > 0 ? count : requireCount;
        }

        public GameSplinter(HeroModel hero)
        {
            typeSplinter = SplinterType.Hero;
            //sprite = hero.General.ImageHero;
            Rarity = hero.General.Rarity;
            Id = hero.General.ViewId;
            reward = new PosibleRewardData();
            //reward.Add<GameHero>(Id);
            requireCount = CalculateRequire();
        }

        private void GetDefaultData()
        {
            switch (_model.SplinterType)
            {
                case SplinterType.Hero:
                    var prefab = Resources.Load<HeroController>($"{Constants.ResourcesPath.HEROES_PATH}{_model.ModelId}");
                    sprite = prefab.GetSprite;
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

        //Rewards
        public static GameSplinter operator *(GameSplinter item, int k)
        {
            return new GameSplinter(item.Id, k);
        }
    }
}