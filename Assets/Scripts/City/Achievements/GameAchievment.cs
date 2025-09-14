using City.Acievements;
using ClientServices;
using Common.Rewards;
using Db.CommonDictionaries;
using Models.Achievments;
using Models.Common.BigDigits;
using Models.Data;
using System;
using UniRx;
using VContainer;

namespace City.Achievements
{
    public abstract class GameAchievment : IDisposable
    {
        [Inject] protected readonly ClientRewardService ClientRewardService;
        [Inject] protected readonly CommonDictionaries CommonDictionaries;

        private AchievmentModel m_model;
        private AchievmentData m_data;

        protected readonly CompositeDisposable Disposables = new CompositeDisposable();
        private BigDigit m_currentProgress;

        public int CurrentStage => m_data.CurrentStage;
        public int CountStage => m_model.Stages.Count;
        public bool IsComplete => m_data.IsComplete;
        public BigDigit Progress => m_currentProgress;
        public int Id => m_data.Id;
        public string ModelId => m_model.Id;
        public readonly ReactiveCommand OnChangeData = new();

        public void SetData(AchievmentModel model, AchievmentData data)
        {
            m_model = model;
            m_data = data;
            m_currentProgress = new BigDigit(m_data.Amount, m_data.E10);
            Subscribe();
        }

        protected abstract void Subscribe();

        protected void Unsubscribe()
        {
            Disposables.Dispose();
        }

        public void AddProgress(BigDigit amount)
        {
            if (!IsComplete)
            {
                switch (m_model.ProgressType)
                {
                    case ProgressType.StorageAmount:
                        m_currentProgress.Add(amount);
                        break;
                    case ProgressType.MaxAmount:
                        if (m_currentProgress > amount)
                            m_currentProgress = amount;
                        break;
                    case ProgressType.CurrentAmount:
                        m_currentProgress = amount;
                        break;
                }

                if (m_currentProgress.CheckCount(GetRequireFinishCount()))
                {
                    m_currentProgress = new BigDigit(GetRequireFinishCount().Mantissa, GetRequireFinishCount().E10);
                    m_data.IsComplete = true;
                }

            }
            OnChangeData.Execute();
        }

        public void SetProgress(int newCurrentStage, BigDigit newProgress)
        {
            m_data.CurrentStage = newCurrentStage;
            m_currentProgress = newProgress;
            if (m_currentProgress.CheckCount(GetRequireFinishCount()))
            {
                m_data.IsComplete = true;
            }
        }

        private BigDigit GetRequireFinishCount()
        {
            return m_model.GetRequireFinishCount();
        }

        public bool CheckCount()
        {
            return m_currentProgress.CheckCount(GetRequireCount());
        }

        public BigDigit GetRequireCount()
        {
            return m_model.GetRequireCount(m_data.CurrentStage);
        }

        public void NextStage()
        {
            m_data.CurrentStage++;
        }

        public GameReward GetReward()
        {
            var currentStage = m_data.CurrentStage;
            if(currentStage >= m_model.Stages.Count)
                currentStage = m_model.Stages.Count - 1;

            var rewardModel = m_model.GetReward(currentStage);
            return new GameReward(rewardModel, CommonDictionaries);
        }

        public void ShowAndGiveReward()
        {
            var reward = GetReward();
            ClientRewardService.ShowReward(reward);
        }
        
        public void GiveReward()
        {
            var reward = GetReward();
            ClientRewardService.GetReward(reward);
        }

        public void ClearProgress()
        {
            //_data.Clear();
        }

        public void Dispose()
        {
            if (!Disposables.IsDisposed)
                Disposables.Dispose();
        }
    }
}
