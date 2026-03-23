using System;
using System.Globalization;
using City.Panels.SubjectPanels.Common;
using City.TaskBoard;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Common.Rewards;
using Cysharp.Threading.Tasks;
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
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.TaskGiver.TaskControllers
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

        private GameReward m_gameReward;
        private CommonDictionaries m_gameDictionaries;
        
        protected IDisposable m_disposable;
        protected IDisposable TimeSliderDisposable;
        protected GameTaskModel _model;
        protected ReactiveCommand<BaseTaskController> _onGetReward = new(); 
        protected ReactiveCommand<BaseTaskController> m_onStartTask = new();
        
        public IObservable<BaseTaskController> OnStartTask => m_onStartTask;
        public TaskData GetTask => Data;
        public IObservable<BaseTaskController> OnGetReward => _onGetReward;
        public GameTaskModel Model => _model;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController, TimeLocalizeService timeLocalizeService,  CommonDictionaries gameDictionaries)
        {
            subjectDetailController.ThrowIfArgumentNull();
            RewardUIController.SetDetailsController(subjectDetailController);
            _timeLocalizeService = timeLocalizeService.ThrowIfArgumentNull();
            m_gameDictionaries = gameDictionaries.ThrowIfArgumentNull();
        }

        public void SetData(TaskData data, ScrollRect scrollRect, GameTaskModel model)
        {
            sliderTime.Init(_localizationSystem, _timeLocalizeService);
            SetData(data, scrollRect, m_gameDictionaries);
            _model = model;

            _name.StringReference = _localizationSystem.GetLocalizedContainer($"Task{_model.Id}Name");
            ratingController.ShowRating(_model.Rating);

            RewardModel rewardWithFactor = _model.Reward * data.Factor;
            m_gameReward = new GameReward(rewardWithFactor, _commonDictionaries);
            
            RewardUIController.ShowReward(rewardWithFactor, _commonDictionaries);
            UpdateStatus();

        }

        public override void SetData(TaskData data, ScrollRect scrollRect, CommonDictionaries gameDictionaries)
        {
            Data = data;
            Scroll = scrollRect.ThrowIfArgumentNull();
            CommonDataDictionaries = gameDictionaries.ThrowIfArgumentNull();
        }

        private void UpdateStatus()
        {
            TaskControllerButton.Clear();

            var requireTime = new TimeSpan(_model.RequireHour, 0, 0);

            m_disposable?.Dispose();
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
            TimeSliderDisposable?.Dispose();
            TimeSliderDisposable = null;
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
            _clientRewardService.ShowReward(m_gameReward);
            _onGetReward.Execute(this);
        }
    }
}