using Models.Events;

namespace Models
{
    [System.Serializable]
    public class CycleEventsData : SimpleBuildingData
    {
        public GameEventType CurrentEventType;
        public string StartGameCycleDateTime = string.Empty;
        public string LastGetAlchemyDateTime = string.Empty;
    }
}
