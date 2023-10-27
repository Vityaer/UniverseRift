using Common.Rewards;
using TMPro;
using UIController;
using UIController.Rewards;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailyTaskUi : ScrollableUiView<GameReward>
    {
        [Header("UI")]
        public RewardUIController RewardController;
        public Image Background;
        public Image BlockPanel;
        public Color CloseColor;
        public Color OpenColor;
        public Color ReceiveColor;
        public TMP_Text Name;
        public TMP_Text Description;

        private GameReward _reward;
        private ScrollableViewStatus _status;

        public ScrollableViewStatus Status => _status;

        public override void SetData(GameReward data, ScrollRect scroll)
        {
            _reward = data;
            Scroll = scroll;
            RewardController.ShowReward(data);
        }

        public void SetStatus(ScrollableViewStatus newStatusReward)
        {
            _status = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (_status)
            {
                case ScrollableViewStatus.Close:
                    BlockPanel.color = CloseColor;
                    break;
                case ScrollableViewStatus.Completed:
                    BlockPanel.color = ReceiveColor;
                    break;
                case ScrollableViewStatus.Open:
                    BlockPanel.color = OpenColor;
                    break;
            }
        }
    }
}