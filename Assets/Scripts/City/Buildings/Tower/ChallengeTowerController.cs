using City.Buildings.Abstractions;
using Common;
using Fight;
using Models;
using Models.Fights.Campaign;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer.Unity;

namespace City.Buildings.Tower
{
    public class ChallengeTowerController : BuildingWithFight<ChallengeTowerView>, IInitializable
    {
        private const int SHOW_MISSION_COUNT = 8;
        private const int SHOW_COMPLETED_MISSION_COUNT = 2;
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";

        public ListMissions listMissions;
        public List<TowerMissionCotroller> _missionsUI = new List<TowerMissionCotroller>();

        private int _currentMissionIndex = 0;
        private List<MissionModel> workMission = new List<MissionModel>(SHOW_MISSION_COUNT);
        private BuildingWithFightTeamsData challengeTower = null;
        private ReactiveCommand<int> _onMissionWin = new ReactiveCommand<int>();

        public ReactiveCommand OnTryMission = new ReactiveCommand();
        public IObservable<int> OnMissionWin => _onMissionWin;

        public void Initialize()
        {
            for (var i = 0; i < SHOW_MISSION_COUNT + SHOW_COMPLETED_MISSION_COUNT; i++)
            {
                var missionUi = UnityEngine.Object.Instantiate(View.MissionPrefab, View.Content);
                _missionsUI.Add(missionUi);
            }
        }

        protected override void OnLoadGame()
        {
            challengeTower = CommonGameData.City.ChallengeTowerSave;
            _currentMissionIndex = challengeTower.IntRecords.GetRecord(NAME_RECORD_NUM_CURRENT_MISSION);
            //LoadMissions();
        }

        private void LoadMissions()
        {
            workMission = new List<MissionModel>(SHOW_MISSION_COUNT);
            for (int i = _currentMissionIndex; i < _currentMissionIndex + SHOW_MISSION_COUNT && i < listMissions.Count; i++)
                workMission.Add(listMissions.missions[i]);
            FillData();
        }

        private void FillData()
        {
            for (int i = 0; i < _missionsUI.Count && i < workMission.Count; i++)
            {
                _missionsUI[i].SetData(workMission[i], _currentMissionIndex + i + 1, i == 0);
            }
        }

        //After fight

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onMissionWin.Execute(_currentMissionIndex);
                _currentMissionIndex += 1;
                challengeTower.IntRecords.SetRecord(NAME_RECORD_NUM_CURRENT_MISSION, _currentMissionIndex);
                LoadMissions();
                //SaveGame();
            }

            OnTryMission.Execute();
        }
    }
}