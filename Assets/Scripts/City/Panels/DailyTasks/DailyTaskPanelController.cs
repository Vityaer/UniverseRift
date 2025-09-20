using System;
using System.Collections.Generic;
using System.Linq;
using City.Achievements;
using City.Buildings.Requirement;
using City.Panels.BatllepasPanels;
using City.Panels.SubjectPanels.Common;
using City.Panels.Widgets.Particles;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Resourses;
using Misc.Json;
using Models;
using Models.Common.BigDigits;
using Models.Data.Inventories;
using Models.Events;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using Utils;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.DailyTasks
{
    public class DailyTaskPanelController : UiPanelController<DailyTaskPanelView>
    {
        private const int CANDY_COUNT = 6;
        private const string DAILY_TASKS = "DailyTasks";

        private const int REWARD_COUNT = 30;

        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;
        [Inject] private readonly IJsonConverter m_jsonConverter;

        private int m_currentProgress = 0;
        private readonly List<int> m_idReceivedReward = new List<int>(REWARD_COUNT);

        private List<DailyTaskAchievmentView> m_dailyTaskAchievments = new List<DailyTaskAchievmentView>();
        private WrapperPool<ParticlesAttraction> m_particleAttractionPool;

        private bool _fastChangeVisualCount = true;

        public override void Start()
        {
            m_particleAttractionPool = new(View.CoinsAttractorPrefab, OnCreateParticle, View.ParticlesContainer);

            m_resourceStorageController.Subscribe(ResourceType.EventAgentMonet, OnChangeCountMonet);
            View.OpenBattlePasPanelButton.OnClickAsObservable().Subscribe(_ => OpenBattlePasPanel()).AddTo(Disposables);
            base.Start();
        }

        public override void OnShow()
        {
            var res = m_resourceStorageController.GetResource(ResourceType.EventAgentMonet);
            m_currentProgress = res.ConvertToInt();
            SetAmount();
            base.OnShow();
        }

        private void OnChangeCountMonet(GameResource obj)
        {
            if (_fastChangeVisualCount) SetAmount();
        }

        private void OnCreateParticle(ParticlesAttraction particle)
        {
            particle.OnFinish.Subscribe(OnParticlesStop).AddTo(Disposables);
            particle.OnParticleFinish.Subscribe(_ => OnParticleFinish()).AddTo(Disposables);
        }

        private void OnParticleFinish()
        {
            m_currentProgress += View.AddOnEachCoin;
            SetAmount();
        }

        private void SetAmount()
        {
            var overMonet = OverMonet();
            View.miniSliderAmount.SetAmount(overMonet, 100);
            if (overMonet >= 100)
                View.miniSliderAmount.ShowDone();
            else
                View.miniSliderAmount.HideDone();
        }

        private void OnParticlesStop(ParticlesAttraction particle)
        {
            m_particleAttractionPool.Release(particle);

            if (m_particleAttractionPool.CurrentCountInWork == 0) _fastChangeVisualCount = true;
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
            var types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList(); //class not matter

            foreach (var taskId in m_commonDictionaries.AchievmentContainers[DAILY_TASKS].TaskIds)
            {
                var taskModel = m_commonDictionaries.Achievments[taskId];

                if (CommonGameData.CycleEventsData.CurrentEventType == GameEventType.Sweet)
                {
                    var eventData = m_jsonConverter
                        .Deserialize<SweetEventData>(CommonGameData.CycleEventsData.CurrentCycle);

                    foreach (var stage in taskModel.Stages)
                        stage.Reward.Resources.Add(
                            new ResourceData
                            {
                                Type = eventData.ResourceType,
                                Amount = new BigDigit(CANDY_COUNT)
                            }
                        );
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
                taskPrefab.ObserverComplete.Subscribe(_ => CheckNews()).AddTo(Disposables);
                taskPrefab.SetData(gameTask, View.Scroll);

                taskPrefab.SetReward(m_subjectDetailController);

                taskPrefab.OnGetReward.Subscribe(OnGetRewardDailyTask).AddTo(Disposables);
            }
        }

        private void CheckNews()
        {
            OnNewsStatusChangeInternal.Execute(m_dailyTaskAchievments.Any(task => task.CheckDoneForReward()));
        }

        private void OnGetRewardDailyTask(AchievmentView achievement)
        {
            var particle = m_particleAttractionPool.Get();
            particle.SetData(achievement.GetButton.GetComponent<RectTransform>(),
                View.OpenBattlePasPanelButton.GetComponent<RectTransform>());
            particle.transform.SetParent(View.ParticlesContainer);

            _fastChangeVisualCount = false;
        }

        private int GetMaxLevelReceivedReward()
        {
            return m_idReceivedReward.Prepend(0).Max();
        }

        private int OverMonet()
        {
            var maxLevel = GetMaxLevelReceivedReward();
            return m_currentProgress - maxLevel * 100;
        }
    }
}