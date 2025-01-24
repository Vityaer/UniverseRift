using City.Buildings.Abstractions;
using City.Buildings.Voyage;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models.City.Mines;
using Models.City.Misc;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.Mines.Panels.Travels
{
    public class MineTravelPanelController : BuildingWithFight<MineTravelPanelView>
    {
        private const string MAIN_MINE_NAME = "MainMineBuilding";
        private const string MINE_SETTINGS_NAME = "MineMainBuildingName";

        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly VoyageMissionPanelController panelVoyageMission;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private int _travelLevel;
        private int _countTravelOpen;
        private StorageChallengeModel _containerTravel;
        private MissionModel _currentMissionModel;
        private MissionModel _bossMissionModel;
        private bool _flagBossMission;
        private int _currentMissionId;
        private MineMissionController _currentMissionController;
        private List<MineMissionData> _missionDatas = new();

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnLoadGame()
        {
            var mines = CommonGameData.City.IndustrySave.Mines;
            var settings = _commonDictionaries.Buildings[MINE_SETTINGS_NAME] as MineBuildingModel;
            var administrationMine = mines.Find(mine => mine.MineId.Equals(MAIN_MINE_NAME));

            var targetIndex = 0;
            for (var i = 0; i < settings.SettingsCampaigns.Count; i++)
            {
                if (administrationMine.Level >= settings.SettingsCampaigns[i].RequireLevel)
                {
                    targetIndex = i;
                }
                else
                {
                    break;
                }
            }
            var selectedSettings = settings.SettingsCampaigns[targetIndex];

            _travelLevel = selectedSettings.LevelHard;
            _countTravelOpen = Mathf.Clamp(selectedSettings.MissionsCount, 1, 15);
            _missionDatas = CommonGameData.City.IndustrySave.MissionDatas;
            LoadMissions();
        }

        private void LoadMissions()
        {
            _containerTravel = GetStorageChallenges(_travelLevel);
            if (_containerTravel == null)
            {
                Debug.LogError("Not found storage challenge");
                return;
            }

            StatusMission statusMission = StatusMission.NotOpen;
            for (int i = 0; i < _countTravelOpen; i++)
            {
                var missionData = _missionDatas[i];
                if (i <= _countTravelOpen)
                {
                    statusMission = missionData.IsComplete ? StatusMission.Complete : StatusMission.Open;
                }
                else
                {
                    statusMission = StatusMission.NotOpen;
                }
                var missionModel = _containerTravel.Missions.Find(data => data.Name.Equals(missionData.MissionId));
                if (missionModel == null)
                    Debug.LogError($"Mission with name: {missionData.MissionId} not found.");

                View.MissionViews[i].SetData(missionData, missionModel, statusMission);
                View.MissionViews[i].OnSelect.Subscribe(OpenVoyageMissionPanel).AddTo(Disposables);
            }

            for (var i = _countTravelOpen; i < View.MissionViews.Count; i++)
            {
                View.MissionViews[i].SetStatus(StatusMission.NotOpen);
            }

            //View.BossMissionView.SetData(1, StatusMission.Open);
            //View.BossMissionView.OnSelect.Subscribe(_ => OpenBossVoyageMissionPanel()).AddTo(Disposables);

        }

        private StorageChallengeModel GetStorageChallenges(int travelLevel)
        {
            StorageChallengeModel result = null;
            for (var i = travelLevel; i >= 0; i--)
            {
                if (_commonDictionaries.StorageChallenges.TryGetValue($"MineTravelLevel_{i}", out result))
                {
                    break;
                }
                else
                {
                    Debug.LogError($"Not found storage Challenge by name: MineTravelLevel_{i}");
                }
            }
            return result;
        }

        private void OpenBossVoyageMissionPanel()
        {
            _flagBossMission = true;
            _currentMissionModel = _bossMissionModel;
            panelVoyageMission.ShowInfo(_currentMissionModel, View.BossMissionView.Status, 1, StartOpenMission);
        }

        private void OpenVoyageMissionPanel(MineMissionController missionController)
        {
            _flagBossMission = false;
            _currentMissionId = missionController.MissionData.Id;
            _currentMissionModel = missionController.MissionModel;
            _currentMissionController = missionController;
            panelVoyageMission.ShowInfo(_currentMissionModel, missionController.Status, _travelLevel, StartOpenMission);
        }

        private void StartOpenMission()
        {
            OpenMission(_currentMissionModel);
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _onWinFight.Execute(1);
                _currentMissionController.SetStatus(StatusMission.Complete);
                SendData().Forget();
            }
            else
            {
                _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }

        private async UniTaskVoid SendData()
        {
            var message = new MineFinishMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id, MissionId = _currentMissionId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(_currentMissionModel.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }
    }
}
