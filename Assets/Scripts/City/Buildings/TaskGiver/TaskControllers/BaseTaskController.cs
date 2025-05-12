using System;
using System.Globalization;
using City.Panels.SubjectPanels.Common;
using City.TaskBoard;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using LocalizationSystems;
using Models.Common;
using Models.Data;
using Models.Tasks;
using Network.DataServer;
using Network.DataServer.Messages.City.Taskboards;
using Services.TimeLocalizeServices;
using Sirenix.Utilities;
using UIController;
using UIController.Rewards;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.TaskGiver
{
    public abstract class BaseTaskController  : ScrollableUiView<TaskData>
    {
        [Inject] protected CommonGameData _commonGameData;
        [Inject] protected ResourceStorageController _resourceStorageController;
        [Inject] protected ClientRewardService _clientRewardService;
        [Inject] protected CommonDictionaries _commonDictionaries;
        [Inject] protected ILocalizationSystem _localizationSystem;
        protected TimeLocalizeService _timeLocalizeService;

        [Header("UI")]
        [SerializeField] protected LocalizeStringEvent _name;
        [SerializeField] protected GameObject objectCurrentTime;
        [SerializeField] protected SliderTime sliderTime;
        public ButtonCostController TaskControllerButton;
        [SerializeField] protected RatingHero ratingController;
        [SerializeField] protected RewardUIController RewardUIController;

        private GameReward _gameReward;
        
        protected IDisposable _disposable;
        protected IDisposable _timeSliderDisposable;
        protected GameTaskModel _model;
        protected ReactiveCommand<BaseTaskController> _onGetReward = new(); 
        protected ReactiveCommand<BaseTaskController> m_onStartTask = new();
        
        public IObservable<BaseTaskController> OnStartTask => m_onStartTask;
        public TaskData GetTask => Data;
        public IObservable<BaseTaskController> OnGetReward => _onGetReward;
        public GameTaskModel Model => _model;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController, TimeLocalizeService timeLocalizeService)
        {
            RewardUIController.SetDetailsController(subjectDetailController);
            _timeLocalizeService = timeLocalizeService;
        }

        public void SetData(TaskData data, ScrollRect scrollRect, GameTaskModel model)
        {
            sliderTime.Init(_localizationSystem, _timeLocalizeService);
            SetData(data, scrollRect);
            _model = model;

            _name.StringReference = _localizationSystem.GetLocalizedContainer($"Task{_model.Id}Name");
            ratingController.ShowRating(_model.Rating);

            RewardModel rewardWithFactor = _model.Reward * data.Factor;
            _gameReward = new GameReward(rewardWithFactor, _commonDictionaries);
            
            RewardUIController.ShowReward(rewardWithFactor, _commonDictionaries);
            UpdateStatus();

        }

        public override void SetData(TaskData data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
        }

        private void UpdateStatus()
        {
            TaskControllerButton.Clear();

            var requireTime = new TimeSpan(_model.RequireHour, 0, 0);

            _disposable?.Dispose();
            DateTime startDateTime = new();

            if (!Data.DateTimeStart.IsNullOrWhitespace())
            {
                try
                {
                    startDateTime = DateTime.ParseExact(
                    Data.DateTimeStart,
                    Constants.Common.DateTimeFormat,
                    CultureInfo.InvariantCulture
                    );
                }
                catch
                {
                    startDateTime = DateTime.Parse( Data.DateTimeStart );
                }

                if (Data.Status == TaskStatusType.InWork)
                {
                    var leftTime = DateTime.UtcNow - startDateTime;
                    if (leftTime > requireTime)
                    {
                        Data.Status = TaskStatusType.Done;
                    }
                }
            }

            UpdateUi(startDateTime, requireTime);
        }

        protected abstract void UpdateUi(DateTime startDateTime, TimeSpan requireTime);

        public void StopTimer()
        {
            sliderTime.StopTimer();
        }
        
        protected void FinishFromSlider()
        {
            _timeSliderDisposable?.Dispose();
            _timeSliderDisposable = null;
            UpdateStatus();
        }

        protected async UniTask StartTask()
        {
            var message = new StartTaskMessage { PlayerId = _commonGameData.PlayerInfoData.Id, TaskId = Data.TaskId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                objectCurrentTime.SetActive(true);
                var now = DateTime.UtcNow;
                var requireTime = new TimeSpan(_model.RequireHour, 0, 0);
                sliderTime?.SetData(now, requireTime);
                Data.Status = TaskStatusType.InWork;
                Data.DateTimeStart = DateTime.UtcNow.ToString();
                UpdateStatus();
                m_onStartTask.Execute(this);
            }
        }

        protected async UniTask BuyProgress()
        {
            var message = new BuyFastCompleteTaskMessage { PlayerId = _commonGameData.PlayerInfoData.Id, TaskId = Data.TaskId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var cost = new GameResource(ResourceType.Diamond, 10 * _model.Rating, 0);
                _resourceStorageController.SubtractResource(cost);
                TaskControllerButton.Clear();
                sliderTime.SetFinish();
                CreateReward();
            }
        }

        protected async UniTask GetReward()
        {
            var message = new CompleteTaskMessage { PlayerId = _commonGameData.PlayerInfoData.Id, TaskId = Data.TaskId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                CreateReward();
            }
        }
        
        private void CreateReward()
        {
            Data.Status = TaskStatusType.Done;
            _clientRewardService.ShowReward(_gameReward);
            _onGetReward.Execute(this);
        }
    }
}