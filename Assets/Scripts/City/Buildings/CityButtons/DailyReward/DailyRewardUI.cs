using Assets.Scripts.City.Buildings.Market;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Market;
using Common.Resourses;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Panels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace City.Buildings.CityButtons.DailyReward
{
    public class DailyRewardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int ID;
        public GameObject blockPanel, readyForGet;
        [SerializeField] private ParentScroll scrollParent;
        public SubjectCellController rewardController;
        private BaseMarketProduct reward;
        private EventAgentRewardStatus statusReward = EventAgentRewardStatus.Close;

        void Start()
        {
            ID = transform.GetSiblingIndex();
        }

        public void SetData(BaseMarketProduct newProduct)
        {
            switch (newProduct)
            {
                case MarketProduct<Resource> product:
                    reward = product;
                    rewardController.SetItem(product.subject);
                    break;
                case MarketProduct<Item> product:
                    reward = product;
                    rewardController.SetItem(product.subject);
                    break;
                case MarketProduct<SplinterModel> product:
                    reward = product;
                    rewardController.SetItem(product.subject);
                    break;
            }
        }

        public void SetStatus(EventAgentRewardStatus newStatusReward)
        {
            statusReward = newStatusReward;
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (statusReward)
            {
                case EventAgentRewardStatus.Received:
                    blockPanel.SetActive(true);
                    readyForGet.SetActive(false);
                    break;
                case EventAgentRewardStatus.Close:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(false);
                    break;
                case EventAgentRewardStatus.Open:
                    blockPanel.SetActive(false);
                    readyForGet.SetActive(true);
                    break;
            }
        }

        public void GetReward()
        {
            switch (statusReward)
            {
                case EventAgentRewardStatus.Close:
                    MessageController.Instance.AddMessage("Награда не открыта, приходите позже");
                    break;
                case EventAgentRewardStatus.Received:
                    MessageController.Instance.AddMessage("Вы уже получали эту награду");
                    break;
                case EventAgentRewardStatus.Open:
                    reward.GetProduct(1);
                    DailyController.Instance.OnGetReward(transform.GetSiblingIndex());
                    SetStatus(EventAgentRewardStatus.Received);
                    break;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            scrollParent.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            scrollParent.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            scrollParent.OnEndDrag(eventData);
        }
    }
}