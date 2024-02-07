using UiExtensions.Panels;

namespace City.Buildings.Market
{
    public class MarketController : BaseMarketController<MarketView>
    {
        protected override string MarketContainerName => "CityMarket";
    }
}