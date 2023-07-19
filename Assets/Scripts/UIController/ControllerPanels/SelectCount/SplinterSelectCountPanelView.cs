using Ui.Misc.Widgets;
using UIController.ControllerPanels.CountControllers;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ControllerPanels.SelectCount
{
    public class SplinterSelectCountPanelView : BasePanel
    {
        [field: SerializeField] public SubjectCell MainImage { get; private set; }
        [field: SerializeField] public ItemSliderController CountSlider { get; private set; }
        [field: SerializeField] public CountPanelController CountController { get; private set; }
        [field: SerializeField] public Button ActionButton { get; private set; }
    }
}
