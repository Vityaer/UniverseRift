using City.Buildings.CityButtons.EventAgent;
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
        [field: SerializeField] public DailytaskProgressSlider mainSliderController { get; private set; }
        [field: SerializeField] public ScrollRect Scroll { get; private set; }
        [field: SerializeField] public DailyTaskRewardView PrefabRewardUi { get; private set; }
        

    }
}
