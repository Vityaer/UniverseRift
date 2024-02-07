using City.Panels.MonthTasks.Abstractions;

namespace City.Panels.MonthTasks.Taskboards
{
    public class MonthTaskboardPanelController : BaseAchievmentPanelController<MonthTaskboardPanelView>
    {
        protected override string AcievmentContainerName => "MonthTaskboardTasks";
    }
}
