using City.Buildings.Abstractions;
using City.Buildings.WorldMaps;
using City.Panels.Arenas.Teams;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models;
using Models.Arenas;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace Campaign
{
    public class CampaignController : BuildingWithFight<CampaignView>, IInitializable
    {
        private const string NAME_RECORD_NUM_CURRENT_MISSION = "CurrentMission";
        private const string NAME_RECORD_NUM_MAX_MISSION = "MaxMission";
        private const string NAME_RECORD_AUTOFIGHT_PREVIOUS_DATETIME = "AutoFight";
        private const int CHAPTER_MISSION_COUNT = 20;

        [Inject] private readonly GoldHeapController _goldHeapController;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly IObjectResolver _resolver;

        private CampaignChapterModel _chapter;
        private CampaignMissionModel _mission;
        private int _currentMissionIndex;
        private int _maxMission;
        private MissionController _infoMission;
        private BuildingWithFightTeamsData campaingSaveObject;
        private List<MissionController> missionControllers = new List<MissionController>();

        public void Initialize()
        {
            View.WorldMapButton.OnClickAsObservable().Subscribe(_ => OpenWorldMap()).AddTo(Disposables);

            for (var i = 0; i < CHAPTER_MISSION_COUNT; i++)
            {
                var missionController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                _resolver.Inject(missionController);
                missionControllers.Add(missionController);
                missionController.OnClickMission.Subscribe(SelectMission).AddTo(Disposables);
                missionController.rewardController.SetDetailsController(SubjectDetailController);
            }
        }

        protected override void OnLoadGame()
        {
            campaingSaveObject = _commonGameData.City.MainCampaignSave;

            if (PlayerPrefs.HasKey(NAME_RECORD_NUM_CURRENT_MISSION))
            {
                _currentMissionIndex = PlayerPrefs.GetInt(NAME_RECORD_NUM_CURRENT_MISSION);
            }


            _maxMission = campaingSaveObject.IntRecords.GetRecord(NAME_RECORD_NUM_MAX_MISSION, 0);
            if (_currentMissionIndex >= _maxMission) _currentMissionIndex = _maxMission - 1;

            var chapter = GetCampaignChapter(_currentMissionIndex);
            OpenChapter(chapter);
        }

        private CampaignChapterModel GetCampaignChapter(int numMission)
        {
            var result = _commonDictionaries.CampaignChapters.ElementAt(numMission / CHAPTER_MISSION_COUNT).Value;
            return result;
        }

        //API
        private void LoadMissions(CampaignChapterModel chapter)
        {
            for (int i = 0; i < missionControllers.Count; i++)
            {
                missionControllers[i].SetMission(chapter.Missions[i], chapter.numChapter * CHAPTER_MISSION_COUNT + i + 1, _commonDictionaries);
            }

            if (chapter.numChapter * CHAPTER_MISSION_COUNT <= _maxMission)
            {
                for (int i = 0; chapter.numChapter * CHAPTER_MISSION_COUNT + i <= _maxMission && i < missionControllers.Count; i++)
                {
                    missionControllers[i].CompletedMission();
                }
                if (chapter.numChapter == _maxMission / CHAPTER_MISSION_COUNT)
                {
                    missionControllers[_maxMission % CHAPTER_MISSION_COUNT].OpenMission();
                }
            }

            if (_currentMissionIndex >= 0)
            {
                if (chapter.numChapter == _currentMissionIndex / CHAPTER_MISSION_COUNT)
                {
                    SelectMission(missionControllers[_currentMissionIndex]);
                }
            }
        }

        public void OpenChapter(CampaignChapterModel chapter)
        {
            this._chapter = chapter;
            LoadMissions(chapter);
        }

        public void OpenNextMission()
        {
            SendData().Forget();
            OpenMission(_currentMissionIndex + 1);
            PlayerPrefs.SetInt(NAME_RECORD_NUM_CURRENT_MISSION, _currentMissionIndex);
            campaingSaveObject.IntRecords.SetRecord(NAME_RECORD_NUM_MAX_MISSION, _maxMission);
        }

        private async UniTaskVoid SendData()
        {
            var message = new CompleteNextMissionMessage { PlayerId = _commonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
        }

        private void OpenMission(int num)
        {
            _currentMissionIndex = num;
            _maxMission = num + 1;
            if (_chapter.numChapter == _maxMission / CHAPTER_MISSION_COUNT)
            {
                var missionController = missionControllers[_maxMission % CHAPTER_MISSION_COUNT];
                missionController.OpenMission();
            }
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                _infoMission.MissionWin();
                var reward = new GameReward(_infoMission.mission.WinReward, _commonDictionaries);
                _clientRewardService.ShowReward(reward, RewardType.Win);
                SetAutoFightMission(_infoMission);
                OpenNextMission();
            }
            else if (result == FightResultType.Defeat)
            {
                _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }

            base.OnResultFight(result);
        }

        public void SelectMission(MissionController infoMission)
        {
            if (this._infoMission != null)
            {
                if (_infoMission.Status == StatusMission.InAutoFight)
                    _infoMission.StopAutoFight();
            }

            this._infoMission = infoMission;
            _mission = infoMission.mission;

            switch (infoMission.Status)
            {
                case StatusMission.Complete:
                    SetAutoFightMission(infoMission);
                    break;
                case StatusMission.Open:
                    OpenMission(_mission);
                    break;
            }
        }

        private void SetAutoFightMission(MissionController infoMission)
        {
            var index = missionControllers.IndexOf(infoMission);
            infoMission.StartAutoFight();
            _goldHeapController.SetNewReward(infoMission.mission.AutoFightReward, index);
            SaveSelectAutoFight(infoMission);
        }

        private void SaveSelectAutoFight(MissionController infoMission)
        {
            var index = missionControllers.IndexOf(infoMission);
            PlayerPrefs.SetInt(NAME_RECORD_NUM_CURRENT_MISSION, index);
        }


        //Auto fight
        public void SaveAutoFight(DateTime newDateTime)
        {
            campaingSaveObject.DateRecords.SetRecord(
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