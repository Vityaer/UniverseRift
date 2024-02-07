using City.Achievements;
using City.Panels.MonthTasks.Abstractions;
using Common;
using Db.CommonDictionaries;
using Models.Achievments;
using Models.Common;
using Models.Data;
using System.Collections.Generic;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Requirement
{
    public class AchievmentsPageController : BaseAchievmentPanelController<AchievmentsPanelView>
    {
        protected override string AcievmentContainerName => "MainAchievments";
    }
}