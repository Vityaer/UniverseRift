using Misc.Json;
using Models;
using Models.Common;
using Models.Events;
using UiExtensions.Panels;
using UnityEngine;
using VContainer;

namespace City.Panels.Events.SweetCycles
{
    public class SweetCycleMainPanelController : BaseMarketController<SweetCycleMainPanelView>
    {
        [Inject] private readonly IJsonConverter _jsonConverter;

        protected override string MarketContainerName => "SweetCycleMarket";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.Sweet)
                return;

            SweetEventData eventData = _jsonConverter
                .Deserialize<SweetEventData>(CommonGameData.CycleEventsData.CurrentCycle);

            var sweetMarket = _commonDictionaries.Markets[MarketContainerName];

            foreach (var sweetGoodId in sweetMarket.Products)
            {
                _commonDictionaries.Products[sweetGoodId].Cost.Type = eventData.ResourceType;
            }

            
            View.ObserverCycleCandy.TypeResource = eventData.ResourceType;
            View.ObserverCycleCandy.Construct();
            base.OnLoadGame();
        }
    }
}
