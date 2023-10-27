using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using City.Panels.BatllepasPanels;
using City.Panels.Messages;
using Common.Inventories.Splinters;
using Common.Resourses;
using Common.Rewards;
using UIController.Inventory;
using UIController.ItemVisual;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace City.Buildings.CityButtons.DailyReward
{
    public class DailyRewardUI : ScrollableUiView<GameReward>
    {
        public int ID;
        public GameObject blockPanel, readyForGet;
        public SubjectCell rewardController;
        private BaseMarketProduct reward;
        private ScrollableViewStatus statusReward = ScrollableViewStatus.Close;

        void Start()
        {
            ID = transform.GetSiblingIndex();
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
                    readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Close:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(false);
                    break;
                case ScrollableViewStatus.Open:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(true);
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
                    reward.GetProduct(1);
                    //DailyRewardPanelController.Instance.OnGetReward(transform.GetSiblingIndex());
                    SetStatus(ScrollableViewStatus.Completed);
                    break;
            }
        }
    }
}