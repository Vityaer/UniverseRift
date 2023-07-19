using City.General;
using UnityEngine;
using UnityEngine.UI;

namespace MainPages.City
{
    public class CityPageView : MainPage
    {
        [field: SerializeField] public Button TavernButton { get; private set; }
        [field: SerializeField] public Button MarketButton { get; private set; }
        [field: SerializeField] public Button AltarButton { get; private set; }
        [field: SerializeField] public Button SanctuaryButton { get; private set; }
        [field: SerializeField] public Button PetZooButton { get; private set; }
        [field: SerializeField] public Button GuildButton { get; private set; }
        [field: SerializeField] public Button TaskboardButton { get; private set; }
        [field: SerializeField] public Button ForgeButton { get; private set; }
        [field: SerializeField] public Button WheelButton { get; private set; }

    }
}
