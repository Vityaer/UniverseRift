using City.Buildings.Abstractions;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.Voyages;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer;

namespace City.Buildings.Voyage
{
    public class VoyageController : BuildingWithFight<VoyageView>
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";

        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly VoyageMissionPanelController panelVoyageMission;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private List<MissionModel> _missions = new();

        private int _currentMission = 0;
        private VoyageBuildingData voyageBuildingSave = null;
        public ReactiveCommand<int> _onTravelComplete = new();
        private MissionModel _currentMissionModel;

        public LocationWithBuildings locationController;

        public IObservable<int> OnTravelComplete => _onTravelComplete;

        protected override void OnLoadGame()
        {
            voyageBuildingSave = _commonGameData.City.VoyageSave;
            _currentMission = voyageBuildingSave.CurrentMissionIndex;
            _missions = voyageBuildingSave.Missions;

            if (_missions.Count > 0)
            {
                LoadMissions();
            }
            else
            {
                ShowMessageHide();
            }
        }

        private void ShowMessageHide()
        {
            View.MessagePanel.SetActive(true);
            View.MissionContentPanel.SetActive(false);
        }

        private void LoadMissions()
        {
            StatusMission statusMission = StatusMission.NotOpen;
            for (int i = 0; i < _missions.Count; i++)
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

                View.MissionViews[i].SetData(i + 1, statusMission);
                View.MissionViews[i].OnSelect.Subscribe(OpenVoyageMissionPanel).AddTo(Disposables);
            }
        }

        private void OpenVoyageMissionPanel(int index)
        {
            var mission = View.MissionViews[index];
            _currentMissionModel = _missions[index];
            var reward = new GameReward(_missions[index].WinReward, _commonDictionaries);
            panelVoyageMission.ShowInfo(reward, mission.Status, index, StartOpenMission);
        }

        private void StartOpenMission()
        {
            OpenMission(_currentMissionModel);
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(_currentMission);
                View.MissionViews[_currentMission].SetStatus(StatusMission.Complete);
                _currentMission += 1;

                if (_currentMission < _missions.Count)
                    View.MissionViews[_currentMission].SetStatus(StatusMission.Open);

                voyageBuildingSave.IntRecords.SetRecord(NAME_RECORD_NUM_CURRENT_MISSION, _currentMission);

                if (_currentMission == _missions.Count)
                    _onTravelComplete.Execute(1);

                SendData().Forget();
            }
        }

        private async UniTaskVoid SendData()
        {
            var message = new VoyageFinishMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(_currentMissionModel.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward);
            }
        }
    }
}