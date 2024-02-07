using Campaign;
using City.Buildings.WheelFortune;
using City.TrainCamp;
using Common;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models;
using Models.Achievments;
using Models.City.AbstactBuildingModels;
using Models.City.FortuneRewards;
using Models.City.Markets;
using Models.City.Mines;
using Models.City.Misc;
using Models.City.TravelCircle;
using Models.Common;
using Models.Data.Dailies;
using Models.Data.Dailies.Tasks;
using Models.Fights.Misc;
using Models.Guilds;
using Models.Heroes;
using Models.Heroes.PowerUps;
using Models.Inventory.Splinters;
using Models.Items;
using Models.Misc.Avatars;
using Models.Rewards;
using Models.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.Rewards;
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
        private Dictionary<string, BaseProductModel> _products = new Dictionary<string, BaseProductModel>();
        private Dictionary<string, MarketModel> _markets = new Dictionary<string, MarketModel>();
        private Dictionary<string, MineModel> _mines = new Dictionary<string, MineModel>();
        private Dictionary<string, StorageChallengeModel> _storageChallenges = new Dictionary<string, StorageChallengeModel>();
        private Dictionary<string, ResistanceModel> _resistances = new Dictionary<string, ResistanceModel>();
        private Dictionary<string, CostLevelUpContainer> _heroesCostLevelUps = new Dictionary<string, CostLevelUpContainer>();
        private Dictionary<string, MonthlyTasksModel> _monthlyTasks = new Dictionary<string, MonthlyTasksModel>();
        private Dictionary<string, GameTaskModel> _gameTaskModels = new Dictionary<string, GameTaskModel>();
        private Dictionary<string, BuildingModel> _buildings = new Dictionary<string, BuildingModel>();
        private Dictionary<string, RewardModel> _rewards = new Dictionary<string, RewardModel>();
        private Dictionary<string, FortuneRewardModel> _fortuneRewardModels = new Dictionary<string, FortuneRewardModel>();
        private Dictionary<string, DailyRewardModel> _dailyRewardDatas = new Dictionary<string, DailyRewardModel>();
        private Dictionary<string, MineRestrictionModel> _mineRestrictions = new Dictionary<string, MineRestrictionModel>();
        private Dictionary<string, TravelRaceModel> _travelRaceCampaigns = new Dictionary<string, TravelRaceModel>();
        private Dictionary<string, AchievmentModel> _achievments = new Dictionary<string, AchievmentModel>();
        private Dictionary<string, AchievmentContainerModel> _achievmentContainers = new Dictionary<string, AchievmentContainerModel>();
        private Dictionary<string, RewardContainerModel> _rewardContainerModels = new();
        private Dictionary<string, GuildBossContainer> _guildBossContainers = new();
        private Dictionary<string, AvatarModel> _avatarModels = new();
        private Dictionary<string, RatingUpContainer> _ratingUpContainers = new();

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
        public Dictionary<string, BaseProductModel> Products => _products;
        public Dictionary<string, MarketModel> Markets => _markets;
        public Dictionary<string, MineModel> Mines => _mines;
        public Dictionary<string, StorageChallengeModel> StorageChallenges => _storageChallenges;
        public Dictionary<string, ResistanceModel> Resistances => _resistances;
        public Dictionary<string, AchievmentModel> Achievments => _achievments;
        public Dictionary<string, ItemRelationModel> ItemRelations => _itemRelations;
        public Dictionary<string, CostLevelUpContainer> CostContainers => _heroesCostLevelUps;
        public Dictionary<string, MonthlyTasksModel> MonthlyTasks => _monthlyTasks;
        public Dictionary<string, GameTaskModel> GameTaskModels => _gameTaskModels;
        public Dictionary<string, BuildingModel> Buildings => _buildings;
        public Dictionary<string, RewardModel> Rewards => _rewards;
        public Dictionary<string, FortuneRewardModel> FortuneRewardModels => _fortuneRewardModels;
        public Dictionary<string, DailyRewardModel> DailyRewardDatas => _dailyRewardDatas;
        public Dictionary<string, AchievmentContainerModel> AchievmentContainers => _achievmentContainers;
        public Dictionary<string, MineRestrictionModel> MineRestrictions => _mineRestrictions;
        public Dictionary<string, TravelRaceModel> TravelRaceCampaigns => _travelRaceCampaigns;
        public Dictionary<string, RewardContainerModel> RewardContainerModels => _rewardContainerModels;
        public Dictionary<string, GuildBossContainer> GuildBossContainers => _guildBossContainers;
        public Dictionary<string, AvatarModel> AvatarModels => _avatarModels;
        public Dictionary<string, RatingUpContainer> RatingUpContainers => _ratingUpContainers;

        private bool IsDownloadedInLocalStorage
        {
            get
            {
                var result = TextUtils.IsLoadedToLocalStorage<HeroModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<ItemModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<RarityModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<ItemSet>();
                //result &= TextUtils.IsLoadedToLocalStorage<RatingModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<RaceModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<VocationModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<CampaignChapterModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<LocationModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<SplinterModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<ProductModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<MarketModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<MineModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<StorageChallengeModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<ResistanceModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<AchievmentModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<ItemRelationModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<CostLevelUpContainer>();
                //result &= TextUtils.IsLoadedToLocalStorage<MonthlyTasksModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<GameTaskModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<BuildingModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<RewardModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<MineRestrictionModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<TravelRaceModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<RewardContainerModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<GuildBossContainer>();
                //result &= TextUtils.IsLoadedToLocalStorage<AvatarModel>();
                //result &= TextUtils.IsLoadedToLocalStorage<CostRatingUpContainer>();

                return result;
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
            if (!TextUtils.IsLoadedToLocalStorage<ConfigVersion>())
            {
                needUpdateConfig = await IsNeedUpdateConfig();
            }
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
            _heroes = await DownloadModels<HeroModel>();
            _races = await DownloadModels<RaceModel>();
            _vocations = await DownloadModels<VocationModel>();
            _items = await DownloadModels<ItemModel>();
            _raryties = await DownloadModels<RarityModel>();
            _itemSets = await DownloadModels<ItemSet>();
            _ratings = await DownloadModels<RatingModel>();
            _campaignChapters = await DownloadModels<CampaignChapterModel>();
            _locations = await DownloadModels<LocationModel>();
            _splinters = await DownloadModels<SplinterModel>();
            _products = await DownloadModels<BaseProductModel>();
            _markets = await DownloadModels<MarketModel>();
            _mines = await DownloadModels<MineModel>();
            _storageChallenges = await DownloadModels<StorageChallengeModel>();
            _resistances = await DownloadModels<ResistanceModel>();
            _achievments = await DownloadModels<AchievmentModel>();
            _itemRelations = await DownloadModels<ItemRelationModel>();
            _heroesCostLevelUps = await DownloadModels<CostLevelUpContainer>();
            _monthlyTasks = await DownloadModels<MonthlyTasksModel>();
            _gameTaskModels = await DownloadModels<GameTaskModel>();
            _buildings = await DownloadModels<BuildingModel>();
            _rewards = await DownloadModels<RewardModel>();
            _fortuneRewardModels = await DownloadModels<FortuneRewardModel>();
            _gameTaskModels = await DownloadModels<GameTaskModel>();
            _dailyRewardDatas = await DownloadModels<DailyRewardModel>();
            _achievmentContainers = await DownloadModels<AchievmentContainerModel>();
            _mineRestrictions = await DownloadModels<MineRestrictionModel>();
            _travelRaceCampaigns = await DownloadModels<TravelRaceModel>();
            _rewardContainerModels = await DownloadModels<RewardContainerModel>();
            _guildBossContainers = await DownloadModels<GuildBossContainer>();
            _avatarModels = await DownloadModels<AvatarModel>();
            _ratingUpContainers = await DownloadModels<RatingUpContainer>();
        }

        private async UniTask<Dictionary<string, T>> DownloadModels<T>() where T : BaseModel
        {
            var jsonData = await TextUtils.DownloadJsonData(typeof(T).Name);
            TextUtils.Save<T>(jsonData);
            return TextUtils.FillDictionary<T>(jsonData, _converter);
        }

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
            _products = GetModels<BaseProductModel>();
            _markets = GetModels<MarketModel>();
            _mines = GetModels<MineModel>();
            _storageChallenges = GetModels<StorageChallengeModel>();
            _resistances = GetModels<ResistanceModel>();
            _achievments = GetModels<AchievmentModel>();
            _itemRelations = GetModels<ItemRelationModel>();
            _heroesCostLevelUps = GetModels<CostLevelUpContainer>();
            _monthlyTasks = GetModels<MonthlyTasksModel>();
            _gameTaskModels = GetModels<GameTaskModel>();
            _buildings = GetModels<BuildingModel>();
            _rewards = GetModels<RewardModel>();
            _fortuneRewardModels = GetModels<FortuneRewardModel>();
            _gameTaskModels = GetModels<GameTaskModel>();
            _dailyRewardDatas = GetModels<DailyRewardModel>();
            _achievmentContainers = GetModels<AchievmentContainerModel>();
            _mineRestrictions = GetModels<MineRestrictionModel>();
            _travelRaceCampaigns = GetModels<TravelRaceModel>();
            _rewardContainerModels = GetModels<RewardContainerModel>();
            _guildBossContainers = GetModels<GuildBossContainer>();
            _avatarModels = GetModels<AvatarModel>();
            _ratingUpContainers = GetModels<RatingUpContainer>();
        }

        private Dictionary<string, T> GetModels<T>() where T : BaseModel
        {
            var jsonData = TextUtils.GetTextFromLocalStorage<T>();
            return TextUtils.FillDictionary<T>(jsonData, _converter);
        }
    }
}