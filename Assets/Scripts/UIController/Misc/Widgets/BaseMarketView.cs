using City.Buildings.Abstractions;
using City.Buildings.Market;
using UnityEngine;

namespace UIController.Misc.Widgets
{
    public class BaseMarketView : BaseBuildingView
    {
        public Transform Content;
        public MarketProductController Prefab;
    }
}
