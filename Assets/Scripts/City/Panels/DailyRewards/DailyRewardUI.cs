using City.Buildings.CityButtons.EventAgent;
using Common.Rewards;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.DailyReward
{
    public class DailyRewardUI : ScrollableUiView<GameReward>
    {
        public GameObject blockPanel;
        public GameObject readyForGet;
        public TMP_Text Label;
        public RewardUIController RewardController;

        private ScrollableViewStatus statusReward = ScrollableViewStatus.Close;
        private int _id;

        public override void SetData(GameReward data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            RewardController.ShowReward(data);
            _id = transform.GetSiblingIndex();
            Label.text = $"Day {_id + 1}";
        }

        public override void SetStatus(ScrollableViewStatus newStatusReward)
        {
            statusReward = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (statusReward)
            {
                case ScrollableViewStatus.Completed:
                    blockPanel.SetActive(true);
                    //readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Close:
                    blockPanel.SetActive(false);
                    //readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Open:
                    blockPanel.SetActive(false);
                    //readyForGet.SetActive(true);
                    break;
            }
        }

        public void GetReward()
        {
            switch (statusReward)
            {
                case ScrollableViewStatus.Close:
                    //MessageController.Instance.AddMessage("Награда не открыта, приходите позже");
                    break;
                case ScrollableViewStatus.Completed:
                    //MessageController.Instance.AddMessage("Вы уже получали эту награду");
                    break;
                case ScrollableViewStatus.Open:
                    //DailyRewardPanelController.Instance.OnGetReward(transform.GetSiblingIndex());
                    SetStatus(ScrollableViewStatus.Completed);
                    break;
            }
        }
    }
}