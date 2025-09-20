using City.Buildings.Abstractions;
using City.Buildings.Voyage;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Models.City.Mines;
using Models.City.Misc;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.City.Mines;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
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
        [Inject] private readonly VoyageMissionPanelController _panelVoyageMission;
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

        protected override void OnLoadGame()
        {
            var mines = CommonGameData.City.IndustrySave.Mines;
            var settings = _commonDictionaries.Buildings[MAIN_MINE_NAME] as MineBuildingModel;
            var administrationMine = mines.Find(mine => mine.MineId.Equals(MAIN_MINE_NAME));

            var targetIndex = 0;
            for (var i = 0; i < settings.ConfigureContainers.Count; i++)
            {
                if (administrationMine.Level >= settings.ConfigureContainers[i].RequireLevel)
                {
                    targetIndex = i;
                }
                else
                {
                    break;
                }
            }
            var selectedSettings = settings.ConfigureContainers[targetIndex];

            _travelLevel = selectedSettings.MissionsLevelHard;
            _countTravelOpen = selectedSettings.MissionsCount;
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
                if (_missionDatas.Count <= i)
                {
                    View.MissionViews[i].SetStatus(StatusMission.NotOpen);
                    continue;
                }

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

                View.MissionViews[i].SetData(missionData, missionModel, statusMission, CommonGameData.City.IndustrySave.DateTimeCreate);
                View.MissionViews[i].OnSelect.Subscribe(OpenVoyageMissionPanel).AddTo(Disposables);
            }

            for (var i = _countTravelOpen; i < View.MissionViews.Count; i++)
            {
                View.MissionViews[i].SetStatus(StatusMission.NotOpen);
            }
            
            if (!_commonDictionaries.StorageChallenges.TryGetValue($"MineTravelLevel_{_travelLevel}",
                    out var bossContainerTravel))
            {
                return;
            }

            var bossMissionData = CommonGameData.City.IndustrySave.BossMissionData;

            if (bossMissionData == null)
            {
                Debug.LogError("bossMissionData is null!");
                return;
            }

            _bossMissionModel = bossContainerTravel.Missions.
                Find(data => data.Name.Equals(bossMissionData.MissionId));

            View.BossMissionView.SetData(bossMissionData,
                _bossMissionModel,
                bossMissionData.IsComplete ? StatusMission.Complete : StatusMission.Open,
                CommonGameData.City.IndustrySave.DateTimeCreate);
            
            View.BossMissionView.OnSelect.Subscribe(_ => OpenBossVoyageMissionPanel()).AddTo(Disposables);

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
            _currentMissionId = View.BossMissionView.MissionData.Id;
            _currentMissionModel = _bossMissionModel;
            _currentMissionController = View.BossMissionView;
            _panelVoyageMission.ShowInfo(_currentMissionModel, View.BossMissionView.Status, _travelLevel, StartOpenMission);
        }

        private void OpenVoyageMissionPanel(MineMissionController missionController)
        {
            _flagBossMission = false;
            _currentMissionId = missionController.MissionData.Id;
            _currentMissionModel = missionController.MissionModel;
            _currentMissionController = missionController;
            _panelVoyageMission.ShowInfo(_currentMissionModel, missionController.Status, _travelLevel, StartOpenMission);
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
