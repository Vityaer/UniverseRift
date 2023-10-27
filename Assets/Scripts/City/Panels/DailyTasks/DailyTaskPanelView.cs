using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Requirement;
using Ui.Misc.Widgets;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.DailyTasks
{
    public class DailyTaskPanelView : BasePanel
    {
        [field: SerializeField] public Transform contentScrollRect { get; private set; }
        [field: SerializeField] public ItemSliderController miniSliderAmount { get; private set; }
        [field: SerializeField] public ScrollRect Scroll { get; private set; }
        [field: SerializeField] public RectTransform Content { get; private set; }
        [field: SerializeField] public AchievmentView AchievmentViewPrefab { get; private set; }
        [field: SerializeField] public Button OpenBattlePasPanelButton { get; private set; }
    }
}
