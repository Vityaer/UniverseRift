using Models.City.LongTravels;
using Models.Data.Buildings.FortuneWheels;
using Models.Data.Buildings.Markets;
using Models.Data.Buildings.Taskboards;
using Models.Data.Dailies;
using Models.MainCampaign;
using Models.TravelRaceDatas;
using Network.DataServer.Models.Guilds;
using System;

namespace Models
{
    [Serializable]
    public class CityData : BaseModel
    {
        public TimeManagementData TimeManagementSave = new();
        public IndustryData IndustrySave = new();
        public MarketData MallSave = new();
        public BuildingWithFightTeamsData ChallengeTowerSave = new();
        public MainCampaignBuildingData MainCampaignSave = new();
        public TravelBuildingData TravelCircleSave = new();
        public VoyageBuildingData VoyageSave = new();
        public ArenaData ArenaSave = new();
        public SimpleBuildingData Tutorial = new();
        public CycleEventsData CycleEvents = new();
        public DailyRewardContainer DailyReward = new();
        public AchievmentStorageData Achievments = new();
        public FortuneWheelData FortuneWheelData = new();
        public TaskBoardData TaskBoardData = new();
        public GuildPlayerSaveContainer GuildPlayerSaveContainer = new();
        public LongTravelData LongTravelData = new();
    }
}
