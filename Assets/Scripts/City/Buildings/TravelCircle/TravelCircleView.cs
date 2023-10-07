using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleView : BaseBuildingView
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
        [field: SerializeField] public TravelCircleMissionController MissionPrefab { get; private set; }
        public List<TravelRaceCampaignButton> TravelRaceCampaignButtons = new List<TravelRaceCampaignButton>();
        
        public RectTransform MainCircle;
        public Button OpenListButton;
    }
}
