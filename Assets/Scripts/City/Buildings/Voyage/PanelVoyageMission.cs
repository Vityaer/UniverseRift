using Models.Fights.Campaign;
using TMPro;
using UIController;
using UIController.Rewards;
using UnityEngine;

namespace City.Buildings.Voyage
{
    public class PanelVoyageMission : MonoBehaviour
    {
        public GameObject panel;
        public TextMeshProUGUI textNameMission;
        public RewardUIController rewardController;

        [SerializeField] private GameObject btnOpenMission, textNotOpenMission, textCompleteMission;

        private GameObject currentObject;
        private VoyageMissionController controller;

        public void ShowInfo(VoyageMissionController controller, Reward winReward, StatusMission status)
        {
            this.controller = controller;
            rewardController.ShowReward(winReward);
            currentObject?.SetActive(false);
            switch (status)
            {
                case StatusMission.NotOpen:
                    currentObject = textNotOpenMission;
                    break;
                case StatusMission.Open:
                    currentObject = btnOpenMission;
                    break;
                case StatusMission.Complete:
                    currentObject = textCompleteMission;
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            currentObject?.SetActive(true);
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
        }

        public void OpenMission()
        {
            Close();
            controller.OpenMission();
        }
    }
}