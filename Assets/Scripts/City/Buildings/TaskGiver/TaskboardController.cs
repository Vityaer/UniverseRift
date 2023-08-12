using City.Buildings.Abstractions;
using City.TaskBoard;
using Common;
using Common.Observers;
using Common.Resourses;
using Db.CommonDictionaries;
using Models;
using Models.Common.BigDigits;
using Models.Data;
using Models.Data.Buildings.Taskboards;
using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseBuilding<TaskboardView>, IStartable
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        public int countFreeTaskOnDay = 6;
        public List<TaskData> _tasks = new List<TaskData>();
        private List<TaskController> _taskControllers = new List<TaskController>();

        private GameResource _simpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private GameResource _specialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private GameResource _costReplacement = new GameResource(ResourceType.Diamond, 10f);

        private TaskBoardData _taskBoardData;
        private ObserverDoneTask observerDoneTasks = new ObserverDoneTask();

        protected override void OnStart()
        {
            View.BuySimpleTaskButton.ChangeCost(_simpleTaskCost, CreateSimpleTask);
            View.BuySpecialTaskButton.ChangeCost(_specialTaskCost, CreateSprecialTask);
        }

        protected override void OnLoadGame()
        {
            _taskBoardData = CommonGameData.City.TaskBoardData;

            foreach (var taskData in _taskBoardData.ListTasks)
            {
                var newTaskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                _taskControllers.Add(newTaskController);
                newTaskController.SetData(taskData, View.Scroll, _commonDictionaries.GameTaskModels[taskData.TaskModelId]);
            }

            SetCostReplacement();
        }

        public override void OnHide()
        {
            foreach (var task in _taskControllers)
            {
                task.StopTimer();
            }
        }

       
        public void CreateSimpleTask()
        {
            //CreateTask(_taskProvider.GetSimpleTask());
        }

        public void CreateSprecialTask()
        {
            //CreateTask(_taskProvider.GetSpecialTask());
        }

        public void UpdateSave()
        {
            //SaveGame();
            SetCostReplacement();
        }

        public void FinishTask(TaskController taskController)
        {
            //TaskModel workTask = taskController.GetTask;
            //workTask.GetReward();
            //OnDoneTask(workTask.Rating);
            //Destroy(taskController.gameObject);
            //tasks.Remove(workTask);
            //SaveGame();
        }

        private void SetCostReplacement()
        {
            _costReplacement *= _tasks.FindAll(x => x.Status == TaskStatusType.NotStart).Count;
            View.BuyReplacementButton.ChangeCost(_costReplacement, ReplacementNotWorkTask);
        }

        public void ReplacementNotWorkTask()
        {
            var tasksForReplacement = _tasks.FindAll(x => x.Status == TaskStatusType.NotStart);
            for (var i = 0; i < tasksForReplacement.Count; i++)
            {
                _tasks.Remove(tasksForReplacement[i]);
                UnityEngine.Object.Destroy(_taskControllers.Find(x => x.GetTask == tasksForReplacement[i])?.gameObject);
            }
            for (int i = 0; i < tasksForReplacement.Count; i++)
            {
                CreateSimpleTask();
            }
        }
    }
}