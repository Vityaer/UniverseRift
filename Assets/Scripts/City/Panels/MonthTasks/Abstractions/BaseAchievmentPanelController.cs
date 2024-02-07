using City.Achievements;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Requirement;
using Db.CommonDictionaries;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Panels.MonthTasks.Abstractions
{
    public abstract class BaseAchievmentPanelController<T> : UiPanelController<T>
        where T : BaseAchievmentPanelView
    {
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private List<AchievmentView> _achievmentViews = new();

        protected abstract string AcievmentContainerName { get; }

        protected override void OnLoadGame()
        {
            var types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList();//class not matter

            foreach (var taskId in _commonDictionaries.AchievmentContainers[AcievmentContainerName].TaskIds)
            {
                var taskModel = _commonDictionaries.Achievments[taskId];
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
                taskPrefab.SetData(gameTask, View.Scroll);

                _achievmentViews.Add(taskPrefab);
            }

            base.OnLoadGame();
        }
    }
}
