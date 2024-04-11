using City.Buildings.Abstractions;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.ChallengeTowers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Tower
{
    public class ChallengeTowerController : BuildingWithFight<ChallengeTowerView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly IObjectResolver _objectResolver;

        private const int SHOW_MISSION_COUNT = 8;
        private const int SHOW_COMPLETED_MISSION_COUNT = 2;
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";


        public List<TowerMissionCotroller> _missionsUI = new();

        private int _currentMissionIndex = 0;

        private List<MissionModel> workMission = new List<MissionModel>(SHOW_MISSION_COUNT);
        private BuildingWithFightTeamsData challengeTower = null;
        private ReactiveCommand<int> _onCompleteMission = new();
        private List<MissionModel> _listMissions;


        public ReactiveCommand OnTryMission = new();
        public IObservable<int> OnCompleteMission => _onCompleteMission;

        public void Initialize()
        {
            for (var i = 0; i < SHOW_MISSION_COUNT + SHOW_COMPLETED_MISSION_COUNT; i++)
            {
                var missionUi = UnityEngine.Object.Instantiate(View.MissionPrefab, View.Content);
                _objectResolver.Inject(missionUi);
                _missionsUI.Add(missionUi);
                missionUi.SetData(null, View.ScrollRect);
                missionUi.OnClick.Subscribe(SelectMission).AddTo(Disposables);
            }
        }

        private void SelectMission(TowerMissionCotroller mission)
        {
            OpenMission(mission.GetData);
        }

        protected override void OnLoadGame()
        {
            challengeTower = CommonGameData.City.ChallengeTowerSave;
            _currentMissionIndex = challengeTower.IntRecords.GetRecord(NAME_RECORD_NUM_CURRENT_MISSION);

            _listMissions = _commonDictionaries.StorageChallenges["ChallengeTower"].Missions;
            LoadMissions();
        }

        private void LoadMissions()
        {
            workMission = new List<MissionModel>(SHOW_MISSION_COUNT);
            for (int i = _currentMissionIndex; i < _currentMissionIndex + SHOW_MISSION_COUNT && i < _listMissions.Count; i++)
                workMission.Add(_listMissions[i]);
            FillData();
        }

        private void FillData()
        {
            for (int i = 0; i < _missionsUI.Count && i < workMission.Count; i++)
            {
                _missionsUI[i].SetData(workMission[i], View.ScrollRect, _currentMissionIndex + i + 1, i == 0);
            }
        }

        //After fight

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                SendData(workMission[_currentMissionIndex]).Forget();
                _currentMissionIndex += 1;
                LoadMissions();
                _onCompleteMission.Execute(_currentMissionIndex);
                SendTryMissionData().Forget();
            }
            else
            {
                SendTryMissionData().Forget();
            }

            OnTryMission.Execute();
        }

        private async UniTaskVoid SendData(MissionModel missionModel)
        {
            var message = new ChallengeTowerFinishMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(missionModel.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }

        private async UniTaskVoid SendTryMissionData()
        {
            UnityEngine.Debug.Log("SendTryMissionData");
            var message = new ChallengeTowerTryMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }
            _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
        }
    }
}