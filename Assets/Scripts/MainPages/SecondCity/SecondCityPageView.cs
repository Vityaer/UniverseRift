using City.General;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.SecondCity
{
    public class SecondCityPageView : MainPage
    {
        [field: SerializeField] public Button TowerDeathButton { get; private set; }
        [field: SerializeField] public Button TravelCirleButton { get; private set; }
        [field: SerializeField] public Button MagicCircleButton { get; private set; }
        [field: SerializeField] public Button MinesButton { get; private set; }
        [field: SerializeField] public Button VoyageButton { get; private set; }
        [field: SerializeField] public Button ArenaButton { get; private set; }
    }
}
