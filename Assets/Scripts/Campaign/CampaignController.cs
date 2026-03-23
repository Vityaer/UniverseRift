using City.Buildings.Abstractions;
using City.Buildings.WorldMaps;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Models;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using Campaign.GoldHeaps;
using Common.Db.CommonDictionaries;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace Campaign
{
    public class CampaignController : BuildingWithFight<CampaignView>, IInitializable
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";
        private const string NAME_RECORD_NUM_MAX_MISSION = "MaxMission";
        private const string NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME = "AutoFight";
        private const int CHAPTER_MISSION_COUNT = 20;

        [Inject] private readonly GoldHeapController m_goldHeapController;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly IObjectResolver m_resolver;
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private CampaignChapterModel m_chapter;
        private CampaignMissionModel m_mission;
        private int m_currentMissionIndex = 0;
        private int m_maxMission;
        private MissionController m_infoMission;
        private BuildingWithFightTeamsData m_campaingSaveObject;
        private readonly List<MissionController> m_missionControllers = new List<MissionController>();

        public List<MissionController> MissionControllers => m_missionControllers;
        
        public void Initialize()
        {
            View.WorldMapButton.OnClickAsObservable().Subscribe(_ => OpenWorldMap()).AddTo(Disposables);

            for (var i = 0; i < CHAPTER_MISSION_COUNT; i++)
            {
                var missionController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                m_resolver.Inject(missionController);
                m_missionControllers.Add(missionController);
                missionController.OnClickMission.Subscribe(SelectMission).AddTo(Disposables);
                missionController.rewardController.SetDetailsController(SubjectDetailController);
            }
        }

        protected override void OnLoadGame()
        {
            m_campaingSaveObject = m_commonGameData.City.MainCampaignSave;

            bool setAutoFight = false;
            if (PlayerPrefs.HasKey(NAME_RECORD_NUM_CURRENT_MISSION))
            {
                m_currentMissionIndex = PlayerPrefs.GetInt(NAME_RECORD_NUM_CURRENT_MISSION);
                setAutoFight = true;
            }


            m_maxMission = m_campaingSaveObject.IntRecords.GetRecord(NAME_RECORD_NUM_MAX_MISSION, 0);
            if (m_currentMissionIndex >= m_maxMission) m_currentMissionIndex = m_maxMission - 1;

            if (m_currentMissionIndex < 0)
            {
                m_currentMissionIndex = 0;
            }

            var chapter = GetCampaignChapter(m_currentMissionIndex);
            OpenChapter(chapter);

            if (!setAutoFight)
                return;
            
            SetAutoFightMission(m_missionControllers[m_currentMissionIndex % CHAPTER_MISSION_COUNT]);
        }

        private CampaignChapterModel GetCampaignChapter(int numMission)
        {
            var result = m_commonDictionaries.CampaignChapters.ElementAt(numMission / CHAPTER_MISSION_COUNT).Value;
            return result;
        }

        //API
        private void LoadMissions(CampaignChapterModel chapter)
        {
            for (int i = 0; i < m_missionControllers.Count; i++)
            {
                m_missionControllers[i].SetMission(chapter.Missions[i], chapter.numChapter * CHAPTER_MISSION_COUNT + i + 1, m_commonDictionaries);
            }

            if (chapter.numChapter * CHAPTER_MISSION_COUNT <= m_maxMission)
            {
                for (int i = 0; chapter.numChapter * CHAPTER_MISSION_COUNT + i <= m_maxMission && i < m_missionControllers.Count; i++)
                {
                    m_missionControllers[i].CompletedMission();
                }
                if (chapter.numChapter == m_maxMission / CHAPTER_MISSION_COUNT)
                {
                    m_missionControllers[m_maxMission % CHAPTER_MISSION_COUNT].OpenMission();
                }
            }

            if (m_currentMissionIndex < 0) return;
            if (chapter.numChapter != m_currentMissionIndex / CHAPTER_MISSION_COUNT) return;
            if (m_missionControllers[m_currentMissionIndex] == null) return;
            
            this.m_infoMission = m_missionControllers[m_currentMissionIndex];
            if (m_infoMission.Status is StatusMission.InAutoFight or StatusMission.Complete)
                m_infoMission.StopAutoFight();
        }

        public void OpenChapter(CampaignChapterModel chapter)
        {
            this.m_chapter = chapter;
            LoadMissions(chapter);
        }

        private void OpenNextMission()
        {
            SendData().Forget();
            int lastMissionIndex = m_missionControllers.FindIndex(m => m.mission == m_mission);
            int targetMissionIndex = lastMissionIndex + 1;
            OpenMission(targetMissionIndex);
            PlayerPrefs.SetInt(NAME_RECORD_NUM_CURRENT_MISSION, targetMissionIndex);
            m_campaingSaveObject.IntRecords.SetRecord(NAME_RECORD_NUM_MAX_MISSION, m_maxMission);
        }

        private async UniTaskVoid SendData()
        {
            var message = new CompleteNextMissionMessage { PlayerId = m_commonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
        }

        private void OpenMission(int num)
        {
            m_currentMissionIndex = num;
            m_maxMission = m_currentMissionIndex;
            if (m_chapter.numChapter != m_maxMission / CHAPTER_MISSION_COUNT) return;

            var missionController = m_missionControllers[m_maxMission % CHAPTER_MISSION_COUNT];
            Debug.Log($"OpenMission {m_maxMission}", missionController.gameObject);
            missionController.OpenMission();
        }

        protected override void OnResultFight(FightResultType result)
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<CampaignController>(openType: OpenType.Exclusive);

            base.OnResultFight(result);
            switch (result)
            {
                case FightResultType.Win:
                {
                    m_infoMission.MissionWin();
                    GameReward reward = new GameReward(m_infoMission.mission.WinReward, m_commonDictionaries);
                    m_clientRewardService.ShowReward(reward, RewardType.Win);
                    SetAutoFightMission(m_infoMission);
                    OpenNextMission();
                    break;
                }
                case FightResultType.Defeat:
                    TrySelectMission(m_maxMission - 1);
                    m_clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
                    break;
            }
        }

        private void TrySelectMission(int targetMissionIndex)
        {
            if (targetMissionIndex < 0 || targetMissionIndex >= m_missionControllers.Count)
            {
                return;
            }
            
            SelectMission(m_missionControllers[targetMissionIndex]);
        }

        private void SelectMission(MissionController infoMission)
        {
            if (this.m_infoMission != null)
            {
                if (m_infoMission.Status == StatusMission.InAutoFight)
                    m_infoMission.StopAutoFight();
            }

            this.m_infoMission = infoMission;
            m_mission = infoMission.mission;

            switch (infoMission.Status)
            {
                case StatusMission.Complete:
                    SetAutoFightMission(infoMission);
                    break;
                case StatusMission.Open:
                    OpenMission(m_mission);
                    break;
            }
        }

        private void SetAutoFightMission(MissionController infoMission)
        {
            var index = m_missionControllers.IndexOf(infoMission);
            infoMission.StartAutoFight();
            m_goldHeapController.SetNewReward(infoMission.mission.AutoFightReward, index);
            SaveSelectAutoFight(infoMission);
        }

        private void SaveSelectAutoFight(MissionController infoMission)
        {
            var index = m_missionControllers.IndexOf(infoMission);
            PlayerPrefs.SetInt(NAME_RECORD_NUM_CURRENT_MISSION, index);
        }


        //Auto fight
        public void SaveAutoFight(DateTime newDateTime)
        {
            m_campaingSaveObject.DateRecords.SetRecord(
                NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME,
                newDateTime.ToString(Constants.Common.DateTimeFormat)
                );
        }

        private void OpenWorldMap()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<WorldMapController>(openType: OpenType.Exclusive);
        }

    }
}