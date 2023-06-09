using Common;
using MainScripts;
using UIController;
using UIController.Rewards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace City.Buildings.CityButtons.EventAgent
{
    public class EventAgentReward : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Data")]
        public RewardUIController RewardController;

        private Reward _reward;
        public int ID;

        public void SetData(Reward reward)
        {
            this._reward = reward;
            RewardController.ShowReward(reward);
        }
        private EventAgentRewardStatus statusReward = EventAgentRewardStatus.Close;
        public void SetStatus(EventAgentRewardStatus newStatusReward)
        {
            statusReward = newStatusReward;
            UpdateUI();
        }
        public void GetReward()
        {
            switch (statusReward)
            {
                case EventAgentRewardStatus.Close:
                    MessageController.Instance.AddMessage("Награду ещё нужно заслужить, приходите позже");
                    break;
                case EventAgentRewardStatus.Received:
                    MessageController.Instance.AddMessage("Вы уже получали эту награду");
                    break;
                case EventAgentRewardStatus.Open:
                    GameController.Instance.AddReward(_reward);
                    EventAgentController.Instance.OnGetReward(ID);
                    SetStatus(EventAgentRewardStatus.Received);
                    break;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            EventAgentController.Instance.scrollRectController.OnBeginDrag(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            EventAgentController.Instance.scrollRectController.OnDrag(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            EventAgentController.Instance.scrollRectController.OnEndDrag(eventData);
        }
        [Header("UI")]
        public Image background;
        public Image blockPanel;
        public Color colorClose, colorOpen, colorReceive;
        private void UpdateUI()
        {
            switch (statusReward)
            {
                case EventAgentRewardStatus.Close:
                    blockPanel.color = colorClose;
                    break;
                case EventAgentRewardStatus.Received:
                    blockPanel.color = colorReceive;
                    break;
                case EventAgentRewardStatus.Open:
                    blockPanel.color = colorOpen;
                    break;
            }
        }
    }
    public enum EventAgentRewardStatus
    {
        Close = 0,
        Open = 1,
        Received = 2
    }
}