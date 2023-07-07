using Campaign;
using City.TrainCamp;
using Common;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models;
using Models.Achievments;
using Models.City.Markets;
using Models.City.Mines;
using Models.City.Misc;
using Models.Common;
using Models.Fights.Misc;
using Models.Heroes;
using Models.Items;
using System.Collections.Generic;
using UIController.Inventory;
using UniRx;
using UnityEngine;
using Utils;

namespace Db.CommonDictionaries
{
    public class CommonDictionaries
    {
        public ReactiveCommand OnStartDownloadFiles = new ReactiveCommand();
        public ReactiveCommand OnFinishDownloadFiles = new ReactiveCommand();

        private Dictionary<string, HeroModel> _heroes = new Dictionary<string, HeroModel>();
        private Dictionary<string, RaceModel> _races = new Dictionary<string, RaceModel>();
        private Dictionary<string, VocationModel> _vocations = new Dictionary<string, VocationModel>();
        private Dictionary<string, ItemModel> _items = new Dictionary<string, ItemModel>();
        private Dictionary<string, RarityModel> _raryties = new Dictionary<string, RarityModel>();
        private Dictionary<string, ItemSet> _itemSets = new Dictionary<string, ItemSet>();
        private Dictionary<string, ItemRelationModel> _itemRelations = new Dictionary<string, ItemRelationModel>();
        private Dictionary<string, RatingModel> _ratings = new Dictionary<string, RatingModel>();
        private Dictionary<string, CampaignChapterModel> _campaignChapters = new Dictionary<string, CampaignChapterModel>();
        private Dictionary<string, LocationModel> _locations = new Dictionary<string, LocationModel>();
        private Dictionary<string, SplinterModel> _splinters = new Dictionary<string, SplinterModel>();
        private Dictionary<string, ProductModel> _products = new Dictionary<string, ProductModel>();
        private Dictionary<string, MarketModel> _markets = new Dictionary<string, MarketModel>();
        private Dictionary<string, MineModel> _mines = new Dictionary<string, MineModel>();
        private Dictionary<string, StorageChallengeModel> _storageChallenges = new Dictionary<string, StorageChallengeModel>();
        private Dictionary<string, ResistanceModel> _resistances = new Dictionary<string, ResistanceModel>();
        private Dictionary<string, AchievmentModel> _achievments = new Dictionary<string, AchievmentModel>();
        private Dictionary<string, CostLevelUpContainer> _heroesCostLevelUps = new Dictionary<string, CostLevelUpContainer>();
        private Dictionary<string, MonthlyTasksModel> _monthlyTasks = new Dictionary<string, MonthlyTasksModel>();
        private Dictionary<string, TaskModel> _patternTasks = new Dictionary<string, TaskModel>();
        
        private readonly IJsonConverter _converter;
        private bool _isInited;

        public bool Inited => _isInited;
        public Dictionary<string, HeroModel> Heroes => _heroes;
        public Dictionary<string, ItemModel> Items => _items;
        public Dictionary<string, RarityModel> Rarities => _raryties;
        public Dictionary<string, ItemSet> ItemSets => _itemSets;
        public Dictionary<string, RatingModel> Ratings => _ratings;
        public Dictionary<string, RaceModel> Races => _races;
        public Dictionary<string, VocationModel> Vocations => _vocations;
        public Dictionary<string, CampaignChapterModel> CampaignChapters => _campaignChapters;
        public Dictionary<string, LocationModel> Locations => _locations;
        public Dictionary<string, SplinterModel> Splinters => _splinters;
        public Dictionary<string, ProductModel> Products => _products;
        public Dictionary<string, MarketModel> Markets => _markets;
        public Dictionary<string, MineModel> Mines => _mines;
        public Dictionary<string, StorageChallengeModel> StorageChallenges => _storageChallenges;
        public Dictionary<string, ResistanceModel> Resistances => _resistances;
        public Dictionary<string, AchievmentModel> Achievments => _achievments;
        public Dictionary<string, ItemRelationModel> ItemRelations => _itemRelations;
        public Dictionary<string, CostLevelUpContainer> CostContainers => _heroesCostLevelUps;
        public Dictionary<string, MonthlyTasksModel> MonthlyTasks => _monthlyTasks;
        public Dictionary<string, TaskModel> PatternTasks => _patternTasks;



        private bool IsDownloadedInLocalStorage
        {
            get
            {
                //var result = TextUtils.IsLoadedToLocalStorage<GameHero>();
                //result &= TextUtils.IsLoadedToLocalStorage<Item>();
                //result &= TextUtils.IsLoadedToLocalStorage<Rarity>();
                //return result;
                return true;
            }
        }

        public CommonDictionaries(IJsonConverter converter)
        {
            _converter = converter;
        }

        public async UniTask Init()
        {
#if UNITY_EDITOR
            var needUpdateConfig = false;
#else
            var needUpdateConfig = await IsNeedUpdateConfig();
#endif
            if (!IsDownloadedInLocalStorage || needUpdateConfig)
            {
                OnStartDownloadFiles.Execute();
                await LoadFromRemoteDirectory();
                OnFinishDownloadFiles.Execute();
            }
            else
            {
                LoadFromLocalDirectory();
            }

            _isInited = true;
        }

        private async UniTask<bool> IsNeedUpdateConfig()
        {
            var jsonData = string.Empty;
            if (TextUtils.IsLoadedToLocalStorage<ConfigVersion>())
            {
                jsonData = TextUtils.GetTextFromLocalStorage<ConfigVersion>();
            }
            else
            {
                jsonData = await TextUtils.DownloadJsonData(nameof(ConfigVersion));
                TextUtils.Save<ConfigVersion>(jsonData);
                return true;
            }

            var currentConfig = TextUtils.FillModel<ConfigVersion>(jsonData, _converter);
            var serverConfigJson = await TextUtils.DownloadJsonData(nameof(ConfigVersion));
            Debug.Log($"server load {serverConfigJson}");
            var serverConfig = TextUtils.FillModel<ConfigVersion>(serverConfigJson, _converter);
            Debug.Log(
                $"current config version:{currentConfig.Version}\nserver config version:{serverConfig.Version}");

            if (currentConfig.Version != serverConfig.Version)
            {
                TextUtils.Save<ConfigVersion>(serverConfigJson);
                return true;
            }

            return false;
        }

        private async UniTask LoadFromRemoteDirectory()
        {
            //_cells = await DownloadModels<BaseCell>();

            //var jsonData = await TextUtils.DownloadJsonData(nameof(AbilityConfigList));
            //_abilitiesConfig = _converter.FromJson<AbilityConfigList>(jsonData).ConfigList;
            //TextUtils.Save<AbilityConfigList>(jsonData);

            //jsonData = await TextUtils.DownloadJsonData(nameof(MatchConfigList));
            //_matchConfigs = _converter.FromJson<MatchConfigList>(jsonData).ConfigList;
            //TextUtils.Save<MatchConfigList>(jsonData);

            //jsonData = await TextUtils.DownloadJsonData(nameof(RareUpgradeCostCoefficents));
            //_rareCoefficentCostConfigs = _converter.FromJson<RareUpgradeCostCoefficents>(jsonData).RareCoefficents;
            //TextUtils.Save<RareUpgradeCostCoefficents>(jsonData);

            //jsonData = await TextUtils.DownloadJsonData(nameof(UpgradeCostModel));
            //_upgradeCoefficentCostConfigs = _converter.FromJson<UpgradeCostModel>(jsonData).LevelCoefficents;
            //TextUtils.Save<UpgradeCostModel>(jsonData);
        }

        //private async UniTask<Dictionary<string, T>> DownloadModels<T>() where T : BaseModel
        //{
        //var jsonData = await TextUtils.DownloadJsonData(typeof(T).Name);
        //TextUtils.Save<T>(jsonData, _converter);
        //return TextUtils.FillDictionary<T>(jsonData, _converter);
        //}

        private void LoadFromLocalDirectory()
        {
            _heroes = GetModels<HeroModel>();
            _races = GetModels<RaceModel>();
            _vocations = GetModels<VocationModel>();
            _items = GetModels<ItemModel>();
            _raryties = GetModels<RarityModel>();
            _itemSets = GetModels<ItemSet>();
            _ratings = GetModels<RatingModel>();
            _campaignChapters = GetModels<CampaignChapterModel>();
            _locations = GetModels<LocationModel>();
            _splinters = GetModels<SplinterModel>();
            _products = GetModels<ProductModel>();
            _markets = GetModels<MarketModel>();
            _mines = GetModels<MineModel>();
            _storageChallenges = GetModels<StorageChallengeModel>();
            _resistances = GetModels<ResistanceModel>();
            _achievments = GetModels<AchievmentModel>();
            _itemRelations = GetModels<ItemRelationModel>();
            _heroesCostLevelUps = GetModels<CostLevelUpContainer>();
            _monthlyTasks = GetModels<MonthlyTasksModel>();
            _patternTasks = GetModels<TaskModel>();
        }

        private Dictionary<string, T> GetModels<T>() where T : BaseModel
        {
            var jsonData = TextUtils.GetTextFromLocalStorage<T>();
            return TextUtils.FillDictionary<T>(jsonData, _converter);
        }
    }
}