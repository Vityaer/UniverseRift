using City.Buildings.Abstractions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Voyage
{
    public class VoyageView : BaseBuildingView
    {
        public List<VoyageMissionController> MissionViews = new();

        public GameObject MessagePanel;
        public GameObject MissionContentPanel;
        public Button ShopButton;
    }
}
