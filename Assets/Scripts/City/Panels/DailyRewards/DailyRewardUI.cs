using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using City.Panels.Messages;
using Common.Inventories.Splinters;
using Common.Resourses;
using UIController.Inventory;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.EventSystems;

namespace City.Buildings.CityButtons.DailyReward
{
    public class DailyRewardUI : MonoBehaviour
    {
        public int ID;
        public GameObject blockPanel, readyForGet;
        public SubjectCell rewardController;
        private BaseMarketProduct reward;
        private DailyTaskRewardStatus statusReward = DailyTaskRewardStatus.Close;

        void Start()
        {
            ID = transform.GetSiblingIndex();
        }

        public void SetData(BaseMarketProduct newProduct)
        {
            switch (newProduct)
            {
                case MarketProduct<GameResource> product:
                    reward = product;
                    //rewardController.SetItem(product.subject);
                    break;
                case MarketProduct<GameItem> product:
                    reward = product;
                    //rewardController.SetItem(product.subject);
                    break;
                case MarketProduct<GameSplinter> product:
                    reward = product;
                    //rewardController.SetItem(product.subject);
                    break;
            }
        }

        public void SetStatus(DailyTaskRewardStatus newStatusReward)
        {
            statusReward = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (statusReward)
            {
                case DailyTaskRewardStatus.Received:
                    blockPanel.SetActive(true);
                    readyForGet.SetActive(false);
                    break;
                case DailyTaskRewardStatus.Close:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(false);
                    break;
                case DailyTaskRewardStatus.Open:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(true);
                    break;
            }
        }

        public void GetReward()
        {
            switch (statusReward)
            {
                case DailyTaskRewardStatus.Close:
                    //MessageController.Instance.AddMessage("Награда не открыта, приходите позже");
                    break;
                case DailyTaskRewardStatus.Received:
                    //MessageController.Instance.AddMessage("Вы уже получали эту награду");
                    break;
                case DailyTaskRewardStatus.Open:
                    reward.GetProduct(1);
                    //DailyRewardPanelController.Instance.OnGetReward(transform.GetSiblingIndex());
                    SetStatus(DailyTaskRewardStatus.Received);
                    break;
            }
        }
    }
}