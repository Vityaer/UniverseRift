using Common;
using UnityEngine;

namespace UIController.MessagePanels
{
    public class RewardPanel : MonoBehaviour
    {
        protected Reward reward;
        public GameObject panel;
        public RewardUIController rewardController;

        public void Open(Reward reward)
        {
            if (reward != null)
                SetReward(reward);

            panel.SetActive(true);
        }

        protected virtual void SetReward(Reward reward)
        {
            this.reward = reward.Clone();
            rewardController.ShowAllReward(reward);
        }

        private void GetReward()
        {
            GameController.Instance.AddReward(reward);
            reward = null;
        }

        public void Close()
        {
            MessageController.Instance.OpenNextPanel();

            if (reward != null)
                GetReward();

            OnClose();
            panel.SetActive(false);
        }

        protected virtual void OnClose() { }
    }
}