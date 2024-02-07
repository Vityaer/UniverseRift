using City.Achievements;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Requirement;
using City.Panels.MonthTasks.Abstractions;
using Db.CommonDictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using UiExtensions.Misc;
using UiExtensions.Scroll.Interfaces;
using VContainer;
using VContainer.Unity;

namespace City.Panels.MonthTasks.Arena
{
    public class MonthArenaPanelController : BaseAchievmentPanelController<MonthArenaPanelView>, IStartable
    {
        protected override string AcievmentContainerName => "MonthArenaTasks";
    }
}
