using City.Buildings.Abstractions;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Fight.Common;
using Models.Fights.Campaign;
using Models.TravelRaceDatas;
using Network.DataServer;
using Network.DataServer.Messages.TravelCircles;
using System.Collections.Generic;
using Common.Db.CommonDictionaries;
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
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly IObjectResolver m_objectResolver;

        private const int SHOW_MISSION_COUNT = 10;

        private readonly List<TravelCircleMissionController> m_missionsUI = new();
        private TravelRaceCampaignButton m_currentCampaignSelector;
        private bool m_loadCurrentTravel;
        private TravelBuildingData m_travelCircleSave;
        private TravelRaceData m_currentTravelData;
        private TravelCircleMissionController m_selectedMissionUi;

        public void Initialize()
        {
            for (int i = 0; i < SHOW_MISSION_COUNT; i++)
            {
                TravelCircleMissionController missionUi = Object.Instantiate(View.MissionPrefab, View.Content);
                m_objectResolver.Inject(missionUi);

                missionUi.OnSelect.Subscribe(OnMissionSelect).AddTo(Disposables);
                m_missionsUI.Add(missionUi);
                missionUi.SetData(null, View.ScrollRect, m_commonDictionaries);
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
            m_travelCircleSave = CommonGameData.City.TravelCircleSave;

            foreach (var travelData in m_travelCircleSave.TravelRaceDatas)
            {
                var campaignSelector = View.TravelRaceCampaignButtons
                    .Find(selector => selector.RaceId.Equals(travelData.RaceId));

                if (campaignSelector == null)
                {
                    Debug.LogError($"Unknow race: {travelData.RaceId}");
                    continue;
                }

                var travelModel = m_commonDictionaries.TravelRaceCampaigns[$"{travelData.RaceId}Travel"];
                var raceContainer = m_commonDictionaries.Races[travelData.RaceId];


                var icon = SpriteUtils.LoadSprite(raceContainer.SpritePath);
                campaignSelector.SetData(travelModel, travelData.MissionIndexCompleted);
                campaignSelector.SetRaceIcon(icon);
            }

            var randomIndex = Random.Range(0, View.TravelRaceCampaignButtons.Count);
            OnSelectCampaign(View.TravelRaceCampaignButtons[randomIndex]);
        }

        private void LoadMissions()
        {
            if (m_loadCurrentTravel)
                return;

            var currentMission = 0;

            var currentTravelData = m_travelCircleSave.TravelRaceDatas
                .Find(data => data.RaceId.Equals(m_currentCampaignSelector.RaceId));

            if (currentTravelData != null)
            {
                m_currentTravelData = currentTravelData;
                currentMission = currentTravelData.MissionIndexCompleted + 1;
            }

            var currentTravelModel = m_commonDictionaries.TravelRaceCampaigns[$"{m_currentCampaignSelector.RaceId}Travel"];
            for (int i = 0; i < currentMission; i++)
            {
                m_missionsUI[i].SetData(currentTravelModel.Missions[i], View.ScrollRect, i + 1);
                //_missionsUI[i].SetCanSmash();
                m_missionsUI[i].Hide();
            }

            for (int i = currentMission; i < currentTravelModel.Missions.Count && i < m_missionsUI.Count; i++)
            {
                m_missionsUI[i].SetData(currentTravelModel.Missions[i], View.ScrollRect, i + 1);
            }
            for (int i = currentTravelModel.Missions.Count; i < m_missionsUI.Count; i++)
            {
                m_missionsUI[i].Hide();
            }

            m_missionsUI[currentMission].OpenForFight();
            m_loadCurrentTravel = true;
        }

        private void OnSelectCampaign(TravelRaceCampaignButton newSelector)
        {
            if (m_currentCampaignSelector == newSelector)
                return;

            m_currentCampaignSelector?.Diselect();
            m_currentCampaignSelector = newSelector;
            m_currentCampaignSelector.Select();
            m_loadCurrentTravel = false;
        }

        private void OnMissionSelect(TravelCircleMissionController missionUi)
        {
            m_selectedMissionUi = missionUi;

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
                TravelId = m_currentTravelData.Id,
                MissionIndex = mission.Index - 1,
                Count = 1
            };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                GameReward reward = new GameReward(mission.GetData.SmashReward, m_commonDictionaries);
                m_clientRewardService.ShowReward(reward, RewardType.Win);
            }
        }

        protected override void OnResultFight(FightResultType result)
        {
            if (result == FightResultType.Win)
            {
                SendData().Forget();

                m_selectedMissionUi.SetCanSmash();
                m_currentTravelData.MissionIndexCompleted += 1;
                m_missionsUI[m_currentTravelData.MissionIndexCompleted].OpenForFight();

                var travelModel = m_commonDictionaries.TravelRaceCampaigns[$"{m_currentTravelData.RaceId}Travel"];
                m_currentCampaignSelector.SetData(travelModel, m_currentTravelData.MissionIndexCompleted);
            }
            else
            {
                m_clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }

        private async UniTaskVoid SendData()
        {
            MissionRaceTravelCompleteMessage message = new MissionRaceTravelCompleteMessage
                {
                    PlayerId = CommonGameData.PlayerInfoData.Id,
                    TravelId = m_currentTravelData.Id
                };
            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                GameReward reward = new GameReward(m_selectedMissionUi.GetData.WinReward, m_commonDictionaries);
                m_clientRewardService.ShowReward(reward);
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