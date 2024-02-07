using Models.Events;
using UiExtensions.Panels;

namespace City.Panels.Events.SweetCycles
{
    public class SweetCycleMainPanelController : BaseMarketController<SweetCycleMainPanelView>
    {
        protected override string MarketContainerName => "SweetCycleMarket";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.Sweet)
                return;

            View.ObserverCycleCandy.TypeResource = Common.Resourses.ResourceType.Candy;
            View.ObserverCycleCandy.Construct();
            base.OnLoadGame();
        }
    }
}
