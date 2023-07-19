using Common.Rewards;
using UIController;
using UIController.Rewards;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailyTaskRewardView : ScrollableUiView<GameReward>
    {
        [Header("UI")]
        public RewardUIController RewardController;
        public Image Background;
        public Image BlockPanel;
        public Color CloseColor;
        public Color OpenColor;
        public Color ReceiveColor;

        private GameReward _reward;
        private DailyTaskRewardStatus _status;

        public DailyTaskRewardStatus Status => _status;

        public override void SetData(GameReward data, ScrollRect scroll)
        {
            _reward = data;
            Scroll = scroll;
            RewardController.ShowReward(data);
        }

        public void SetStatus(DailyTaskRewardStatus newStatusReward)
        {
            _status = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (_status)
            {
                case DailyTaskRewardStatus.Close:
                    BlockPanel.color = CloseColor;
                    break;
                case DailyTaskRewardStatus.Received:
                    BlockPanel.color = ReceiveColor;
                    break;
                case DailyTaskRewardStatus.Open:
                    BlockPanel.color = OpenColor;
                    break;
            }
        }
    }
}