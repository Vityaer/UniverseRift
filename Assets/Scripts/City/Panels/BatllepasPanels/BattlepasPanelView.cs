using System.Collections.Generic;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.Widgets.Particles;
using Ui.Misc.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasPanelView : BasePanel
    {
        public List<RectTransform> RectsForReset = new List<RectTransform>();
        [field: SerializeField] public RectTransform SliderRect { get; private set; }
        [field: SerializeField] public ScrollRect Scroll { get; private set; }
        [field: SerializeField] public DailytaskProgressSlider MainSliderController { get; private set; }
        [field: SerializeField] public BattlepasRewardView BattlepasRewardViewPrefab { get; private set; }
        [field: SerializeField] public RectTransform Content { get; private set; }
    }
}
