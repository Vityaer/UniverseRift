using Common.Rewards;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine.UI;

namespace City.Buildings.Guild.BossRaidRewardPanels
{
    public class BossRewardView : ScrollableUiView<GameReward>
    {
        public RewardUIController RewardController;
        public TMP_Text Label;

        public void SetData(GameReward data, int startIndex, int finishIndex, ScrollRect scrollRect)
        {
            Label.text = startIndex == finishIndex
                ? $"{startIndex}"
                : $"{startIndex} - {finishIndex}";
            
            RewardController.ShowReward(data);
            base.SetData(data, scrollRect);
        }
    }
}