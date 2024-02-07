using City.Panels.MonthTasks.Abstractions;
using Models.Events;

namespace City.Panels.Events.RaceCircleCycles
{
    public class RaceCicrleCycleMainPanelController : BaseAchievmentPanelController<RaceCicrleCycleMainPanelView>
    {
        protected override string AcievmentContainerName => "MainRaceHireCycle";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.RaceCircle)
                return;

            base.OnLoadGame();
        }
    }
}
