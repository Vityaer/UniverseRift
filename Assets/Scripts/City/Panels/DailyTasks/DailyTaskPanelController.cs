using City.Achievements;
using City.Buildings.Requirement;
using City.Panels.BatllepasPanels;
using City.Panels.DailyTasks;
using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Resourses;
using Db.CommonDictionaries;
using Models.Common;
using Models.Data.Inventories;
using Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailyTaskPanelController : UiPanelController<DailyTaskPanelView>, IStartable
    {
        private const int CANDY_COUNT = 6;
        private const string DAILY_TASKS = "DailyTasks";

        private const int REWARD_COUNT = 30;

        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly SubjectDetailController _subjectDetailController;

        private int _currentProgress = 0;
        private List<int> _idReceivedReward = new List<int>(REWARD_COUNT);

        private List<AchievmentView> _achievmentViews = new();

        public override void Start()
        {
            _resourceStorageController.Subscribe(ResourceType.EventAgentMonet, OnGetMonet).AddTo(Disposables);
            View.OpenBattlePasPanelButton.OnClickAsObservable().Subscribe(_ => OpenBattlePasPanel()).AddTo(Disposables);
            base.Start();
        }

        public void OnGetMonet(GameResource res)
        {
            _currentProgress = res.ConvertToInt();
            View.miniSliderAmount.SetAmount(OverMonet(), 100);
        }

        private void OpenBattlePasPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<BattlepasPanelController>(openType: OpenType.Exclusive);
        }

        protected override void OnLoadGame()
        {
            CreateTaskViews();
        }

        private void CreateTaskViews()
        {
            var types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList();//class not matter

            foreach (var taskId in _commonDictionaries.AchievmentContainers[DAILY_TASKS].TaskIds)
            {
                var taskModel = _commonDictionaries.Achievments[taskId];

                if (CommonGameData.CycleEventsData.CurrentEventType == GameEventType.Sweet)
                {
                    foreach (var stage in taskModel.Stages)
                    {
                        stage.Reward.Resources.Add(
                            new ResourceData
                            {
                                Type = ResourceType.Candy,
                                Amount = new(CANDY_COUNT)
                            }
                        );
                    }
                }

                var taskData = CommonGameData.AchievmentStorage.Achievments
                    .Find(data => data.ModelId.Equals(taskId));

                if (taskData == null)
                {
                    UnityEngine.Debug.LogError($"Not found data: {taskId}");
                    continue;
                }

                var taskPrefab = UnityEngine.Object.Instantiate(View.AchievmentViewPrefab, View.Content);

                var type = types.Find(type => type.Name.Equals(taskModel.ImplementationName));
                var gameTask = Activator.CreateInstance(type) as GameAchievment;

                Resolver.Inject(gameTask);
                gameTask.SetData(taskModel, taskData);

                Resolver.Inject(taskPrefab);
                taskPrefab.SetData(gameTask, View.Scroll);

                _achievmentViews.Add(taskPrefab);
                taskPrefab.SetReward(_subjectDetailController);
            }
        }

        public int GetMaxLevelReceivedReward()
        {
            int result = 0;
            for (int i = 0; i < _idReceivedReward.Count; i++)
            {
                if (_idReceivedReward[i] > result)
                {
                    result = _idReceivedReward[i];
                }
            }
            return result;
        }

        public int OverMonet()
        {
            int maxLevel = GetMaxLevelReceivedReward();
            return _currentProgress - maxLevel * 100;
        }
    }
}