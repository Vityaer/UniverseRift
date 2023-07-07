using City.Buildings.Abstractions;
using City.TaskBoard;
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

        private GameResource costReplacement = new GameResource(ResourceType.Diamond, 10f);

        private bool isLoadedTask = false;
        private TaskGiverModel taskGiverBuilding;
        private ObserverDoneTask observerDoneTasks = new ObserverDoneTask();

        protected override void OnStart()
        {
            taskGiverBuilding = CommonGameData.City.TaskboardSave;
            //View.BuySimpleTaskButton.RegisterOnBuy(CreateSimpleTask);
            //View.BuySpecialTaskButton.RegisterOnBuy(CreateSprecialTask);
        }

        public override void OnHide()
        {
            foreach (TaskController task in taskControllers)
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
        public void CreateSimpleTask(int count = 1)
        {
            CreateTask(_taskProvider.GetSimpleTask());
        }

        public void CreateSprecialTask(int count = 1)
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
            //costReplacement.Count = 10f * tasks.FindAll(x => x.status == TaskStatusType.NotStart).Count;
            //View.BuyReplacementButton.UpdateCost(costReplacement, ReplacementNotWorkTask);
        }

        public void ReplacementNotWorkTask(int count = 1)
        {
            //List<TaskModel> tasksForReplacement = tasks.FindAll(x => x.status == TaskStatusType.NotStart);
            //for (int i = 0; i < tasksForReplacement.Count; i++)
            //{
            //    tasks.Remove(tasksForReplacement[i]);
            //    Destroy(taskControllers.Find(x => x.GetTask == tasksForReplacement[i])?.gameObject);
            //}
            //for (int i = 0; i < tasksForReplacement.Count; i++)
            //{
            //    CreateTask(patternTasks.GetSimpleTask());
            //}
            //SaveGame();
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