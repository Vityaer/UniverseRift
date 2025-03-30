using UiExtensions.Panels;

namespace City.Buildings.Voyage.Shops
{
    public class VoyageShopController : BaseMarketController<VoyageShopView>
    {
        protected override string MarketContainerName => "VoyageMarket";
    }
}