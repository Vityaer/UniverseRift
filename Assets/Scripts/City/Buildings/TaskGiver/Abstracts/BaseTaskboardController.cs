using System.Collections.Generic;
using City.Buildings.Abstractions;
using ClientServices;
using Common.Db.CommonDictionaries;
using Misc.Json;
using Models.Data;
using Models.Data.Buildings.Taskboards;
using UniRx;
using VContainer;

namespace City.Buildings.TaskGiver.Abstracts
{
    public class BaseTaskboardController<T> : BaseBuilding<T>
    where T : BaseTaskboardView
    {
        [Inject] protected readonly ResourceStorageController _resourceStorageController;
        [Inject] protected readonly IJsonConverter _jsonConverter;
        [Inject] protected readonly CommonDictionaries _commonDictionaries;
        
        protected TaskBoardData _taskBoardData;
        protected List<BaseTaskController> TaskControllers = new List<BaseTaskController>();

        protected virtual void CreateTask(TaskData taskData)
        {
            var newTaskController = UnityEngine.Object.Instantiate(View.Prefab, View.Content);
            TaskControllers.Add(newTaskController);
            Resolver.Inject(newTaskController);
            Resolver.Inject(newTaskController.TaskControllerButton);
            newTaskController.SetData(taskData, View.Scroll, _commonDictionaries.GameTaskModels[taskData.TaskModelId]);
            newTaskController.OnGetReward.Subscribe(FinishTask).AddTo(Disposables);
            newTaskController.OnStartTask.Subscribe(OnStartTask).AddTo(Disposables);
        }

        public virtual void RecreateTaskControllers(List<TaskData> taskDatas)
        {
            for (var i = TaskControllers.Count - 1; i >= 0; i--)
            {
                DeleteTask(TaskControllers[i]);
            }

            foreach (var taskData in taskDatas)
            {
                CreateTask(taskData);
            }
        }

        protected void FinishTask(BaseTaskController taskController)
        {
            OnFinishTask(taskController);
            DeleteTask(taskController);
        }

        protected virtual void OnFinishTask(BaseTaskController taskController)
        {
            
        }

        protected void DeleteTask(BaseTaskController taskController)
        {
            TaskControllers.Remove(taskController);
            _taskBoardData.ListTasks.Remove(taskController.GetTask);
            UnityEngine.Object.Destroy(taskController.gameObject);
        }

        protected virtual void OnStartTask(BaseTaskController taskController)
        {
            
        }
        
        public override void OnHide()
        {
            foreach (var task in TaskControllers) task.StopTimer();
        }
    }
}