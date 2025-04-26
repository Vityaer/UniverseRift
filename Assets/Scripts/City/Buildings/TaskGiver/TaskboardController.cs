using System;
using System.Collections.Generic;
using City.Buildings.TaskGiver.Abstracts;
using City.TaskBoard;
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
        public int countFreeTaskOnDay = 6;

        private GameResource _simpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private GameResource _specialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private GameResource _costReplacement = new GameResource(ResourceType.Diamond, 10f);

        private ReactiveCommand<TaskController> _onCompleteTask = new();

        public IObservable<TaskController> OnCompleteTask => _onCompleteTask;

        protected override void OnStart()
        {
            View.BuySimpleTaskButton.ChangeCost(_simpleTaskCost,
                () => BuyTask<BuySimpleTaskMessage>(_simpleTaskCost).Forget());
            View.BuySpecialTaskButton.ChangeCost(_specialTaskCost,
                () => BuyTask<BuySpecialTaskMessage>(_specialTaskCost).Forget());
        }

        protected override void OnLoadGame()
        {
            _taskBoardData = CommonGameData.City.TaskBoardData;

            foreach (var taskData in _taskBoardData.ListTasks) CreateTask(taskData);

            CheckNews();
            SetCostReplacement();
        }

        private void CheckNews()
        {
            var taskInNotWork =
                _taskControllers.Find(taskController => (taskController.GetTask.Status != TaskStatusType.InWork));
            _onNews.Execute(taskInNotWork != null);
        }

        protected override void OnFinishTask(TaskController taskController)
        {
            _onCompleteTask.Execute(taskController);
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
            var cost = _costReplacement * taskCanReplaceCount;
            View.BuyReplacementButton.ChangeCost(cost, StartReplacement);
        }

        private async UniTaskVoid ReplacementNotWorkTask()
        {
            var message = new ReplaceTasksMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var tasksForReplacement = _taskBoardData.ListTasks
                    .FindAll(x => x.Status == TaskStatusType.NotStart);

                var cost = _costReplacement * tasksForReplacement.Count;
                for (var i = 0; i < tasksForReplacement.Count; i++)
                {
                    var taskForReplace = tasksForReplacement[i];
                    _taskBoardData.ListTasks.Remove(taskForReplace);
                    var taskController = _taskControllers
                        .Find(x => x.GetTask == taskForReplace);

                    if (taskController == null)
                    {
                        Debug.LogError("taskController is null");
                        continue;
                    }

                    _taskControllers.Remove(taskController);
                    UnityEngine.Object.Destroy(taskController.gameObject);
                }

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

        private void StartReplacement()
        {
            ReplacementNotWorkTask().Forget();
        }

        protected override void OnStartTask(TaskController taskController)
        {
            SetCostReplacement();
        }
    }
}