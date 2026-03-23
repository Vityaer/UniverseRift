using Misc.Json;
using Models;
using Models.City.Markets;
using Models.Events;
using UiExtensions.Panels;
using VContainer;

namespace City.Panels.Events.SweetCycles
{
    public class SweetCycleMainPanelController : BaseMarketController<SweetCycleMainPanelView>
    {
        [Inject] private readonly IJsonConverter m_jsonConverter;

        protected override string MarketContainerName => "SweetCycleMarket";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.Sweet)
                return;

            SweetEventData eventData = m_jsonConverter
                .Deserialize<SweetEventData>(CommonGameData.CycleEventsData.CurrentCycle);

            MarketModel sweetMarket = CommonDictionaries.Markets[MarketContainerName];

            foreach (string sweetGoodId in sweetMarket.Products)
            {
                CommonDictionaries.Products[sweetGoodId].Cost.Type = eventData.ResourceType;
            }

            View.ObserverCycleCandy.TypeResource = eventData.ResourceType;
            View.ObserverCycleCandy.Construct();
            base.OnLoadGame();
        }
    }
}
