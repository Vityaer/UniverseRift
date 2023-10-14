using City.Buildings.TravelCircle;
using System.Collections.Generic;

namespace Models.City.TravelCircle
{
    [System.Serializable]
    public class TravelRaceModel : BaseModel
    {
        public string Race;
        public List<MissionWithSmashReward> Missions = new List<MissionWithSmashReward>();
    }
}
