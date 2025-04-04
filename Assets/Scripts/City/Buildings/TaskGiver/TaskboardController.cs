﻿using City.Buildings.Abstractions;
using City.TaskBoard;
using ClientServices;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Models.Data;
using Models.Data.Buildings.Taskboards;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Messages.City.Taskboards;
using System;
using System.Collections.Generic;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseBuilding<TaskboardView>, IStartable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly ResourceStorageController _resourceStorageController;

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
            View.BuySimpleTaskButton.ChangeCost(_simpleTaskCost, () => BuyTask<BuySimpleTaskMessage>(_simpleTaskCost).Forget());
            View.BuySpecialTaskButton.ChangeCost(_specialTaskCost, () => BuyTask<BuySpecialTaskMessage>(_specialTaskCost).Forget());
        }

        protected override void OnLoadGame()
        {
            _taskBoardData = CommonGameData.City.TaskBoardData;

            foreach (var taskData in _taskBoardData.ListTasks)
            {
                var newTaskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                _taskControllers.Add(newTaskController);
                Resolver.Inject(newTaskController);
                Resolver.Inject(newTaskController.TaskControllerButton);
                newTaskController.SetData(taskData, View.Scroll, _commonDictionaries.GameTaskModels[taskData.TaskModelId]);
                newTaskController.OnGetReward.Subscribe(DeleteTask).AddTo(Disposables);
            }

            CheckNews();
            SetCostReplacement();
        }

        private void CheckNews()
        {
            var taskInNotWork = _taskControllers.Find(taskController => (taskController.GetTask.Status != TaskStatusType.InWork));
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

            foreach (var task in _taskControllers)
            {
                task.StopTimer();
            }
        }

        public async UniTaskVoid BuyTask<T>(GameResource cost) where T : AbstractMessage, new()
        {
            var message = new T { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);
            if (!string.IsNullOrEmpty(result))
            {
                _resourceStorageController.SubtractResource(cost);
            }
        }

        private void SetCostReplacement()
        {
            var cost = _costReplacement * _taskBoardData.ListTasks.FindAll(x => x.Status == TaskStatusType.NotStart).Count;
            View.BuyReplacementButton.ChangeCost(_costReplacement, () => ReplacementNotWorkTask().Forget());
        }

        public async UniTaskVoid ReplacementNotWorkTask()
        {
            var message = new ReplaceTasksMessage { PlayerId = CommonGameData.PlayerInfoData.Id };
            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                var tasksForReplacement = _taskBoardData.ListTasks.FindAll(x => x.Status == TaskStatusType.NotStart);

                foreach (var task in tasksForReplacement)
                {
                    _taskControllers.Remove(_taskControllers.Find(x => x.GetTask == task));
                }

                var cost = _costReplacement * tasksForReplacement.Count;
                for (var i = 0; i < tasksForReplacement.Count; i++)
                {
                    _taskBoardData.ListTasks.Remove(tasksForReplacement[i]);
                    UnityEngine.Object.Destroy(_taskControllers.Find(x => x.GetTask == tasksForReplacement[i])?.gameObject);
                }

                foreach (var taskData in _taskBoardData.ListTasks)
                {
                    var newTaskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                    _taskControllers.Add(newTaskController);
                    Resolver.Inject(newTaskController);
                    Resolver.Inject(newTaskController.TaskControllerButton);
                    newTaskController.SetData(taskData, View.Scroll, _commonDictionaries.GameTaskModels[taskData.TaskModelId]);
                    newTaskController.OnGetReward.Subscribe(DeleteTask).AddTo(Disposables);
                }

                _resourceStorageController.SubtractResource(cost);
            }
        }
    }
}