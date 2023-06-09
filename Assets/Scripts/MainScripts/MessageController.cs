using Common;
using GeneralObject;
using System;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.MessagePanels;
using UIController.MessagePanels.TinyPanels;
using UIController.Rewards;
using UnityEngine;

namespace MainScripts
{
    public partial class MessageController : MonoBehaviour
    {
        private static MessageController instance;
        private Canvas canvas;

        public static MessageController Instance => instance;

        //	Messages
        [SerializeField] private ErrorMessageController errorMessageController;
        public void ShowErrorMessage(string errorText) { errorMessageController.ShowMessage(errorText); }
        //	Result fight
        [SerializeField] private RewardPanel rewardPanel, winPanel, losePanel;
        [SerializeField] private AutoRewardPanel autoRewardPanel;
        [SerializeField] private PanelNewLevelPlayer panelNewLevelPlayer;

        private Queue<PanelRecord> queuePanels = new Queue<PanelRecord>();
        private PanelRecord currentPanel = null;

        void Awake()
        {
            canvas = GetComponent<Canvas>();
            instance = this;
        }

        public void OpenWin(Reward reward, Action delOnClose)
        {
            AddQueue(winPanel, () => winPanel.Open(reward), delOnClose);
        }

        public void OpenDefeat(Reward reward, Action delOnClose)
        {
            AddQueue(losePanel, () => losePanel.Open(reward), delOnClose);
        }

        public void OpenAutoReward(AutoReward autoReward, Reward calculatedReward, DateTime previousDateTime)
        {
            AddQueue(autoRewardPanel, () => autoRewardPanel.Open(autoReward, calculatedReward, previousDateTime));
        }

        public void OpenPanelNewLevel(Reward reward)
        {
            AddQueue(panelNewLevelPlayer, () => panelNewLevelPlayer.Open(reward));
        }

        public void OpenSimpleRewardPanel(Reward reward)
        {
            AddQueue(rewardPanel, () => rewardPanel.Open(reward));
        }

        public void AddMessage(string newMessage, bool isLong = false)
        {
            Debug.Log(newMessage);
            AndroidPlugin.PluginController.ToastPlugin.Show(newMessage, isLong: isLong);
        }

        //Queue panel reward

        private void AddQueue(RewardPanel panel, Action delOnpen, Action delOnClose = null)
        {
            queuePanels.Enqueue(new PanelRecord(panel, delOnpen, delOnClose));
            if (queuePanels.Count == 1) OpenNextPanel();
        }

        public void OpenNextPanel()
        {
            if (currentPanel != null) currentPanel.OnClose();
            if (queuePanels.Count > 0)
            {
                currentPanel = queuePanels.Dequeue();
                currentPanel.Open();
            }
        }

        //Queue tiny reward
        [Header("Tiny rewards")]
        public TinyResourceRewardPanel tinyResourceReward;
        public TinyItemRewardPanel tinyItemReward;
        public TinySplinterRewardPanel tinySplinterReward;
        public void OpenTinyRewards(Reward reward)
        {
            GameController.Instance.AddReward(reward);
            for (int i = 0; i < reward.GetListResource.List.Count; i++)
                AddQueueTinyRewards(tinyResourceReward, () => tinyResourceReward.Open(reward.GetListResource.List[i]));

            List<Item> items = reward.GetItems;
            for (int i = 0; i < items.Count; i++)
                AddQueueTinyRewards(tinyItemReward, () => tinyItemReward.Open(items[i]));

            List<Splinter> splinters = reward.GetSplinters;
            for (int i = 0; i < splinters.Count; i++)
                AddQueueTinyRewards(tinySplinterReward, () => tinySplinterReward.Open(splinters[i]));
        }
        private Queue<PanelTinyRecord> queueTinyPanels = new Queue<PanelTinyRecord>();
        PanelTinyRecord currentTinyPanel = null;
        private void AddQueueTinyRewards(TinyRewardPanel panel, Action delOnpen)
        {
            queueTinyPanels.Enqueue(new PanelTinyRecord(panel, delOnpen));
            if (queueTinyPanels.Count == 1) OpenNextPanel();
        }
        public void OpenNextTinyPanel()
        {
            if (currentTinyPanel != null) currentTinyPanel.Close();
            if (queueTinyPanels.Count > 0)
            {
                currentTinyPanel = queueTinyPanels.Dequeue();
                currentTinyPanel.Open();
            }
        }
    }
}