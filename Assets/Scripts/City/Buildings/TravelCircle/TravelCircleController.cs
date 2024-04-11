using City.Buildings.Abstractions;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models.Common;
using Models.Fights.Campaign;
using Models.TravelRaceDatas;
using Network.DataServer;
using Network.DataServer.Messages.TravelCircles;
using System.Collections.Generic;
using UIController.Cards;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleController : BuildingWithFight<TravelCircleView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly IObjectResolver _objectResolver;

        private const int SHOW_MISSION_COUNT = 10;

        private List<TravelCircleMissionController> _missionsUI = new();
        private TravelRaceCampaignButton _currentCampaingSelector;
        private bool _loadCurrentTravel;
        private TravelBuildingData _travelCircleSave;
        private TravelRaceData _currentTravelData;
        private TravelCircleMissionController _selectedMissionUi;

        public void Initialize()
        {
            for (var i = 0; i < SHOW_MISSION_COUNT; i++)
            {
                var missionUi = UnityEngine.Object.Instantiate(View.MissionPrefab, View.Content);
                _objectResolver.Inject(missionUi);

                missionUi.OnSelect.Subscribe(OnMissionSelect).AddTo(Disposables);
                _missionsUI.Add(missionUi);
                missionUi.SetData(null, View.ScrollRect);
            }

            foreach (var campaignSelector in View.TravelRaceCampaignButtons)
            {
                campaignSelector.OnSelect.Subscribe(OnSelectCampaign).AddTo(Disposables);
            }

            View.OpenListButton.OnClickAsObservable().Subscribe(_ => OpenTravel()).AddTo(Disposables);
            View.CloseListButton.OnClickAsObservable().Subscribe(_ => CloseTravel()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            _travelCircleSave = CommonGameData.City.TravelCircleSave;

            foreach (var travelData in _travelCircleSave.TravelRaceDatas)
            {
                var campaignSelector = View.TravelRaceCampaignButtons
                    .Find(selector => selector.RaceId.Equals(travelData.RaceId));

                if (campaignSelector == null)
                {
                    Debug.LogError($"Unknow race: {travelData.RaceId}");
                    continue;
                }

                var travelModel = _commonDictionaries.TravelRaceCampaigns[$"{travelData.RaceId}Travel"];
                var raceContainer = _commonDictionaries.Races[travelData.RaceId];


                var icon = SpriteUtils.LoadSprite(raceContainer.SpritePath);
                campaignSelector.SetData(travelModel, travelData.MissionIndexCompleted);
                campaignSelector.SetRaceIcon(icon);
            }

            var randomIndex = Random.Range(0, View.TravelRaceCampaignButtons.Count);
            OnSelectCampaign(View.TravelRaceCampaignButtons[randomIndex]);
        }

        private void LoadMissions()
        {
            if (_loadCurrentTravel)
                return;

            var currentMission = 0;

            var currentTravelData = _travelCircleSave.TravelRaceDatas
                .Find(data => data.RaceId.Equals(_currentCampaingSelector.RaceId));

            if (currentTravelData != null)
            {
                _currentTravelData = currentTravelData;
                currentMission = currentTravelData.MissionIndexCompleted + 1;
            }

            var currentTravelModel = _commonDictionaries.TravelRaceCampaigns[$"{_currentCampaingSelector.RaceId}Travel"];
            for (int i = 0; i < currentMission; i++)
            {
                _missionsUI[i].SetData(currentTravelModel.Missions[i], View.ScrollRect, i + 1);
                //_missionsUI[i].SetCanSmash();
                _missionsUI[i].Hide();
            }

            for (int i = currentMission; i < currentTravelModel.Missions.Count && i < _missionsUI.Count; i++)
            {
                _missionsUI[i].SetData(currentTravelModel.Missions[i], View.ScrollRect, i + 1);
            }
            for (int i = currentTravelModel.Missions.Count; i < _missionsUI.Count; i++)
            {
                _missionsUI[i].Hide();
            }

            _missionsUI[currentMission].OpenForFight();
            _loadCurrentTravel = true;
        }

        private void OnSelectCampaign(TravelRaceCampaignButton newSelector)
        {
            if (_currentCampaingSelector == newSelector)
                return;

            _currentCampaingSelector?.Diselect();
            _currentCampaingSelector = newSelector;
            _currentCampaingSelector.Select();
            _loadCurrentTravel = false;
        }

        private void OnMissionSelect(TravelCircleMissionController missionUi)
        {
            _selectedMissionUi = missionUi;

            switch (missionUi.Status)
            {
                case StatusMission.Open:
                    OpenMission(missionUi.GetData);
                    break;
                case StatusMission.InAutoFight:
                    SmashMission(missionUi).Forget();
                    break;
            }
        }

        private async UniTaskVoid SmashMission(TravelCircleMissionController mission)
        {
            var message = new MissionRaceTravelSmashMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                TravelId = _currentTravelData.Id,
                MissionIndex = mission.Index - 1,
                Count = 1
            };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(mission.GetData.SmashReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                SendData().Forget();

                _selectedMissionUi.SetCanSmash();
                _currentTravelData.MissionIndexCompleted += 1;
                _missionsUI[_currentTravelData.MissionIndexCompleted].OpenForFight();

                var travelModel = _commonDictionaries.TravelRaceCampaigns[$"{_currentTravelData.RaceId}Travel"];
                _currentCampaingSelector.SetData(travelModel, _currentTravelData.MissionIndexCompleted);
            }
            else
            {
                _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }

        private async UniTaskVoid SendData()
        {
            var message = new MissionRaceTravelCompleteMessage { PlayerId = CommonGameData.PlayerInfoData.Id, TravelId = _currentTravelData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(_selectedMissionUi.GetData.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward);
            }
        }

        private void OpenTravel()
        {
            LoadMissions();
            View.MissionsPanel.SetActive(true);
        }


        private void CloseTravel()
        {
            View.MissionsPanel.SetActive(false);
        }
    }
}