using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController.Campaign;

namespace City.Buildings.WorldMaps
{
    public class WorldMapView : BaseBuildingView
    {
        public List<CampaignChapterController> Chapters = new List<CampaignChapterController>();
    }
}
