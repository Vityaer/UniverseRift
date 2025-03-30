using System;
using System.Collections.Generic;
using City.Buildings.Abstractions;
using City.Buildings.Voyage.Shops;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Fight.WarTable;
using Models;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.Voyages;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.Voyage
{
    public class VoyageController : BuildingWithFight<VoyageView>, IInitializable
    {
        private const int MINIMAL_HEROES_LEVEL = 40;

        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly VoyageMissionPanelController panelVoyageMission;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ResourceStorageController m_resourceManager;

        private List<MissionModel> _missions = new();

        private int _currentMission = 0;
        private VoyageBuildingData voyageBuildingSave = null;
        private MissionModel _currentMissionModel;

        public ReactiveCommand<int> _onTravelComplete = new();
        public LocationWithBuildings locationController;

        public IObservable<int> OnTravelComplete => _onTravelComplete;

        public VoyageController()
        {
            List<IWarLimiter> warLimiters = new();
            warLimiters.Add(new MinorLevelLimiter(MINIMAL_HEROES_LEVEL));
            WarTableLimiter = new WarTableLimiter(warLimiters);
        }
        
        public void Initialize()
        {
            View.ShopButton.OnClickAsObservable().Subscribe(_ => OpenShop()).AddTo(Disposables);
        }

        private void OpenShop()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<VoyageShopController>(openType: OpenType.Additive);
        }

        protected override void OnLoadGame()
        {
            voyageBuildingSave = _commonGameData.City.VoyageSave;
            _currentMission = voyageBuildingSave.CurrentMissionIndex;
            _missions = voyageBuildingSave.Missions;

            if (_missions.Count > 0)
                LoadMissions();
            else
                ShowMessageHide();
        }

        private void ShowMessageHide()
        {
            View.MessagePanel.SetActive(true);
            View.MissionContentPanel.SetActive(false);
        }

        private void LoadMissions()
        {
            var statusMission = StatusMission.NotOpen;
            for (var i = 0; i < _missions.Count; i++)
            {
                if (i < _currentMission)
                    statusMission = StatusMission.Complete;
                else if (i == _currentMission)
                    statusMission = StatusMission.Open;
                else
                    statusMission = StatusMission.NotOpen;

                View.MissionViews[i].SetData(i + 1, statusMission);
                View.MissionViews[i].OnSelect.Subscribe(OpenVoyageMissionPanel).AddTo(Disposables);
            }
        }

        private void OpenVoyageMissionPanel(int index)
        {
            var mission = View.MissionViews[index];
            _currentMissionModel = _missions[index];

            var status = mission.Status;
            panelVoyageMission.ShowInfo(_currentMissionModel, status, index, StartOpenMission);
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

                voyageBuildingSave.CurrentMissionIndex = _currentMission;

                if (_currentMission == _missions.Count)
                    _onTravelComplete.Execute(1);

                SendFinishMissionData().Forget();
            }
            else
            {
                _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }

        private async UniTaskVoid SendFinishMissionData()
        {
            var message = new VoyageFinishMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(_currentMissionModel.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }


    }
}