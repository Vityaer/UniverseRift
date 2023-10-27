using City.Achievements;
using City.Buildings.Requirement;
using City.Panels.BatllepasPanels;
using City.Panels.DailyTasks;
using ClientServices;
using Common;
using Common.Resourses;
using Db.CommonDictionaries;
using Models;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Buildings.CityButtons.EventAgent
{
    public class DailyTaskPanelController : UiPanelController<DailyTaskPanelView>, IStartable
    {
        private const string DAILY_TASKS = "DailyTasks";

        private const int REWARD_COUNT = 30;
        private const int REWARD_MONET_COST = 100;
        private const string SUM_RECEIVIED_REWARD = "SumReward";
        private const char SYMBOL_1 = '1';

        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;

        private int _currentProgress = 0;
        private int _sumReward = 0;
        private List<RewardModel> _listRewards;
        private List<int> _idReceivedReward = new List<int>(REWARD_COUNT);
        private List<DailyTaskUi> _rewardsUi = new List<DailyTaskUi>(REWARD_COUNT);
        private SimpleBuildingData _progressObjectSave;

        private List<AchievmentView> _achievmentViews = new();

        public void OnGetMonet(GameResource res)
        {
            _currentProgress = res.ConvertToInt();
            View.miniSliderAmount.SetAmount(OverMonet(), 100);
        }

        public override void Start()
        {
            _resourceStorageController.Subscribe(ResourceType.EventAgentMonet, OnGetMonet).AddTo(Disposables);
            View.OpenBattlePasPanelButton.OnClickAsObservable().Subscribe(_ => OpenBattlePasPanel()).AddTo(Disposables);
            base.Start();
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
            var types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList();

            foreach (var taskId in _commonDictionaries.AchievmentContainers[DAILY_TASKS].TaskIds)
            {
                var taskModel = _commonDictionaries.Achievments[taskId];
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

                Debug.Log(gameTask.GetType());
                
                Resolver.Inject(gameTask);
                gameTask.SetData(taskModel, taskData);

                Resolver.Inject(taskPrefab);
                taskPrefab.SetData(gameTask, View.Scroll);

                _achievmentViews.Add(taskPrefab);
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

        private void OnGetReward(int index)
        {
            _idReceivedReward.Add(index);
            _sumReward += (int)Math.Pow(2, index);
        }
    }
}