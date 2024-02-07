using City.Panels.Events.TavernCycleMainPanels;
using City.Panels.MonthTasks.Abstractions;
using Models.Events;
using UiExtensions.Scroll.Interfaces;

namespace MainPages.Events.Cycles.TavernCycles.Panels
{
    public class TavernCycleMainPanelController : BaseAchievmentPanelController<TavernCycleMainPanelView>
    {
        protected override string AcievmentContainerName => "MainTavernHireCycle";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.Tavern)
                return;

            base.OnLoadGame();
        }
    }
}
