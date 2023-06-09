using City.Buildings.General;
using Common;
using Fight;
using Fight.WarTable;
using Models;
using Models.City.TravelCircle;
using Models.Fights.Campaign;
using System.Collections.Generic;
using UIController.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleScript : Building
    {
        [SerializeField] private List<TravelRaceModel> travels = new List<TravelRaceModel>();
        private TravelRaceModel currentTravel;
        BuildingWithFightTeamsModel travelCircleSave = null;
        [Header("UI")]
        public List<TravelCircleMissionControllerScript> missionsUI = new List<TravelCircleMissionControllerScript>();
        public RectTransform mainCircle;
        public PanelTravelListMissions panelListMissions;
        public ParentScroll scrollRectController;
        public Button OpenListButton;

        public static TravelCircleScript Instance { get => instance; }
        private static TravelCircleScript instance;
        void Awake() { instance = this; }

        protected override void OnStart()
        {
            OpenListButton.onClick.AddListener(() => OpenTravel());
        }

        protected override void OnLoadGame()
        {
            travelCircleSave = GameController.GetCitySave.travelCircleBuilding;
            foreach (TravelRaceModel travel in travels)
                travel.CurrentMission = travelCircleSave.GetRecordInt(travel.GetNameRecord);

            ChangeTravel(travels[Random.Range(0, travels.Count)].race);
        }

        protected override void OpenPage()
        {
            LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
        }

        public void OpenMission(MissionModel mission)
        {
            FightController.Instance.RegisterOnFightResult(OnResultFight);
            WarTableController.Instance.OpenMission(mission, OnAfterFight);
        }

        public void OnAfterFight(bool isOpen)
        {
            if (!isOpen)
            {
                WarTableController.Instance.UnregisterOnOpenCloseMission(OnAfterFight);
                Open();
            }
            else
            {
                Close();
            }
        }

        private void LoadMissions(List<MissionWithSmashReward> missions, int currentMission)
        {
            for (int i = 0; i < currentMission - 1; i++)
            {
                missionsUI[i].SetData(missions[i], i + 1);
                missionsUI[i].SetCanSmash();
            }
            for (int i = currentMission; i < missions.Count && i < missionsUI.Count; i++)
            {
                missionsUI[i].SetData(missions[i], i + 1);
            }
            for (int i = missions.Count; i < missionsUI.Count; i++)
            {
                missionsUI[i].Hide();
            }
            missionsUI[currentMission].OpenForFight();
        }

        public void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                currentTravel.OpenNextMission();
                travelCircleSave.SetRecordInt(currentTravel.GetNameRecord, currentTravel.CurrentMission);
                SaveGame();
                LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
            }
        }

        public void ChangeTravel(string newRace)
        {
            if (currentTravel == null || currentTravel.race != newRace)
            {
                currentTravel = travels.Find(x => x.race == newRace);
                currentTravel.controllerUI.Select();
                LoadMissions(currentTravel.missions, currentTravel.CurrentMission);
            }
        }

        public void OpenTravel()
        {
            Debug.Log("open travel");
            panelListMissions.Open();
        }

    }
}