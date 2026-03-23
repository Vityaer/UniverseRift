using City.Buildings.CityButtons.EventAgent;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataCheckUtils;

namespace City.Panels.DailyTasks
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

        private ScrollableViewStatus m_status;

        public ScrollableViewStatus Status => m_status;

        public override void SetData(GameReward data, ScrollRect scroll, CommonDictionaries dictionaries)
        {
            Scroll = scroll.ThrowIfArgumentNull();
            CommonDataDictionaries = dictionaries.ThrowIfArgumentNull();
            RewardController.ShowReward(data, CommonDataDictionaries);
        }

        public void SetStatus(ScrollableViewStatus newStatusReward)
        {
            m_status = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (m_status)
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