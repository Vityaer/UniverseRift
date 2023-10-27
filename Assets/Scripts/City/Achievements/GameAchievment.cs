using City.Acievements;
using ClientServices;
using Common.Rewards;
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

        protected AchievmentModel Model;
        protected AchievmentData Data;

        protected CompositeDisposable Disposables = new CompositeDisposable();
        protected BigDigit CurrentProgress;

        public int CurrentStage => Data.CurrentStage;
        public int CountStage => Model.Stages.Count;
        public bool IsComplete => Data.IsComplete;
        public BigDigit Progress => CurrentProgress;
        public int Id => Data.Id;
        public string ModelId => Model.Id;
        public ReactiveCommand OnChangeData = new();

        public void SetData(AchievmentModel model, AchievmentData data)
        {
            Model = model;
            Data = data;
            CurrentProgress = new BigDigit(Data.Amount, Data.E10);
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
                switch (Model.ProgressType)
                {
                    case ProgressType.StorageAmount:
                        CurrentProgress.Add(amount);
                        break;
                    case ProgressType.MaxAmount:
                        if (CurrentProgress > amount)
                            CurrentProgress = amount;
                        break;
                    case ProgressType.CurrentAmount:
                        CurrentProgress = amount;
                        break;
                }

                if (CurrentProgress.CheckCount(GetRequireFinishCount()))
                {
                    CurrentProgress = new BigDigit(GetRequireFinishCount().Mantissa, GetRequireFinishCount().E10);
                    Data.IsComplete = true;
                }

                OnChangeData.Execute();
            }
        }

        public void SetProgress(int newCurrentStage, BigDigit newProgress)
        {
            Data.CurrentStage = newCurrentStage;
            CurrentProgress = newProgress;
            if (CurrentProgress.CheckCount(GetRequireFinishCount()))
            {
                Data.IsComplete = true;
            }
        }

        private BigDigit GetRequireFinishCount()
        {
            return Model.GetRequireFinishCount();
        }

        public bool CheckCount()
        {
            return CurrentProgress.CheckCount(GetRequireCount());
        }

        public BigDigit GetRequireCount()
        {
            return Model.GetRequireCount(Data.CurrentStage);
        }

        public void NextStage()
        {
            Data.CurrentStage++;
        }

        public GameReward GetReward()
        {
            var currentStage = Data.CurrentStage;
            if(currentStage >= Model.Stages.Count)
                currentStage = Model.Stages.Count - 1;

            var rewardModel = Model.GetReward(currentStage);
            return new GameReward(rewardModel);
        }

        public void GiveReward()
        {
            var reward = GetReward();
            ClientRewardService.ShowReward(reward);
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
