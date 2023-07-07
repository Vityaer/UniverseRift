using City.Buildings.Abstractions;
using Common;
using Common.Rewards;
using Fight;
using Fight.WarTable;
using Models;
using Models.Common;
using Models.Common.BigDigits;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using UIController.Rewards;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.Voyage
{
    public class VoyageController : BuildingWithFight<VoyageView>
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";

        [Inject] private CommonGameData _commonGameData;
        [Inject] private WarTableController _warTableController;

        [SerializeField] private VoyageMissionPanelController panelVoyageMission;
        [SerializeField] private List<MissionModel> missions = new List<MissionModel>();
        [SerializeField] private List<VoyageMissionController> missionsUI = new List<VoyageMissionController>();

        private VoyageBuildingData voyageBuildingSave = null;
        private int _currentMission = 0;
        private ReactiveCommand<int> _onDoneTravel = new ReactiveCommand<int>();
        public LocationWithBuildings locationController;

        public IObservable<int> OnDoneTravel => _onDoneTravel;

        protected override void OnLoadGame()
        {
            voyageBuildingSave = _commonGameData.City.VoyageSave;
            _currentMission = voyageBuildingSave.IntRecords.GetRecord(NAME_RECORD_NUM_CURRENT_MISSION);
            LoadMissions();
        }

        private void LoadMissions()
        {
            StatusMission statusMission = StatusMission.NotOpen;
            for (int i = 0; i < missions.Count; i++)
            {
                if (i < _currentMission)
                {
                    statusMission = StatusMission.Complete;
                }
                else if (i == _currentMission)
                {
                    statusMission = StatusMission.Open;
                }
                else
                {
                    statusMission = StatusMission.NotOpen;
                }

                missionsUI[i].SetData(i + 1, statusMission);
            }
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(_currentMission);
                missionsUI[_currentMission].SetStatus(StatusMission.Complete);
                _currentMission += 1;
                if (_currentMission < missions.Count) missionsUI[_currentMission].SetStatus(StatusMission.Open);
                voyageBuildingSave.IntRecords.SetRecord(NAME_RECORD_NUM_CURRENT_MISSION, _currentMission);
                //SaveGame();

                if (_currentMission == missions.Count)
                    _onDoneTravel.Execute(1);
            }
        }

        public void ShowInfo(VoyageMissionController controller, GameReward winReward, StatusMission status)
        {
            panelVoyageMission.ShowInfo(controller, winReward, status);
        }
    }
}