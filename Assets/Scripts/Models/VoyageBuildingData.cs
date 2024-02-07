using Models.Fights.Campaign;
using System.Collections.Generic;

namespace Models
{
    [System.Serializable]
    public class VoyageBuildingData : BuildingWithFightTeamsData
    {
        public int CurrentMissionIndex;
        public List<MissionModel> Missions = new();
    }
}
