using City.Panels.BoxRewards;
using Common.Rewards;
using Db.CommonDictionaries;
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

        public void ShowReward(RewardModel rewardData, CommonDictionaries commonDictionaries)
        {
            _reward = new GameReward(rewardData, commonDictionaries);
            ShowReward(_reward);
        }

        public void ShowAutoReward(AutoRewardData autoReward, CommonDictionaries commonDictionaries)
        {
            _reward = new GameReward(autoReward, commonDictionaries);
            ShowReward(_reward);
        }

        public void ShowReward(GameReward reward, bool lengthReward = false)
        {
            _reward = reward;
            var count = 0;

            if (lengthReward)
            {
                count = reward.Objects.Count;
            }
            else
            {
                count = Mathf.Min(4, reward.Objects.Count);
            }

            if (btnAllReward != null)
                btnAllReward.SetActive(reward.Objects.Count > 4 && lengthReward == false);

            for (int i = 0; i < count; i++) 
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