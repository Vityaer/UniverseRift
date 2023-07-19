using System;

namespace Models
{
    [Serializable]
    public class CityData : BaseModel
    {
        public TimeManagementData TimeManagementSave = new TimeManagementData();
        public IndustryModel IndustrySave = new IndustryModel();
        public ShopModel MallSave = new ShopModel();
        public TaskGiverModel TaskboardSave = new TaskGiverModel();
        public BuildingWithFightTeamsData ChallengeTowerSave = new BuildingWithFightTeamsData();
        public BuildingWithFightTeamsData MainCampaignSave = new BuildingWithFightTeamsData();
        public BuildingWithFightTeamsData TravelCircleSave = new BuildingWithFightTeamsData();
        public VoyageBuildingData VoyageSave = new VoyageBuildingData();
        public ArenaBuildingModel ArenaSave = new ArenaBuildingModel();
        public SimpleBuildingData Tutorial = new SimpleBuildingData();
        public CycleEventsData CycleEvents = new CycleEventsData();
        public SimpleBuildingData DailyReward = new SimpleBuildingData();
        public AchievmentStorageData Achievments = new AchievmentStorageData();

    }
}
