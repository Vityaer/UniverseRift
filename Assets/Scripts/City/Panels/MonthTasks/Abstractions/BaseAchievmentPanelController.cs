using City.Achievements;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Requirement;
using City.Panels.SubjectPanels.Common;
using Db.CommonDictionaries;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using City.Panels.DailyTasks;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;

namespace City.Panels.MonthTasks.Abstractions
{
    public abstract class BaseAchievmentPanelController<T> : UiPanelController<T>
        where T : BaseAchievmentPanelView
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] protected readonly SubjectDetailController SubjectDetailController;

        protected List<AchievmentView> AchievmentViews = new();

        protected abstract string AcievmentContainerName { get; }

        protected override void OnLoadGame()
        {
            var types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList();//class not matter

            foreach (var taskId in m_commonDictionaries.AchievmentContainers[AcievmentContainerName].TaskIds)
            {
                var taskModel = m_commonDictionaries.Achievments[taskId];
                var taskData = CommonGameData.AchievmentStorage.Achievments
                    .Find(data => data.ModelId.Equals(taskId));

                if (taskData == null)
                {
                    UnityEngine.Debug.LogError($"Not found data: {taskId}");
                    continue;
                }

                var taskPrefab = UnityEngine.Object.Instantiate(View.Prefab, View.Content);

                var type = types.Find(type => type.Name.Equals(taskModel.ImplementationName));
                var gameTask = Activator.CreateInstance(type) as GameAchievment;

                Resolver.Inject(gameTask);
                gameTask.SetData(taskModel, taskData);

                Resolver.Inject(taskPrefab);
                taskPrefab.ObserverComplete.Subscribe(_ => CheckNews()).AddTo(Disposables);
                taskPrefab.SetData(gameTask, View.Scroll);

                AchievmentViews.Add(taskPrefab);
                taskPrefab.SetReward(SubjectDetailController);
                
            }

            base.OnLoadGame();
            CheckNews();
        }
        
        
        private void CheckNews()
        {
            OnNewsStatusChangeInternal.Execute(
                AchievmentViews.Any(achievment => achievment.CheckDoneForReward()));
        }
    }
}
