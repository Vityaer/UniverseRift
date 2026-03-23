using City.Buildings.Requirements;
using Ui.Misc.Widgets;
using UnityEngine.UI;
using UnityEngine;

namespace City.Panels.MonthTasks.Abstractions
{
    public class BaseAchievmentPanelView : BasePanel
    {
        public Transform Content;
        public ScrollRect Scroll;
        public AchievmentView Prefab;
    }
}
