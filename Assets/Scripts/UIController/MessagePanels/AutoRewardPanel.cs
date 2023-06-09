using Campaign;
using Common.Resourses;
using GeneralObject;
using System;
using TMPro;
using UIController.Rewards;

namespace UIController.MessagePanels
{
    public class AutoRewardPanel : RewardPanel
    {
        public SliderTime sliderAccumulation;
        public TextMeshProUGUI textAutoRewardGold, textAutoRewardStone, textAutoRewardExperience;
        private TimeSpan maxTime = new TimeSpan(12, 0, 0);

        public void Open(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime)
        {
            textAutoRewardGold.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Gold).ToString(), "/ 5sec");
            textAutoRewardStone.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.ContinuumStone).ToString(), "/ 5sec");
            textAutoRewardExperience.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Exp).ToString(), "/ 5sec");
            Open(calculatedReward);
            sliderAccumulation.SetData(previousDateTime, maxTime);
        }

        protected override void OnClose()
        {
            AutoFight.Instance.heap.GetReward();
        }
    }
}