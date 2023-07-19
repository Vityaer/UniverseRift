using City.Buildings.Abstractions;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleView : BaseBuildingView
    {
        [field: SerializeField] public RectTransform Content { get; private set; }
        [field: SerializeField] public TravelCircleMissionController MissionPrefab { get; private set; }

        public RectTransform MainCircle;
        public Button OpenListButton;
    }
}
