using City.Buildings.Abstractions;
using City.Buildings.Market;
using UnityEngine.UI;

namespace UIController.Misc.Widgets
{
    public class BaseMarketView : BaseBuildingView
    {
        public ScrollRect ScrollRect;
        public MarketProductController Prefab;
    }
}
