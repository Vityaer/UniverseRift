using System;
using System.Collections.Generic;
using City.Buildings.TaskGiver.Abstracts;
using City.Buildings.TaskGiver.TaskControllers;
using City.TaskBoard;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Models.Common;
using Models.Data;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.Taskboards;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseTaskboardController<TaskboardView>
    {
        [Inject] private readonly CommonGameData m_commonGameData;

        private readonly GameResource m_simpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private readonly GameResource m_specialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private readonly GameResource m_costReplacement = new GameResource(ResourceType.Diamond, 10);

        private readonly ReactiveCommand<BaseTaskController> m_onCompleteTask = new();

        private TaskDataComparer m_comparer;

        public IObservable<BaseTaskController> OnCompleteTask => m_onCompleteTask;

        protected override void OnStart()
        {
            m_comparer = new TaskDataComparer(CommonDictionaries);
            View.BuySimpleTaskButton.ChangeCost(m_simpleTaskCost, BuySimpleTask);
            View.BuySpecialTaskButton.ChangeCost(m_specialTaskCost, BuySpecialTask);

            View.LowRatingFilterTasksButton.OnClickAsObservable()
                .Subscribe(_ => ShowLowRatingTask())
                .AddTo(Disposables);

            View.HighRatingFilterTasksButton.OnClickAsObservable()
                .Subscribe(_ => ShowHighRatingTask())
                .AddTo(Disposables);

            View.NoFilterTasksButton.OnClickAsObservable()
                .Subscribe(_ => ShowAllTasks())
                .AddTo(Disposables);
        }


        private void BuySimpleTask()
        {
            BuyTask<BuySimpleTaskMessage>(m_simpleTaskCost).Forget();
        }

        private void BuySpecialTask()
        {
            BuyTask<BuySpecialTaskMessage>(m_specialTaskCost).Forget();
        }

        private void ShowAllTasks()
        {
            TaskControllers.ForEach(taskController => taskController.gameObject.SetActive(true));
        }

        private void ShowHighRatingTask()
        {
            foreach (var taskController in TaskControllers)
                taskController.gameObject.SetActive(taskController.Model.Rating is > 4 and <= 7);
        }

        private void ShowLowRatingTask()
        {
            foreach (var taskController in TaskControllers)
                taskController.gameObject.SetActive(taskController.Model.Rating <= 4);
        }

        protected override void OnLoadGame()
        {
            TaskBoardData = CommonGameData.City.TaskBoardData;
            TaskBoardData.ListTasks.Sort(m_comparer);

            foreach (var taskData in TaskBoardData.ListTasks) CreateTask(taskData);

            DateTime startDateTime = TimeUtils.ParseTime(m_commonGameData.City.TaskBoardData.CreateTasksLeftTime);
            View.SliderEventLeftTime.SetData(startDateTime, Constants.Game.DayTime);
            View.SetTaskCount(m_commonGameData.City.TaskBoardData.TaskCountForCreate, 5);
            CheckNews();
            SetCostReplacement();
        }

        private void CheckNews()
        {
            var taskInNotWork =
                TaskControllers.Find(taskController => (taskController.GetTask.Status != TaskStatusType.InWork));
            OnNewsStatusChangeInternal.Execute(taskInNotWork != null);
        }

        protected override void OnFinishTask(BaseTaskController taskController)
        {
            m_onCompleteTask.Execute(taskController);
        }

        public override void OnHide()
        {
            CheckNews();
            base.OnHide();
        }

        private async UniTaskVoid BuyTask<T>(GameResource cost) where T : AbstractMessage, new()
        {
            T message = new T { PlayerId = CommonGameData.PlayerInfoData.Id };
            string result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var newTasks = JsonConverter.Deserialize<List<TaskData>>(result);

                foreach (TaskData taskData in newTasks)
                {
                    TaskBoardData.ListTasks.Add(taskData);
                    CreateTask(taskData);
                }

                ResourceStorageController.SubtractResource(cost);
                SetCostReplacement();
            }
        }

        private void SetCostReplacement()
        {
            int taskCanReplaceCount = TaskBoardData.ListTasks.FindAll(x => x.Status == TaskStatusType.NotStart).Count;
            GameResource cost = m_costReplacement * taskCanReplaceCount;
            View.BuyReplacementButton.ChangeCost(cost, StartReplacement);

            View.BuyReplacementButton.gameObject.SetActive(taskCanReplaceCount != 0);
        }

        private async UniTaskVoid ReplacementNotWorkTask()
        {
            var message = new ReplaceTasksMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var tasksForReplacement = TaskBoardData.ListTasks
                    .FindAll(x => x.Status == TaskStatusType.NotStart);

                var cost = m_costReplacement * tasksForReplacement.Count;
                foreach (var taskForReplace in tasksForReplacement)
                {
                    TaskBoardData.ListTasks.Remove(taskForReplace);
                    var replace = taskForReplace;
                    var taskController = TaskControllers
                        .Find(x => x.GetTask == replace);

                    if (taskController == null)
                    {
                        Debug.LogError("taskController is null");
                        continue;
                    }

                    TaskControllers.Remove(taskController);
                    UnityEngine.Object.Destroy(taskController.gameObject);
                }

                var newTasks = JsonConverter.Deserialize<List<TaskData>>(result);

                newTasks.Sort(m_comparer);
                foreach (var taskData in newTasks)
                {
                    TaskBoardData.ListTasks.Add(taskData);
                    CreateTask(taskData);
                }

                ResourceStorageController.SubtractResource(cost);
                SetCostReplacement();
            }
        }

        private void StartReplacement()
        {
            ReplacementNotWorkTask().Forget();
        }

        protected override void OnStartTask(BaseTaskController taskController)
        {
            SetCostReplacement();
        }
    }
}