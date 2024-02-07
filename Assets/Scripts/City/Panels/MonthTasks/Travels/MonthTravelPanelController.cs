using City.Panels.MonthTasks.Abstractions;

namespace City.Panels.MonthTasks.Travels
{
    public class MonthTravelPanelController : BaseAchievmentPanelController<MonthTravelPanelView>
    {
        protected override string AcievmentContainerName => "MonthTravelTasks";
    }
}
