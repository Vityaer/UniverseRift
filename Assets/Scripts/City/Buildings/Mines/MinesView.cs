using City.Buildings.Abstractions;
using System.Collections.Generic;
using UnityEngine.UI;

namespace City.Buildings.Mines
{
    public class MinesView : BaseBuildingView
    {
        public List<PlaceForMine> MinePlaces = new List<PlaceForMine>();
        public Button CollectAllButton;
    }
}
