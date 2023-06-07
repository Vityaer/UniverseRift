using Assets.Scripts.City.Buildings.Tower;
using City.Buildings.General;
using Common;
using Models;
using Models.Fights.Campaign;
using System.Collections.Generic;
using UIController.Panels;

namespace City.Buildings.Tower
{
    public class ChallengeTower : BuildingWithFight
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";

        public int countShowMission = 8;
        public ListMissions listMissions;
        public List<TowerMissionCotrollerScript> missionsUI = new List<TowerMissionCotrollerScript>();
        public List<TowerMissionCotrollerScript> bottomMissionsUI = new List<TowerMissionCotrollerScript>();
        public List<TowerMissionCotrollerScript> topMissionsUI = new List<TowerMissionCotrollerScript>();
        public ParentScroll scrollRectController;
        int currentMission = 0;
        private List<MissionModel> workMission = new List<MissionModel>();
        private BuildingWithFightTeamsModel challengeTower = null;

        protected override void OnLoadGame()
        {
            challengeTower = GameController.GetCitySave.challengeTowerBuilding;
            currentMission = challengeTower.GetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION);
            LoadMissions();
        }

        int skipStart = 2;
        private void LoadMissions()
        {
            skipStart = currentMission > bottomMissionsUI.Count ? bottomMissionsUI.Count : 0;
            for (int i = currentMission; i < currentMission + countShowMission && i < listMissions.Count; i++)
                workMission.Add(listMissions.missions[i]);
            FillData();
        }
        private void FillData()
        {
            // for(int i= 0; (i < skipStart) && (i < bottomMissionsUI.Count); i++){
            // 	bottomMissionUI[i].SetData(null, currentMission - (i + 1));
            // }

            for (int i = 0; i < missionsUI.Count && i < workMission.Count; i++)
            {
                missionsUI[i].SetData(workMission[i], currentMission + i + 1, i == 0);
            }
        }
        void Awake() { instance = this; }
        public static ChallengeTower Instance { get => instance; }
        private static ChallengeTower instance;



        //After fight

        protected override void OnResultFight(FightResult result)
        {
            if (result == FightResult.Win)
            {
                OnWinFight(currentMission);
                currentMission += 1;
                challengeTower.SetRecordInt(NAME_RECORD_NUM_CURRENT_MISSION, currentMission);
                LoadMissions();
                SaveGame();
            }
            OnTryFight();
        }
    }
}