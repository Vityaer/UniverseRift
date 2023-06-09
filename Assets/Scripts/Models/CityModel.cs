using System;

namespace Models
{
    [Serializable]
    public class CityModel : BaseModel
    {
        public TimeManagementModel timeManagement = new TimeManagementModel();
        public IndustryModel industry = new IndustryModel();
        public ShopModel mall = new ShopModel();
        public TaskGiverModel taskGiverBuilding = new TaskGiverModel();
        public BuildingWithFightTeamsModel challengeTowerBuilding = new BuildingWithFightTeamsModel();
        public BuildingWithFightTeamsModel mainCampaignBuilding = new BuildingWithFightTeamsModel();
        public BuildingWithFightTeamsModel travelCircleBuilding = new BuildingWithFightTeamsModel();
        public VoyageBuildingModel voyageBuildingSave = new VoyageBuildingModel();
        public ArenaBuildingModel arenaBuilding = new ArenaBuildingModel();
        public SimpleBuildingModel tutorial = new SimpleBuildingModel();
        public CycleEventsModel cycleEvents = new CycleEventsModel();
        public SimpleBuildingModel dailyReward = new SimpleBuildingModel();
    }
}
