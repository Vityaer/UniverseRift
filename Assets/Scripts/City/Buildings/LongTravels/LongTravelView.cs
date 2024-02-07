using City.Buildings.Abstractions;
using City.Buildings.Voyage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.LongTravels
{
    public class LongTravelView : BaseBuildingView
    {
        public Button MainTravelButton;
        public Button TrainTravelButton;
        public Button HeroTravelButton;

        public GameObject MissionsPanel;
        public GameObject TravelPanel;
        public Button MissionPanelCloseButton;
        public List<VoyageMissionController> MissionViews = new();

        public SerializableDictionary<LongTravelType, LongTravelContainerView> TravelViews = new();
    }
}
