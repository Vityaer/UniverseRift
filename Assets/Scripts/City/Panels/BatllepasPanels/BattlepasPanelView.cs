using City.Buildings.CityButtons.EventAgent;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasPanelView : BasePanel
    {
        [field: SerializeField] public ScrollRect Scroll { get; private set; }
        [field: SerializeField] public DailytaskProgressSlider MainSliderController { get; private set; }
        [field: SerializeField] public BattlepasRewardView BattlepasRewardViewPrefab { get; private set; }
        [field: SerializeField] public RectTransform Content { get; private set; }
        
    }
}
