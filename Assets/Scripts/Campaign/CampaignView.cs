using City.Buildings.Abstractions;
using UnityEngine;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace Campaign
{
    public class CampaignView : BaseBuildingView
    {
        public RectTransform Content;
        public MissionController Prefab;
        public Button WorldMapButton; 
    }
}
