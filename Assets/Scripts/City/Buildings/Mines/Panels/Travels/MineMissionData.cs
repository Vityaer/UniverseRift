using Fight.Common;

namespace City.Buildings.Mines.Panels.Travels
{
    public class MineMissionData : MissionData
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string StorageMissionContainerId { get; set; }
        public string MissionId { get; set; } = string.Empty;
        public string UnitsStateJSON { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
