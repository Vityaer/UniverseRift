using System;
using TMPro;

public class AutoRewardPanel : RewardPanel
{
    public SliderTimeScript sliderAccumulation;
    public TextMeshProUGUI textAutoRewardGold, textAutoRewardStone, textAutoRewardExperience;
    private TimeSpan maxTime = new TimeSpan(12, 0, 0);

    public void Open(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime)
    {
        textAutoRewardGold.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Gold).ToString(), "/ 5sec");
        textAutoRewardStone.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.ContinuumStone).ToString(), "/ 5sec");
        textAutoRewardExperience.text = string.Concat(autoReward.resources.List.Find(x => x.Name == TypeResource.Exp).ToString(), "/ 5sec");
        base.Open(calculatedReward);
        sliderAccumulation.SetData(previousDateTime, maxTime);
    }

    protected override void OnClose()
    {
        AutoFight.Instance.heap.GetReward();
    }
}