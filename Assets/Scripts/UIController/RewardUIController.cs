using City.Panels.BoxRewards;
using Common.Rewards;
using Models.Data.Rewards;
using System.Collections.Generic;
using UIController.Inventory;
using UIController.ItemVisual;
using UIController.Rewards;
using UnityEngine;

namespace UIController
{
    public class RewardUIController : MonoBehaviour
    {
        [SerializeField] private List<SubjectCell> Cells = new List<SubjectCell>();
        public Transform panelRewards;
        public GameObject btnAllReward;
        private GameReward _reward;

        public void ShowReward(RewardModel rewardData)
        {
            _reward = new GameReward(rewardData);
            ShowReward(_reward);
        }

        public void ShowAutoReward(AutoRewardData autoReward)
        {
            _reward = new GameReward(autoReward);
            ShowReward(_reward);
        }

        public void ShowReward(GameReward reward, bool lengthReward = false)
        {
            this._reward = reward;

            if (btnAllReward != null)
                btnAllReward.SetActive(reward.Objects.Count > 4 && lengthReward == false);

            for (int i = 0; i < 4 && i < reward.Objects.Count; i++) 
                Cells[i].SetData(reward.Objects[i]);

            for (int i = reward.Objects.Count; i < Cells.Count; i++)
                Cells[i].Disable();

            panelRewards.gameObject.SetActive(reward.Objects.Count > 0);
        }

        public void OpenAllReward()
        {
            //BoxRewardsPanelController.Instance.ShowAll(reward);
        }

        public void CloseReward()
        {
            gameObject.SetActive(false);
        }

        public void OpenReward()
        {
            gameObject.SetActive(true);
        }
    }
}