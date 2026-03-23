using System;
using System.Collections.Generic;
using System.Linq;
using City.Achievements;
using City.Acievements;
using City.Buildings.Requirements;
using City.Panels.BatllepasPanels;
using City.Panels.SubjectPanels.Common;
using City.Panels.Widgets.Particles;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Common.Resourses;
using Misc.Json;
using Models;
using Models.Achievments;
using Models.Common.BigDigits;
using Models.Data;
using Models.Data.Inventories;
using Models.Events;
using UiExtensions.Panels;
using UniRx;
using UnityEngine;
using Utils;
using Utils.DataCheckUtils;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.DailyTasks
{
    public class DailyTaskPanelController : UiPanelController<DailyTaskPanelView>
    {
        private const int CANDY_COUNT = 6;
        private const string DAILY_TASKS = "DailyTasks";

        private const int REWARD_COUNT = 30;

        private readonly ResourceStorageController m_resourceStorageController;
        private readonly CommonDictionaries m_commonDictionaries;
        private readonly SubjectDetailController m_subjectDetailController;
        private readonly IJsonConverter m_jsonConverter;

        private int m_currentProgress;
        private readonly List<int> m_idReceivedReward = new(REWARD_COUNT);

        private readonly List<DailyTaskAchievmentView> m_dailyTaskAchievments = new();
        private WrapperPool<ParticlesAttraction> m_particleAttractionPool;

        private bool m_fastChangeVisualCount = true;

        public DailyTaskPanelController(ResourceStorageController resourceStorageController,
            CommonDictionaries commonDictionaries,
            SubjectDetailController subjectDetailController,
            IJsonConverter jsonConverter)
        {
            m_resourceStorageController = resourceStorageController.ThrowIfArgumentNull();
            m_commonDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_subjectDetailController = subjectDetailController.ThrowIfArgumentNull();
            m_jsonConverter = jsonConverter.ThrowIfArgumentNull();
        }

        public override void Start()
        {
            m_particleAttractionPool = new(View.CoinsAttractorPrefab, OnCreateParticle, View.ParticlesContainer);

            m_resourceStorageController.Subscribe(ResourceType.EventAgentMonet, OnChangeCountMonet);
            View.OpenBattlePasPanelButton.OnClickAsObservable().Subscribe(_ => OpenBattlePasPanel()).AddTo(Disposables);
            base.Start();
        }

        public override void OnShow()
        {
            GameResource res = m_resourceStorageController.GetResource(ResourceType.EventAgentMonet);
            m_currentProgress = res.ConvertToInt();
            SetAmount();
            base.OnShow();
        }

        private void OnChangeCountMonet(GameResource obj)
        {
            if (m_fastChangeVisualCount) SetAmount();
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
            int overMonet = OverMonet();
            View.miniSliderAmount.SetAmount(overMonet, 100);
            if (overMonet >= 100)
                View.miniSliderAmount.ShowDone();
            else
                View.miniSliderAmount.HideDone();
        }

        private void OnParticlesStop(ParticlesAttraction particle)
        {
            m_particleAttractionPool.Release(particle);

            if (m_particleAttractionPool.CurrentCountInWork == 0) m_fastChangeVisualCount = true;
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
            List<Type> types = typeof(DailyTaskPanelController).Assembly.GetTypes().ToList(); //class not matter

            foreach (string taskId in m_commonDictionaries.AchievmentContainers[DAILY_TASKS].TaskIds)
            {
                AchievmentModel taskModel = m_commonDictionaries.Achievments[taskId];

                if (CommonGameData.CycleEventsData.CurrentEventType == GameEventType.Sweet)
                {
                    SweetEventData eventData = m_jsonConverter
                        .Deserialize<SweetEventData>(CommonGameData.CycleEventsData.CurrentCycle);

                    foreach (AchievmentStageModel stage in taskModel.Stages)
                        stage.Reward.Resources.Add(
                            new ResourceData
                            {
                                Type = eventData.ResourceType,
                                Amount = new BigDigit(CANDY_COUNT)
                            }
                        );
                }

                AchievmentData taskData = CommonGameData.AchievmentStorage.Achievments
                    .Find(data => data.ModelId.Equals(taskId));

                if (taskData == null)
                {
                    UnityEngine.Debug.LogError($"Not found data: {taskId}");
                    continue;
                }

                DailyTaskAchievmentView taskPrefab =
                    UnityEngine.Object.Instantiate(View.AchievmentViewPrefab, View.Content);

                Type type = types.Find(type => type.Name.Equals(taskModel.ImplementationName));
                GameAchievment gameTask = Activator.CreateInstance(type) as GameAchievment;

                Resolver.Inject(gameTask);
                gameTask.SetData(taskModel, taskData);

                Resolver.Inject(taskPrefab);
                m_dailyTaskAchievments.Add(taskPrefab);
                taskPrefab.ObserverComplete.Subscribe(_ => CheckNews()).AddTo(Disposables);
                taskPrefab.SetData(gameTask, View.Scroll, m_commonDictionaries);

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
            ParticlesAttraction particle = m_particleAttractionPool.Get();
            particle.SetData(achievement.GetButton.GetComponent<RectTransform>(),
                View.OpenBattlePasPanelButton.GetComponent<RectTransform>());
            particle.transform.SetParent(View.ParticlesContainer);

            m_fastChangeVisualCount = false;
            CheckNews();
        }

        private int GetMaxLevelReceivedReward()
        {
            return m_idReceivedReward.Prepend(0).Max();
        }

        private int OverMonet()
        {
            int maxLevel = GetMaxLevelReceivedReward();
            return m_currentProgress - maxLevel * 100;
        }
    }
}