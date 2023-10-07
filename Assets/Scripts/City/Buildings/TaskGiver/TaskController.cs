using City.TaskBoard;
using Common.Resourses;
using Models.Data;
using Models.Tasks;
using System;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;
using Models.Common;
using Network.DataServer;
using Network.DataServer.Messages.City.Taskboards;
using VContainer;
using Assets.Scripts.ClientServices;
using ClientServices;
using Common.Rewards;
using System.Globalization;
using Sirenix.Utilities;

namespace City.Buildings.TaskGiver
{
    public class TaskController : ScrollableUiView<TaskData>
    {
        [Inject] private CommonGameData _commonGameData;
        [Inject] private ResourceStorageController _resourceStorageController;
        [Inject] private ClientRewardService _clientRewardService;

        [Header("UI")]
        public TextMeshProUGUI Name;
        public GameObject objectCurrentTime;
        public SliderTime sliderTime;
        public ButtonCostController TaskControllerButton;
        public RatingHero ratingController;
        public RewardUIController RewardUIController;

        private IDisposable _disposable;
        private GameTaskModel _model;
        private ReactiveCommand<TaskData> _onGetReward = new ReactiveCommand<TaskData>(); 
        
        public TaskData GetTask => Data;
        public IObservable<TaskData> OnGetReward => _onGetReward;
 

        public void SetData(TaskData data, ScrollRect scrollRect, GameTaskModel model)
        {
            SetData(data, scrollRect);
            _model = model;

            Name.text = _model.Id;
            ratingController.ShowRating(_model.Rating);
            RewardUIController.ShowReward(_model.Reward);
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
            //Debug.Log($"start time: {Data.DateTimeStart}");

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

            switch (Data.Status)
            {
                case TaskStatusType.NotStart:
                    sliderTime.SetMaxValue(requireTime);
                    TaskControllerButton.SetLabel("Start");
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => StartTask().Forget());
                    break;

                case TaskStatusType.InWork:
                    sliderTime.RegisterOnFinish(FinishFromSlider);
                    var costFastFinish = new GameResource(ResourceType.Diamond, _model.Rating * 10, 0);
                    TaskControllerButton.SetCost(costFastFinish);
                    sliderTime.SetData(startDateTime, requireTime);
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => BuyProgress().Forget());
                    break;

                case TaskStatusType.Done:
                    sliderTime.SetData(startDateTime, requireTime);
                    sliderTime.UnregisterOnFinish(FinishFromSlider);
                    TaskControllerButton.SetLabel("Complete");
                    _disposable = TaskControllerButton.OnClick.Subscribe(_ => GetReward().Forget());
                    break;
            }
        }
        public void StopTimer()
        {
            sliderTime.StopTimer();
        }
        private void FinishFromSlider()
        {
            FinishTask();
        }

        private void FinishTask()
        {
            sliderTime.UnregisterOnFinish(FinishFromSlider);
            UpdateStatus();
        }

        //Action button
        public async UniTaskVoid StartTask()
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
            }
        }

        public async UniTaskVoid BuyProgress()
        {
            var message = new BuyFastCompleteTaskMessage { PlayerId = _commonGameData.PlayerInfoData.Id, TaskId = Data.TaskId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var cost = new GameResource(ResourceType.Diamond, 10 * _model.Rating, 0);
                _resourceStorageController.SubtractResource(cost);
                TaskControllerButton.Clear();
                sliderTime.SetFinish();
                Data.Status = TaskStatusType.Done;
                var reward = new GameReward(_model.Reward);
                _clientRewardService.ShowReward(reward);
                _onGetReward.Execute(Data);
            }
        }

        public async UniTaskVoid GetReward()
        {
            var message = new CompleteTaskMessage { PlayerId = _commonGameData.PlayerInfoData.Id, TaskId = Data.TaskId };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                Data.Status = TaskStatusType.Done;
                var reward = new GameReward(_model.Reward);
                _clientRewardService.ShowReward(reward);
                _onGetReward.Execute(Data);
            }
        }


    }
}