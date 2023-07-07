using City.General;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.City
{
    public class CityPageView : MainPage
    {
        [field: SerializeField] public Button TavernButton { get; private set; }
        [field: SerializeField] public Button MarketButton { get; private set; }
        [field: SerializeField] public Button ArenaButton { get; private set; }
        [field: SerializeField] public Button TowerDeathButton { get; private set; }
        [field: SerializeField] public Button AltarButton { get; private set; }
        [field: SerializeField] public Button TravelCirleButton { get; private set; }
        [field: SerializeField] public Button SanctuaryButton { get; private set; }
        [field: SerializeField] public Button PetZooButton { get; private set; }
        [field: SerializeField] public Button GuildButton { get; private set; }
        [field: SerializeField] public Button VoyageButton { get; private set; }
        [field: SerializeField] public Button TaskboardButton { get; private set; }
        [field: SerializeField] public Button MinesButton { get; private set; }
        [field: SerializeField] public Button MagicCircleButton { get; private set; }
        [field: SerializeField] public Button ForgeButton { get; private set; }
        [field: SerializeField] public Button WheelButton { get; private set; }

    }
}
