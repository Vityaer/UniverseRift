using City.Buildings.TravelCircle;
using System.Collections.Generic;

namespace Models.City.TravelCircle
{
    [System.Serializable]
    public class TravelRaceModel
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";
        public string race;
        public TravelSelectScript controllerUI;
        public List<MissionWithSmashReward> missions = new List<MissionWithSmashReward>();
        private int currentMission = 0;
        public int CurrentMission { get => currentMission; set => currentMission = value; }
        public string GetNameRecord { get => string.Concat(NAME_RECORD_NUM_CURRENT_MISSION, race.ToString()); }

        public void OpenNextMission()
        {
            currentMission += 1;
        }
    }
}
