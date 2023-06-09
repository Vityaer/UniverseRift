using Models.Fights.Campaign;
using UnityEngine;

namespace City.Buildings.Voyage
{
    public class VoyageMissionController : MonoBehaviour
    {
        [SerializeField] private StatusMission _status = StatusMission.NotOpen;
        private MissionModel _mission;
        private int _numMission = 0;

        public void SetData(MissionModel mission, int numMission, StatusMission newStatus)
        {
            this._mission = mission;
            this._numMission = numMission;
            _status = newStatus;
        }

        public void SetStatus(StatusMission newStatus)
        {
            _status = newStatus;
        }

        public void OpenPanelInfo()
        {
            VoyageController.Instance.ShowInfo(this, _mission.WinReward, _status);
        }

        public void OpenMission()
        {
            if (_status == StatusMission.Open)
            {
                VoyageController.Instance.OpenMission(_mission);
            }
        }
    }
}