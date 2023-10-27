using City.Buildings.CityButtons.DailyReward;
using Ui.Misc.Widgets;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.DailyRewards
{
    public class DailyRewardPanelView : BasePanel
    {
        [field: SerializeField] public ScrollRect Scroll { get; private set; }
        public SliderTime SliderTime;
        public DailyRewardUI RewardPrefab;
        public RectTransform Content;
    }
}
