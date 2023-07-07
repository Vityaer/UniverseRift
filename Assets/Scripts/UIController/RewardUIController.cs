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
        private GameReward reward;

        public void ShowReward(GameReward reward, bool lengthReward = false)
        {
            //this.reward = reward;
            //if (cells.Count == 0) GetCells();
            //if (btnAllReward != null) btnAllReward.SetActive(reward.Count > 4 && lengthReward == false);
            //for (int i = 0; i < 4 && i < reward.Count; i++) cells[i].SetItem(reward.GetList[i] as VisualAPI);
            //for (int i = reward.Count; i < cells.Count; i++) cells[i].OffCell();
            //panelRewards.gameObject.SetActive(reward.Count > 0);
        }

        public void ShowAllReward(GameReward reward)
        {
            this.reward = reward;
            if (Cells.Count == 0) GetCells();
            for (int i = 0; i < reward.Objects.Count; i++)
            {
                Cells[i].SetData(reward.Objects[i]);
            }

            for (int i = reward.Objects.Count; i < Cells.Count; i++)
            {
                Cells[i].Disable();
            }

            panelRewards.gameObject.SetActive(reward.Objects.Count > 0);
        }

        public void ShowAutoReward(GameReward autoReward)
        {
            if (Cells.Count == 0)
                GetCells();
            List<PosibleRewardObject> listPosibleObject = new List<PosibleRewardObject>();
            //autoReward.GetListPosibleRewards(listPosibleObject);

            //if (btnAllReward != null)
            //    btnAllReward.SetActive(listPosibleObject.Count > 4);

            //for (int i = 0; i < 4 && i < listPosibleObject.Count; i++)
            //    cells[i].SetItem(listPosibleObject[i]);

            //for (int i = listPosibleObject.Count; i < cells.Count; i++)
            //    cells[i].OffCell();
            //panelRewards.gameObject.SetActive(listPosibleObject.Count > 0);
        }

        void GetCells()
        {
            foreach (Transform cell in panelRewards)
            {
                Cells.Add(cell.GetComponent<SubjectCell>());
            }
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