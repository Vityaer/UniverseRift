using City.Buildings.UiBuildings;
using City.General;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.SecondCity
{
    public class SecondCityPageView : MainPage
    {
        public Canvas BackgroundCanvas;
        [field: SerializeField] public BuildingVisual TowerDeath { get; private set; }
        [field: SerializeField] public BuildingVisual TravelCirle { get; private set; }
        [field: SerializeField] public BuildingVisual MagicCircle { get; private set; }
        [field: SerializeField] public BuildingVisual Mines { get; private set; }
        [field: SerializeField] public BuildingVisual Voyage { get; private set; }
        [field: SerializeField] public BuildingVisual Arena { get; private set; }
        [field: SerializeField] public BuildingVisual RaidTravels { get; private set; }
    }
}
