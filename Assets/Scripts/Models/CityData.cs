using Models.Data.Buildings.FortuneWheels;
using Models.Data.Buildings.Markets;
using Models.Data.Buildings.Taskboards;
using System;

namespace Models
{
    [Serializable]
    public class CityData : BaseModel
    {
        public TimeManagementData TimeManagementSave = new TimeManagementData();
        public IndustryModel IndustrySave = new IndustryModel();
        public MarketData MallSave = new MarketData();
        public BuildingWithFightTeamsData ChallengeTowerSave = new BuildingWithFightTeamsData();
        public BuildingWithFightTeamsData MainCampaignSave = new BuildingWithFightTeamsData();
        public BuildingWithFightTeamsData TravelCircleSave = new BuildingWithFightTeamsData();
        public VoyageBuildingData VoyageSave = new VoyageBuildingData();
        public ArenaBuildingModel ArenaSave = new ArenaBuildingModel();
        public SimpleBuildingData Tutorial = new SimpleBuildingData();
        public CycleEventsData CycleEvents = new CycleEventsData();
        public SimpleBuildingData DailyReward = new SimpleBuildingData();
        public AchievmentStorageData Achievments = new AchievmentStorageData();
        public FortuneWheelData FortuneWheelData = new FortuneWheelData();
        public TaskBoardData TaskBoardData = new TaskBoardData();

    }
}
