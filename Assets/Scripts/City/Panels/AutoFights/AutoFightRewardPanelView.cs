using Ui.Misc.Widgets;
using UIController;
using UnityEngine.Localization.Components;

namespace City.Panels.AutoFights
{
    public class AutoFightRewardPanelView : BasePanel
    {
        public SliderTime AccumulationSlider;
        public LocalizeStringEvent AutoRewardGoldText;
        public LocalizeStringEvent AutoRewardStoneText;
        public LocalizeStringEvent AutoRewardExperienceText;
        public RewardUIController RewardUIController;
    }
}
