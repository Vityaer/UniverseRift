using System;
using System.Collections.Generic;
using System.Linq;
using City.Achievements;
using City.Buildings.Requirements;
using City.Panels.DailyTasks;
using City.Panels.SubjectPanels.Common;
using Common.Db.CommonDictionaries;
using Models.Achievments;
using Models.Data;
using UiExtensions.Panels;
using UniRx;
using VContainer;

namespace City.Panels.MonthTasks.Abstractions
{
    public abstract class BaseAchievmentPanelController<T> : UiPanelController<T>
        where T : BaseAchievmentPanelView
    {
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] protected readonly SubjectDetailController SubjectDetailController;

        private readonly List<AchievmentView> m_achievmentViews = new();

        protected abstract string AcievmentContainerName { get; }

        protected override void OnLoadGame()
        {
            List<Type> types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList(); //class not matter

            foreach (string taskId in m_commonDictionaries.AchievmentContainers[AcievmentContainerName].TaskIds)
            {
                AchievmentModel taskModel = m_commonDictionaries.Achievments[taskId];
                AchievmentData taskData = CommonGameData.AchievmentStorage.Achievments
                    .Find(data => data.ModelId.Equals(taskId));

                if (taskData == null)
                {
                    UnityEngine.Debug.LogError($"Not found data: {taskId}");
                    continue;
                }

                AchievmentView taskPrefab = UnityEngine.Object.Instantiate(View.Prefab, View.Content);

                Type type = types.Find(type => type.Name.Equals(taskModel.ImplementationName));
                GameAchievment gameTask = Activator.CreateInstance(type) as GameAchievment;

                Resolver.Inject(gameTask);
                gameTask.SetData(taskModel, taskData);

                Resolver.Inject(taskPrefab);
                m_achievmentViews.Add(taskPrefab);
                taskPrefab.ObserverComplete.Subscribe(_ => CheckNews()).AddTo(Disposables);
                taskPrefab.SetData(gameTask, View.Scroll, m_commonDictionaries);

                taskPrefab.SetReward(SubjectDetailController);
            }

            base.OnLoadGame();
            CheckNews();
        }


        private void CheckNews()
        {
            OnNewsStatusChangeInternal.Execute(
                m_achievmentViews.Any(achievment => achievment.CheckDoneForReward()));
        }
    }
}