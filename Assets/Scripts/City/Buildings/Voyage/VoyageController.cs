using City.Buildings.General;
using Common;
using Fight;
using Fight.WarTable;
using Models;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using UIController.Rewards;
using UnityEngine;

namespace City.Buildings.Voyage
{
    public class VoyageController : BuildingWithFight
    {
        [SerializeField] private List<MissionModel> missions = new List<MissionModel>();
        [SerializeField] private List<VoyageMissionController> missionsUI = new List<VoyageMissionController>();
        [SerializeField] private PanelVoyageMission panelVoyageMission;
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";
        VoyageBuildingModel voyageBuildingSave = null;
        int currentMission = 0;
        private Action<BigDigit> observerDoneTravel;
        public LocationWithBuildings locationController;
        private static VoyageController instance;
        public static VoyageController Instance { get => instance; }
        void Awake() { instance = this; }

        protected override void OnLoadGame()
        {
            voyageBuildingSave = GameController.GetCitySave.voyageBuildingSave;
            currentMission = voyageBuildingSave.GetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION);
            LoadMissions();
        }

        private void LoadMissions()
        {
            StatusMission statusMission = StatusMission.NotOpen;
            for (int i = 0; i < missions.Count; i++)
            {
                if (i < currentMission)
                {
                    statusMission = StatusMission.Complete;
                }
                else if (i == currentMission)
                {
                    statusMission = StatusMission.Open;
                }
                else { statusMission = StatusMission.NotOpen; }
                missionsUI[i].SetData(missions[i], i + 1, statusMission);
            }
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                OnWinFight(currentMission);
                missionsUI[currentMission].SetStatus(StatusMission.Complete);
                currentMission += 1;
                if (currentMission < missions.Count) missionsUI[currentMission].SetStatus(StatusMission.Open);
                voyageBuildingSave.SetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, currentMission);
                SaveGame();
                if (currentMission == missions.Count) OnDoneTravel(1);
            }
        }

        public void ShowInfo(VoyageMissionController controller, Reward winReward, StatusMission status)
        {
            panelVoyageMission.ShowInfo(controller, winReward, status);
        }

        public void RegisterOnDoneTravel(Action<BigDigit> d) { observerDoneTravel += d; }
        public void UnregisterOnDoneTravel(Action<BigDigit> d) { observerDoneTravel -= d; }

        protected void OnDoneTravel(int num)
        {
            if (observerDoneTravel != null)
                observerDoneTravel(new BigDigit(num));
        }

        protected override void OpenPage()
        {
            WarTableController.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
            locationController.Show();
        }
        protected override void ClosePage()
        {
            Debug.Log("close page");
            locationController.Hide();
        }

    }
}