using System;
using System.Collections.Generic;
using City.Buildings.TaskGiver.Abstracts;
using City.TaskBoard;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Models.Data;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.Taskboards;
using UniRx;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseTaskboardController<TaskboardView>
    {
        private readonly GameResource m_simpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private readonly GameResource m_specialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private readonly GameResource m_costReplacement = new GameResource(ResourceType.Diamond, 10);

        private readonly ReactiveCommand<BaseTaskController> m_onCompleteTask = new();

        private TaskDataComparer m_comparer;

        public IObservable<BaseTaskController> OnCompleteTask => m_onCompleteTask;

        protected override void OnStart()
        {
            m_comparer = new TaskDataComparer(_commonDictionaries);
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
            _taskBoardData = CommonGameData.City.TaskBoardData;
            _taskBoardData.ListTasks.Sort(m_comparer);

            foreach (var taskData in _taskBoardData.ListTasks) CreateTask(taskData);


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
            var message = new T { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                var newTasks = _jsonConverter.Deserialize<List<TaskData>>(result);

                foreach (var taskData in newTasks)
                {
                    _taskBoardData.ListTasks.Add(taskData);
                    CreateTask(taskData);
                }

                _resourceStorageController.SubtractResource(cost);
                SetCostReplacement();
            }
        }

        private void SetCostReplacement()
        {
            var taskCanReplaceCount = _taskBoardData.ListTasks.FindAll(x => x.Status == TaskStatusType.NotStart).Count;
            var cost = m_costReplacement * taskCanReplaceCount;
            View.BuyReplacementButton.ChangeCost(cost, StartReplacement);

            View.BuyReplacementButton.gameObject.SetActive(taskCanReplaceCount != 0);
        }

        private async UniTaskVoid ReplacementNotWorkTask()
        {
            var message = new ReplaceTasksMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var tasksForReplacement = _taskBoardData.ListTasks
                    .FindAll(x => x.Status == TaskStatusType.NotStart);

                var cost = m_costReplacement * tasksForReplacement.Count;
                foreach (var taskForReplace in tasksForReplacement)
                {
                    _taskBoardData.ListTasks.Remove(taskForReplace);
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

                var newTasks = _jsonConverter.Deserialize<List<TaskData>>(result);

                newTasks.Sort(m_comparer);
                foreach (var taskData in newTasks)
                {
                    _taskBoardData.ListTasks.Add(taskData);
                    CreateTask(taskData);
                }

                _resourceStorageController.SubtractResource(cost);
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