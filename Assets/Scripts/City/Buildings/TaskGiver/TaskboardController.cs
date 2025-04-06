using System;
using System.Collections.Generic;
using City.Buildings.Abstractions;
using City.TaskBoard;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Misc.Json;
using Models.Data;
using Models.Data.Buildings.Taskboards;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.Taskboards;
using UniRx;
using UnityEngine;
using VContainer;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseBuilding<TaskboardView>
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly IJsonConverter _jsonConverter;

        public int countFreeTaskOnDay = 6;
        private List<TaskController> _taskControllers = new List<TaskController>();

        private GameResource _simpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private GameResource _specialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private GameResource _costReplacement = new GameResource(ResourceType.Diamond, 10f);

        private TaskBoardData _taskBoardData;
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

        private void DeleteTask(TaskData task)
        {
            var controller = _taskControllers.Find(controller => controller.GetTask == task);
            _taskControllers.Remove(controller);
            _taskBoardData.ListTasks.Remove(task);
            _onCompleteTask.Execute(controller);
            UnityEngine.Object.Destroy(controller.gameObject);
        }

        public override void OnHide()
        {
            CheckNews();

            foreach (var task in _taskControllers) task.StopTimer();
        }

        public async UniTaskVoid BuyTask<T>(GameResource cost) where T : AbstractMessage, new()
        {
            var message = new T { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result)) _resourceStorageController.SubtractResource(cost);
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
            }
        }

        private void StartReplacement()
        {
            ReplacementNotWorkTask().Forget();
        }

        private void CreateTask(TaskData taskData)
        {
            var newTaskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
            _taskControllers.Add(newTaskController);
            Resolver.Inject(newTaskController);
            Resolver.Inject(newTaskController.TaskControllerButton);
            newTaskController.SetData(taskData, View.Scroll, _commonDictionaries.GameTaskModels[taskData.TaskModelId]);
            newTaskController.OnGetReward.Subscribe(DeleteTask).AddTo(Disposables);
        }
    }
}