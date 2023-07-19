using City.Buildings.Voyage.Panels;
using Common.Rewards;
using Models.Fights.Campaign;
using TMPro;
using UIController;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;

namespace City.Buildings.Voyage
{
    public class VoyageMissionPanelController : UiPanelController<VoyageMissionPanelView>
    {
        private VoyageMissionController _controller;

        public void ShowInfo(VoyageMissionController controller, GameReward winReward, StatusMission status)
        {
            this._controller = controller;
            View.rewardController.ShowReward(winReward);
            View.currentObject?.SetActive(false);
            switch (status)
            {
                case StatusMission.NotOpen:
                    View.currentObject = View.textNotOpenMission;
                    break;
                case StatusMission.Open:
                    View.currentObject = View.btnOpenMission;
                    break;
                case StatusMission.Complete:
                    View.currentObject = View.textCompleteMission;
                    break;
                default:
                    Debug.Log("Error");
                    break;
            }
            View.currentObject?.SetActive(true);
        }

        public void OpenMission()
        {
            Close();
            _controller.OpenMission();
        }
    }
}