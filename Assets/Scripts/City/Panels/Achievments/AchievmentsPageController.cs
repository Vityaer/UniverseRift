using System.Linq;
using City.Buildings.Requirement;
using City.Panels.MonthTasks.Abstractions;

namespace City.Panels.Achievments
{
    public class AchievmentsPageController : BaseAchievmentPanelController<AchievmentsPanelView>
    {
        protected override string AcievmentContainerName => "MainAchievments";
    }
}