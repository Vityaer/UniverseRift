using City.Buildings.Abstractions;
using City.Buildings.TravelCircle.PanelMissions;
using Common;
using Fight;
using Fight.WarTable;
using Models;
using Models.City.TravelCircle;
using Models.Fights.Campaign;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Model;
using VContainerUi.Services;
using VContainerUi.Messages;
using Models.Common;
using UIController.Cards;
using System;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleController : BuildingWithFight<TravelCircleView>, IInitializable
    {
        private const int SHOW_MISSION_COUNT = 10;

        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly TravelMissionsPanelController panelTravelListMissions;
        [Inject] private readonly IUiMessagesPublisherService _messagesPublisher;

        private List<TravelCircleMissionController> _missionsUI = new List<TravelCircleMissionController>();
        private TravelRaceCampaignButton _currentCampaingSelector;

        private List<TravelRaceModel> _travels = new List<TravelRaceModel>();
        private TravelRaceModel _currentTravel;
        BuildingWithFightTeamsData _travelCircleSave = null;

        public void Initialize()
        {
            for (var i = 0; i < SHOW_MISSION_COUNT; i++)
            {
                var missionUi = UnityEngine.Object.Instantiate(View.MissionPrefab, View.Content);
                _missionsUI.Add(missionUi);
            }

            foreach (var campaignSelector in View.TravelRaceCampaignButtons)
            {
                campaignSelector.OnSelect.Subscribe(OnSelectCampaign).AddTo(Disposables);
            }

            View.OpenListButton.OnClickAsObservable().Subscribe(_ => OpenTravel()).AddTo(Disposables);
        }

        protected override void OnStart()
        {
        }

        protected override void OnLoadGame()
        {
            _travelCircleSave = _commonGameData.City.TravelCircleSave;
            foreach (TravelRaceModel travel in _travels)
                travel.CurrentMission = _travelCircleSave.IntRecords.GetRecord(travel.GetNameRecord);

            //ChangeTravel(_travels[Random.Range(0, _travels.Count)].race);
        }

        protected override void OpenPage()
        {
            LoadMissions(_currentTravel.missions, _currentTravel.CurrentMission);
        }

        private void LoadMissions(List<MissionWithSmashReward> missions, int currentMission)
        {
            for (int i = 0; i < currentMission - 1; i++)
            {
                _missionsUI[i].SetData(missions[i], i + 1);
                _missionsUI[i].SetCanSmash();
            }
            for (int i = currentMission; i < missions.Count && i < _missionsUI.Count; i++)
            {
                _missionsUI[i].SetData(missions[i], i + 1);
            }
            for (int i = missions.Count; i < _missionsUI.Count; i++)
            {
                _missionsUI[i].Hide();
            }
            _missionsUI[currentMission].OpenForFight();
        }

        private void OnSelectCampaign(TravelRaceCampaignButton newSelector)
        {
            _currentCampaingSelector?.Diselect();
            _currentCampaingSelector = newSelector;
            _currentCampaingSelector.Select();
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _currentTravel.OpenNextMission();
                _travelCircleSave.IntRecords.SetRecord(_currentTravel.GetNameRecord, _currentTravel.CurrentMission);
                //SaveGame();
                LoadMissions(_currentTravel.missions, _currentTravel.CurrentMission);
            }
        }

        public void ChangeTravel(string newRace)
        {
            //if (_currentTravel == null || _currentTravel.race != newRace)
            //{
            //    _currentTravel = _travels.Find(x => x.race == newRace);
            //    _currentTravel.controllerUI.Select();
            //    LoadMissions(_currentTravel.missions, _currentTravel.CurrentMission);
            //}
        }

        public void OpenTravel()
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<TravelMissionsPanelController>(openType: OpenType.Additive);
        }

    }
}