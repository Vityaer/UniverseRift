using System.Collections.Generic;
using Campaign;
using City.TrainCamp;
using Cysharp.Threading.Tasks;
using Misc.Json;
using Models;
using Models.Achievments;
using Models.City.AbstactBuildingModels;
using Models.City.FortuneRewards;
using Models.City.Hires;
using Models.City.Markets;
using Models.City.Mines;
using Models.City.Misc;
using Models.City.TravelCircle;
using Models.Data.Dailies;
using Models.Data.Dailies.Tasks;
using Models.Fights.Misc;
using Models.Guilds;
using Models.Heroes;
using Models.Heroes.HeroCharacteristics.Abstractions;
using Models.Heroes.PowerUps;
using Models.Inventory.Splinters;
using Models.Items;
using Models.Misc.Avatars;
using Models.Misc.Helps;
using Models.Rewards;
using Models.Tasks;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using Utils;

namespace Common.Db.CommonDictionaries
{
    public class CommonDictionaries
    {
        public ReactiveCommand OnStartDownloadFiles = new();
        public ReactiveCommand OnFinishDownloadFiles = new();

        private Dictionary<string, HeroModel> m_heroes = new();
        private Dictionary<string, RaceModel> m_races = new();
        private Dictionary<string, VocationModel> m_vocations = new();
        private Dictionary<string, ItemModel> m_items = new();
        private Dictionary<string, RarityModel> m_raryties = new();
        private Dictionary<string, ItemSet> m_itemSets = new();
        private Dictionary<string, ItemRelationModel> m_itemRelations = new();
        private Dictionary<string, RatingModel> m_ratings = new();
        private Dictionary<string, CampaignChapterModel> m_campaignChapters = new();
        private Dictionary<string, LocationModel> m_locations = new();
        private Dictionary<string, SplinterModel> m_splinters = new();
        private Dictionary<string, BaseProductModel> m_products = new();
        private Dictionary<string, MarketModel> m_markets = new();
        private Dictionary<string, MineModel> m_mines = new();
        private Dictionary<string, StorageChallengeModel> m_storageChallenges = new();
        private Dictionary<string, ResistanceModel> m_resistances = new();
        private Dictionary<string, CostLevelUpContainer> m_heroesCostLevelUps = new();
        private Dictionary<string, MonthlyTasksModel> m_monthlyTasks = new();
        private Dictionary<string, GameTaskModel> m_gameTaskModels = new();
        private Dictionary<string, BuildingModel> m_buildings = new();
        private Dictionary<string, RewardModel> m_rewards = new();
        private Dictionary<string, FortuneRewardModel> m_fortuneRewardModels = new();
        private Dictionary<string, DailyRewardModel> m_dailyRewardDatas = new();
        private Dictionary<string, MineRestrictionModel> m_mineRestrictions = new();
        private Dictionary<string, TravelRaceModel> m_travelRaceCampaigns = new();
        private Dictionary<string, AchievmentModel> m_achievments = new();
        private Dictionary<string, AchievmentContainerModel> m_achievmentContainers = new();
        private Dictionary<string, RewardContainerModel> m_rewardContainerModels = new();
        private Dictionary<string, GuildBossContainer> m_guildBossContainers = new();
        private Dictionary<string, AvatarModel> m_avatarModels = new();
        private Dictionary<string, RatingUpContainer> m_ratingUpContainers = new();
        private Dictionary<string, CharacteristicModel> m_characteristicModels = new();
        private Dictionary<string, HireContainerModel> m_hireContainerModels = new();
        private Dictionary<string, LocationModel> m_locationModels = new();
        private Dictionary<string, HelpResourceModel> m_helpResourceModels = new();

        private readonly IJsonConverter m_converter;
        private bool m_isInited;

        public bool Inited => m_isInited;
        public Dictionary<string, HeroModel> Heroes => m_heroes;
        public Dictionary<string, ItemModel> Items => m_items;
        public Dictionary<string, RarityModel> Rarities => m_raryties;
        public Dictionary<string, ItemSet> ItemSets => m_itemSets;
        public Dictionary<string, RatingModel> Ratings => m_ratings;
        public Dictionary<string, RaceModel> Races => m_races;
        public Dictionary<string, VocationModel> Vocations => m_vocations;
        public Dictionary<string, CampaignChapterModel> CampaignChapters => m_campaignChapters;
        public Dictionary<string, LocationModel> Locations => m_locations;
        public Dictionary<string, SplinterModel> Splinters => m_splinters;
        public Dictionary<string, BaseProductModel> Products => m_products;
        public Dictionary<string, MarketModel> Markets => m_markets;
        public Dictionary<string, MineModel> Mines => m_mines;
        public Dictionary<string, StorageChallengeModel> StorageChallenges => m_storageChallenges;
        public Dictionary<string, ResistanceModel> Resistances => m_resistances;
        public Dictionary<string, AchievmentModel> Achievments => m_achievments;
        public Dictionary<string, ItemRelationModel> ItemRelations => m_itemRelations;
        public Dictionary<string, CostLevelUpContainer> CostContainers => m_heroesCostLevelUps;
        public Dictionary<string, MonthlyTasksModel> MonthlyTasks => m_monthlyTasks;
        public Dictionary<string, GameTaskModel> GameTaskModels => m_gameTaskModels;
        public Dictionary<string, BuildingModel> Buildings => m_buildings;
        public Dictionary<string, RewardModel> Rewards => m_rewards;
        public Dictionary<string, FortuneRewardModel> FortuneRewardModels => m_fortuneRewardModels;
        public Dictionary<string, DailyRewardModel> DailyRewardDatas => m_dailyRewardDatas;
        public Dictionary<string, AchievmentContainerModel> AchievmentContainers => m_achievmentContainers;
        public Dictionary<string, MineRestrictionModel> MineRestrictions => m_mineRestrictions;
        public Dictionary<string, TravelRaceModel> TravelRaceCampaigns => m_travelRaceCampaigns;
        public Dictionary<string, RewardContainerModel> RewardContainerModels => m_rewardContainerModels;
        public Dictionary<string, GuildBossContainer> GuildBossContainers => m_guildBossContainers;
        public Dictionary<string, AvatarModel> AvatarModels => m_avatarModels;
        public Dictionary<string, RatingUpContainer> RatingUpContainers => m_ratingUpContainers;
        public Dictionary<string, CharacteristicModel> CharacteristicModels => m_characteristicModels;
        public Dictionary<string, HireContainerModel> HireContainerModels => m_hireContainerModels;
        public Dictionary<string, LocationModel> LocationModels => m_locationModels;
        public Dictionary<string, HelpResourceModel> HelpResourceModels => m_helpResourceModels;

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
            m_converter = converter;
        }

        public async UniTask Init()
        {
#if UNITY_EDITOR
            var needUpdateConfig = false;
            if (!TextUtils.IsLoadedToLocalStorage<ConfigVersion>()) needUpdateConfig = await IsNeedUpdateConfig();
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

            m_isInited = true;
        }

        private async UniTask<bool> IsNeedUpdateConfig()
        {
            string jsonData = string.Empty;
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

            var currentConfig = TextUtils.FillModel<ConfigVersion>(jsonData, m_converter);
            var serverConfigJson = await TextUtils.DownloadJsonData(nameof(ConfigVersion));
            Debug.Log($"server load {serverConfigJson}");
            var serverConfig = TextUtils.FillModel<ConfigVersion>(serverConfigJson, m_converter);
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
            m_heroes = await DownloadModels<HeroModel>();
            m_races = await DownloadModels<RaceModel>();
            m_vocations = await DownloadModels<VocationModel>();
            m_items = await DownloadModels<ItemModel>();
            m_raryties = await DownloadModels<RarityModel>();
            m_itemSets = await DownloadModels<ItemSet>();
            m_ratings = await DownloadModels<RatingModel>();
            m_campaignChapters = await DownloadModels<CampaignChapterModel>();
            m_locations = await DownloadModels<LocationModel>();
            m_splinters = await DownloadModels<SplinterModel>();
            m_products = await DownloadModels<BaseProductModel>();
            m_markets = await DownloadModels<MarketModel>();
            m_mines = await DownloadModels<MineModel>();
            m_storageChallenges = await DownloadModels<StorageChallengeModel>();
            m_resistances = await DownloadModels<ResistanceModel>();
            m_achievments = await DownloadModels<AchievmentModel>();
            m_itemRelations = await DownloadModels<ItemRelationModel>();
            m_heroesCostLevelUps = await DownloadModels<CostLevelUpContainer>();
            m_monthlyTasks = await DownloadModels<MonthlyTasksModel>();
            m_gameTaskModels = await DownloadModels<GameTaskModel>();
            m_buildings = await DownloadModels<BuildingModel>();
            m_rewards = await DownloadModels<RewardModel>();
            m_fortuneRewardModels = await DownloadModels<FortuneRewardModel>();
            m_gameTaskModels = await DownloadModels<GameTaskModel>();
            m_dailyRewardDatas = await DownloadModels<DailyRewardModel>();
            m_achievmentContainers = await DownloadModels<AchievmentContainerModel>();
            m_mineRestrictions = await DownloadModels<MineRestrictionModel>();
            m_travelRaceCampaigns = await DownloadModels<TravelRaceModel>();
            m_rewardContainerModels = await DownloadModels<RewardContainerModel>();
            m_guildBossContainers = await DownloadModels<GuildBossContainer>();
            m_avatarModels = await DownloadModels<AvatarModel>();
            m_ratingUpContainers = await DownloadModels<RatingUpContainer>();
            m_characteristicModels = await DownloadModels<CharacteristicModel>();
            m_hireContainerModels = await DownloadModels<HireContainerModel>();
            m_locationModels = await DownloadModels<LocationModel>();
            m_helpResourceModels = await DownloadModels<HelpResourceModel>();
        }

        private async UniTask<Dictionary<string, T>> DownloadModels<T>() where T : BaseModel
        {
            var jsonData = await TextUtils.DownloadJsonData(typeof(T).Name);
            TextUtils.Save<T>(jsonData);
            return TextUtils.FillDictionary<T>(jsonData, m_converter);
        }

        private void LoadFromLocalDirectory()
        {
            m_heroes = GetModels<HeroModel>();
            m_races = GetModels<RaceModel>();
            m_vocations = GetModels<VocationModel>();
            m_items = GetModels<ItemModel>();
            m_raryties = GetModels<RarityModel>();
            m_itemSets = GetModels<ItemSet>();
            m_ratings = GetModels<RatingModel>();
            m_campaignChapters = GetModels<CampaignChapterModel>();
            m_locations = GetModels<LocationModel>();
            m_splinters = GetModels<SplinterModel>();
            m_products = GetModels<BaseProductModel>();
            m_markets = GetModels<MarketModel>();
            m_mines = GetModels<MineModel>();
            m_storageChallenges = GetModels<StorageChallengeModel>();
            m_resistances = GetModels<ResistanceModel>();
            m_achievments = GetModels<AchievmentModel>();
            m_itemRelations = GetModels<ItemRelationModel>();
            m_heroesCostLevelUps = GetModels<CostLevelUpContainer>();
            m_monthlyTasks = GetModels<MonthlyTasksModel>();
            m_gameTaskModels = GetModels<GameTaskModel>();
            m_buildings = GetModels<BuildingModel>();
            m_rewards = GetModels<RewardModel>();
            m_fortuneRewardModels = GetModels<FortuneRewardModel>();
            m_gameTaskModels = GetModels<GameTaskModel>();
            m_dailyRewardDatas = GetModels<DailyRewardModel>();
            m_achievmentContainers = GetModels<AchievmentContainerModel>();
            m_mineRestrictions = GetModels<MineRestrictionModel>();
            m_travelRaceCampaigns = GetModels<TravelRaceModel>();
            m_rewardContainerModels = GetModels<RewardContainerModel>();
            m_guildBossContainers = GetModels<GuildBossContainer>();
            m_avatarModels = GetModels<AvatarModel>();
            m_ratingUpContainers = GetModels<RatingUpContainer>();
            m_characteristicModels = GetModels<CharacteristicModel>();
            m_hireContainerModels = GetModels<HireContainerModel>();
            m_locationModels = GetModels<LocationModel>();
            m_helpResourceModels = GetModels<HelpResourceModel>();
        }

        private Dictionary<string, T> GetModels<T>() where T : BaseModel
        {
            var jsonData = TextUtils.GetTextFromLocalStorage<T>();
            return TextUtils.FillDictionary<T>(jsonData, m_converter);
        }
    }
}