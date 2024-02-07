using City.Panels.MonthTasks.Abstractions;

namespace City.Panels.MonthTasks.Evolutions
{
    public class MonthEvolutionPanelController : BaseAchievmentPanelController<MonthEvolutionPanelView>
    {
        protected override string AcievmentContainerName => "MonthEvolutionTasks";
    }
}
