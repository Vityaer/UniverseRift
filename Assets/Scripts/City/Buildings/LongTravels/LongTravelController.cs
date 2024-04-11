using City.Buildings.Abstractions;
using City.Buildings.Voyage;
using ClientServices;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Fight;
using Models.City.Misc;
using Models.Common;
using Models.Fights.Campaign;
using Network.DataServer;
using Network.DataServer.Messages.City.LongTravels;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.LongTravels
{
    public class LongTravelController : BuildingWithFight<LongTravelView>, IInitializable, IDisposable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly VoyageMissionPanelController _panelVoyageMission;
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private int _currentMissionIndex;
        private LongTravelType _currentTravelType;
        private LongTravelContainerView _currentTravelView;
        private StorageChallengeModel _currentMissionContainer;
        private MissionModel _currentMissionModel;

        public void Initialize()
        {
            View.MainTravelButton.OnClickAsObservable().Subscribe(_ => OpenTravel(LongTravelType.Main)).AddTo(Disposables);
            View.TrainTravelButton.OnClickAsObservable().Subscribe(_ => OpenTravel(LongTravelType.Train)).AddTo(Disposables);
            View.HeroTravelButton.OnClickAsObservable().Subscribe(_ => OpenTravel(LongTravelType.Hero)).AddTo(Disposables);
            View.MissionPanelCloseButton.OnClickAsObservable().Subscribe(_ => CloseMissionPanel()).AddTo(Disposables);
            View.TravelPanel.SetActive(true);
            LoadMissions();
        }

        protected override void OnLoadGame()
        {
            foreach (var travelView in View.TravelViews)
            {
                var amount = GetLongTravelCurrentAmount(travelView.Key);
                travelView.Value.Amount.text = $"{amount} / 2";
            }
        }

        private int GetLongTravelCurrentAmount(LongTravelType travelType)
        {
            var amount = 0;
            if (_сommonGameData.City.LongTravelData.TravelProgress.ContainsKey(travelType))
            {
                amount = _сommonGameData.City.LongTravelData.TravelProgress[travelType];
            }

            return amount;
        }

        private void LoadMissions()
        {
            StatusMission statusMission = StatusMission.NotOpen;
            var level = 1;
            for (int i = 0; i < View.MissionViews.Count; i++)
            {
                View.MissionViews[i].SetData(i + 1, statusMission);
                View.MissionViews[i].SetLabel($"Уровень {level}");
                level = GetNextLevel(level);
                View.MissionViews[i].OnSelect.Subscribe(OpenMissionPanel).AddTo(Disposables);
            }
        }

        private void OpenMissionPanel(int index)
        {
            var amount = GetLongTravelCurrentAmount(_currentTravelType);
            if (amount >= 2)
                return;

            _currentMissionIndex = index;
            var mission = View.MissionViews[index];
            _currentMissionModel = _currentMissionContainer.Missions[index];
            _panelVoyageMission.ShowInfo(_currentMissionModel, mission.Status, index, StartOpenMission);
        }

        private void StartOpenMission()
        {
            OpenMission(_currentMissionModel);
        }

        private int GetNextLevel(int level)
        {
            if (level == 1)
                return 10;

            level += 10;
            return level;
        }

        private void OpenTravel(LongTravelType type)
        {
            _currentTravelType = type;
            _currentTravelView = View.TravelViews[type];
            _currentTravelView.MainPanel.SetActive(true);
            View.MissionsPanel.SetActive(true);
            View.TravelPanel.SetActive(false);

            _currentMissionContainer = _commonDictionaries.StorageChallenges[$"LongTravel_{type}"];
            var openIndex = GetOpenIndex();

            StatusMission status;
            for (var i = 0; i < View.MissionViews.Count; i++)
            {
                if (i <= openIndex)
                {
                    status = StatusMission.Open;
                }
                else
                {
                    status = StatusMission.NotOpen;
                }

                View.MissionViews[i].SetStatus(status);
            }
        }

        private int GetOpenIndex()
        {
            var playerLevel = CommonGameData.PlayerInfoData.Level;
            var result = playerLevel / 10;
            return result;
        }

        private void CloseMissionPanel()
        {
            _currentTravelView.MainPanel.SetActive(false);
            View.MissionsPanel.SetActive(false);
            View.TravelPanel.SetActive(true);
        }

        protected override void OnResultFight(FightResultType result)
        {
            _onTryFight.Execute(1);

            if (!_сommonGameData.City.LongTravelData.TravelProgress.ContainsKey(_currentTravelType))
                _сommonGameData.City.LongTravelData.TravelProgress.Add(_currentTravelType, 0);

            _сommonGameData.City.LongTravelData.TravelProgress[_currentTravelType] += 1;

            var amount = _сommonGameData.City.LongTravelData.TravelProgress[_currentTravelType];
            _currentTravelView.Amount.text = $"{amount} / 2";

            base.OnResultFight(result);
            SendFinishMissionMessage(result).Forget();
        }

        private async UniTaskVoid SendFinishMissionMessage(FightResultType resultType)
        {
            var message = new WinMissionLongTravelMessage
            {
                PlayerId = _сommonGameData.PlayerInfoData.Id,
                TravelType = (int)_currentTravelType,
                MissionIndex = _currentMissionIndex,
                Result = (int)resultType
            };

            var result = await DataServer.PostData(message);

            if (resultType == FightResultType.Win)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    var reward = new GameReward(_currentMissionModel.WinReward, _commonDictionaries);
                    _clientRewardService.ShowReward(reward, RewardType.Win);
                }
            }
            else
            {
                _clientRewardService.ShowReward(new GameReward(), RewardType.Defeat);
            }
        }
    }
}
