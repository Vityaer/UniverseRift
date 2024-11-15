﻿using Models.City.LongTravels;
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
        public TimeManagementData TimeManagementSave = new TimeManagementData();
        public IndustryData IndustrySave = new IndustryData();
        public MarketData MallSave = new MarketData();
        public BuildingWithFightTeamsData ChallengeTowerSave = new BuildingWithFightTeamsData();
        public MainCampaignBuildingData MainCampaignSave = new();
        public TravelBuildingData TravelCircleSave = new TravelBuildingData();
        public VoyageBuildingData VoyageSave = new VoyageBuildingData();
        public ArenaBuildingModel ArenaSave = new ArenaBuildingModel();
        public SimpleBuildingData Tutorial = new SimpleBuildingData();
        public CycleEventsData CycleEvents = new CycleEventsData();
        public DailyRewardContainer DailyReward = new DailyRewardContainer();
        public AchievmentStorageData Achievments = new AchievmentStorageData();
        public FortuneWheelData FortuneWheelData = new FortuneWheelData();
        public TaskBoardData TaskBoardData = new TaskBoardData();
        public GuildPlayerSaveContainer GuildPlayerSaveContainer = new();
        public LongTravelData LongTravelData = new();
    }
}
