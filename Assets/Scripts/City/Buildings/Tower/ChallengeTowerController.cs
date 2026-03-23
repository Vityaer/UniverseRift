using City.Buildings.Abstractions;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Models;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.ChallengeTowers;
using System;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using UniRx;
using Utils.DataCheckUtils;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Tower
{
    public class ChallengeTowerController : BuildingWithFight<ChallengeTowerView>, IInitializable
    {
        private readonly CommonDictionaries m_commonDictionaries;
        private readonly ClientRewardService m_clientRewardService;
        private readonly IObjectResolver m_objectResolver;

        private const int SHOW_MISSION_COUNT = 21;
        private const int SHOW_COMPLETED_MISSION_COUNT = 2;
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";

        private readonly List<TowerMissionController> m_missionsUI = new();
        private int m_currentMissionIndex;

        private List<MissionModel> m_workMission = new(SHOW_MISSION_COUNT);
        private BuildingWithFightTeamsData m_challengeTower;
        private readonly ReactiveCommand<int> m_onCompleteMission = new();
        private List<MissionModel> m_listMissions;


        public readonly ReactiveCommand OnTryMission = new();

        public ChallengeTowerController(CommonDictionaries commonDictionaries,
            ClientRewardService clientRewardService,
            IObjectResolver objectResolver)
        {
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_clientRewardService = clientRewardService.ThrowIfArgumentNull();
            m_objectResolver = objectResolver.ThrowIfArgumentNull();
        }

        public IObservable<int> OnCompleteMission => m_onCompleteMission;

        public void Initialize()
        {
            for (var i = 0; i < SHOW_MISSION_COUNT + SHOW_COMPLETED_MISSION_COUNT; i++)
            {
                var missionUi = UnityEngine.Object.Instantiate(View.MissionPrefab, View.Content);
                m_objectResolver.Inject(missionUi);
                m_missionsUI.Add(missionUi);
                missionUi.SetData(null, View.ScrollRect, m_commonDictionaries);
                missionUi.OnClick.Subscribe(SelectMission).AddTo(Disposables);
            }
        }

        private void SelectMission(TowerMissionController mission)
        {
            OpenMission(mission.GetData);
        }

        protected override void OnLoadGame()
        {
            m_challengeTower = CommonGameData.City.ChallengeTowerSave;
            m_currentMissionIndex = m_challengeTower.IntRecords.GetRecord(NAME_RECORD_NUM_CURRENT_MISSION);

            m_listMissions = m_commonDictionaries.StorageChallenges["ChallengeTower"].Missions;
            LoadMissions();
        }

        private void LoadMissions()
        {
            m_workMission = new List<MissionModel>(SHOW_MISSION_COUNT);
            for (int i = m_currentMissionIndex; i < m_currentMissionIndex + SHOW_MISSION_COUNT && i < m_listMissions.Count; i++)
                m_workMission.Add(m_listMissions[i]);
            FillData();
        }

        private void FillData()
        {
            int index = 0;
            for (int i = 0; i < m_missionsUI.Count && i < m_workMission.Count; i++)
            {
                m_missionsUI[i].gameObject.SetActive(true);
                m_missionsUI[i].SetData(m_workMission[i], View.ScrollRect, m_currentMissionIndex + i + 1,
                    i == 0);
                index = i;
            }

            for (int i = index; i < m_missionsUI.Count; i++)
            {
                m_missionsUI[i].gameObject.SetActive(false);
            }
        }

        //After fight

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                SendData(m_workMission[m_currentMissionIndex]).Forget();
                m_currentMissionIndex += 1;
                LoadMissions();
                m_onCompleteMission.Execute(m_currentMissionIndex);
                SendTryMissionData(result).Forget();
            }
            else
            {
                SendTryMissionData(result).Forget();
            }

            OnTryMission.Execute();
        }

        private async UniTaskVoid SendData(MissionModel missionModel)
        {
            ChallengeTowerFinishMissionMessage message = new ChallengeTowerFinishMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var reward = new GameReward(missionModel.WinReward, m_commonDictionaries);
                m_clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }

        private async UniTaskVoid SendTryMissionData(FightResultType fightResultType)
        {
            ChallengeTowerTryMissionMessage message = new ChallengeTowerTryMissionMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
            }

            if (fightResultType == FightResultType.Defeat)
            {
                m_clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }
    }
}