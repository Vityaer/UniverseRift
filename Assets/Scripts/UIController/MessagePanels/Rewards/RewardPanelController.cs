using City.Panels.Messages;
using Common;
using Common.Rewards;
using UIController.MessagePanels.Rewards;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;

namespace UIController.MessagePanels
{
    public class RewardPanelController : UiPanelController<RewardPanelView>
    {
        protected GameReward reward;

        public void Open(GameReward reward)
        {
            if (reward != null)
                SetReward(reward);

            View.panel.SetActive(true);
        }

        protected virtual void SetReward(GameReward reward)
        {
            //this.reward = reward.Clone();
            View.rewardController.ShowAllReward(reward);
        }

        private void GetReward()
        {
            //GameController.Instance.AddReward(reward);
            reward = null;
        }

        public void Close()
        {
            //MessageController.Instance.OpenNextPanel();

            if (reward != null)
                GetReward();

            OnClose();
            View.panel.SetActive(false);
        }

        protected virtual void OnClose() { }
    }
}