using City.Buildings.Abstractions;
using City.Buildings.Voyage;
using System.Collections.Generic;

namespace City.Buildings.Mines.Panels.Travels
{
    public class MineTravelPanelView : BaseBuildingView
    {
        public List<MineMissionController> MissionViews = new();
        public MineMissionController BossMissionView;
    }
}
