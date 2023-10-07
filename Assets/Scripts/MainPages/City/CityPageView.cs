using City.Buildings.UiBuildings;
using City.General;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.City
{
    public class CityPageView : MainPage
    {
        [field: SerializeField] public BuildingVisual Tavern { get; private set; }
        [field: SerializeField] public BuildingVisual Market { get; private set; }
        [field: SerializeField] public BuildingVisual Altar { get; private set; }
        [field: SerializeField] public BuildingVisual Sanctuary { get; private set; }
        [field: SerializeField] public BuildingVisual PetZoo { get; private set; }
        [field: SerializeField] public BuildingVisual Guild { get; private set; }
        [field: SerializeField] public BuildingVisual Taskboard { get; private set; }
        [field: SerializeField] public BuildingVisual Forge { get; private set; }
        [field: SerializeField] public BuildingVisual Wheel { get; private set; }

    }
}
