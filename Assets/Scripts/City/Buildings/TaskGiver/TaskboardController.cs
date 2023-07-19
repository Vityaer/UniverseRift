using City.Buildings.Abstractions;
using City.TaskBoard;
using Common;
using Common.Observers;
using Common.Resourses;
using Models;
using Models.Common.BigDigits;
using Models.Data;
using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.TaskGiver
{
    public class TaskboardController : BaseBuilding<TaskboardView>, IStartable
    {
        [Inject] private readonly GameTaskProvider _taskProvider;

        public int countFreeTaskOnDay = 6;
        public List<TaskData> tasks = new List<TaskData>();
        private List<TaskController> taskControllers = new List<TaskController>();

        private GameResource SimpleTaskCost = new GameResource(ResourceType.SimpleTask, 1, 0);
        private GameResource SpecialTaskCost = new GameResource(ResourceType.SpecialTask, 1, 0);
        private GameResource costReplacement = new GameResource(ResourceType.Diamond, 10f);

        private bool isLoadedTask = false;
        private TaskGiverModel taskGiverBuilding;
        private ObserverDoneTask observerDoneTasks = new ObserverDoneTask();

        protected override void OnStart()
        {
            taskGiverBuilding = CommonGameData.City.TaskboardSave;
            View.BuySimpleTaskButton.ChangeCost(SimpleTaskCost, CreateSimpleTask);
            View.BuySpecialTaskButton.ChangeCost(SpecialTaskCost, CreateSprecialTask);
            SetCostReplacement();
        }

        public override void OnHide()
        {
            foreach (var task in taskControllers)
            {
                task.StopTimer();
            }
        }

        private void FirstCreateTasks()
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                var taskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
                //taskController.SetData(tasks[i]);
            }
        }

        private void GetDailyTask()
        {
            var taskInNotWork = tasks.FindAll(x => x.Status == TaskStatusType.NotStart);

            for (int i = 0; i < countFreeTaskOnDay; i++)
            {
                if (i < taskInNotWork.Count)
                {
                    tasks.Remove(taskInNotWork[i]);
                    UnityEngine.Object.Destroy(taskControllers.Find(x => x.GetTask == taskInNotWork[i])?.gameObject);
                }

                var newTask = _taskProvider.GetSimpleTask();
                //tasks.Add(newTask);
                //var newTaskController = Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskController>();
                //taskControllers.Add(newTaskController);
                //newTaskController.SetData(newTask);
                //taskGiverBuilding.tasks = tasks;
            }
            SetCostReplacement();
            //SaveGame();
        }
        public void CreateSimpleTask()
        {
            CreateTask(_taskProvider.GetSimpleTask());
        }

        public void CreateSprecialTask()
        {
            CreateTask(_taskProvider.GetSpecialTask());
        }

        private void CreateTask(GameTask newTask)
        {
            //tasks.Add(newTask);
            //TaskController newTaskController = Instantiate(prefabTask, taskboard).gameObject.GetComponent<TaskController>();
            //taskControllers.Add(newTaskController);
            //newTaskController.SetData(newTask);
            //taskGiverBuilding.tasks = tasks;
            //SetCostReplacement();
            //SaveGame();
        }

        protected override void OnLoadGame()
        {
            //taskGiverBuilding = GameController.GetCitySave.TaskboardSave;
            //tasks = taskGiverBuilding.tasks;
            //FirstCreateTasks();
            //TimeControllerSystem.Instance.RegisterOnNewCycle(GetDailyTask, CycleRecover.Day);
            //SetCostReplacement();
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
            costReplacement *= tasks.FindAll(x => x.Status == TaskStatusType.NotStart).Count;
            View.BuyReplacementButton.ChangeCost(costReplacement, ReplacementNotWorkTask);
        }

        public void ReplacementNotWorkTask()
        {
            var tasksForReplacement = tasks.FindAll(x => x.Status == TaskStatusType.NotStart);
            for (var i = 0; i < tasksForReplacement.Count; i++)
            {
                tasks.Remove(tasksForReplacement[i]);
                UnityEngine.Object.Destroy(taskControllers.Find(x => x.GetTask == tasksForReplacement[i])?.gameObject);
            }
            for (int i = 0; i < tasksForReplacement.Count; i++)
            {
                CreateSimpleTask();
            }

            GameController.SaveGame();
        }

        public void RegisterOnDoneTask(Action<BigDigit> d, int rating)
        {
            observerDoneTasks.Add(d, rating);
        }

        public void UnregisterOnDoneTask(Action<BigDigit> d, int rating)
        {
            observerDoneTasks.Remove(d, rating);
        }

        public void OnDoneTask(int rating)
        {
            observerDoneTasks.OnDoneTask(rating);
        }
    }
}