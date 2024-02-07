using City.Panels.MonthTasks.Abstractions;
using Models.Events;

namespace City.Panels.Events.FortuneCycles
{
    public class FortuneCycleMainPanelController : BaseAchievmentPanelController<FortuneCycleMainPanelView>
    {
        protected override string AcievmentContainerName => "MainFortuneCycle";

        protected override void OnLoadGame()
        {
            if (CommonGameData.CycleEventsData.CurrentEventType != GameEventType.Fortune)
                return;

            base.OnLoadGame();
        }
    }
}
