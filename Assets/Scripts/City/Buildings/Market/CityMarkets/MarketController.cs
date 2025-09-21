using UiExtensions.Panels;

namespace City.Buildings.Market.CityMarkets
{
    public class MarketController : BaseMarketController<MarketView>
    {
        protected override string MarketContainerName => "CityMarket";
    }
}