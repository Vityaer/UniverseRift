using Models.Fights.Campaign;
using UnityEngine;

namespace City.Buildings.Voyage
{
    public class VoyageMissionController : MonoBehaviour
    {
        [SerializeField] private StatusMission status = StatusMission.NotOpen;
        private Mission mission;
        private int numMission = 0;

        public void SetData(Mission mission, int numMission, StatusMission newStatus)
        {
            this.mission = mission;
            this.numMission = numMission;
            status = newStatus;
        }

        public void SetStatus(StatusMission newStatus)
        {
            status = newStatus;
        }

        public void OpenPanelInfo()
        {
            VoyageControllerSctipt.Instance.ShowInfo(this, mission.WinReward, status);
        }

        public void OpenMission()
        {
            if (status == StatusMission.Open)
            {
                VoyageControllerSctipt.Instance.OpenMission(mission);
            }
        }
    }
}