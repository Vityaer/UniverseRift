using City.Acievements;
using Common.Rewards;
using Models.Achievments;
using Models.Common.BigDigits;
using Models.Data;
using UIController.Rewards;

namespace City.Achievements
{
    public class GameAchievment
    {
        private AchievmentModel _model;
        private AchievmentData _data;

        public int CurrentStage => _data.CurrentStage;
        public int CountStage => _model.Stages.Count;
        public bool IsComplete => _data.IsComplete;
        public BigDigit Progress => _data.Progress;
        public string Id => _model.Id;

        public GameAchievment(AchievmentModel model, AchievmentData data)
        {
            _model = model;
            _data = data;
        }

        public void AddProgress(BigDigit amount)
        {
            if (!IsComplete)
            {
                switch (_model.ProgressType)
                {
                    case ProgressType.StorageAmount:
                        _data.Progress.Add(amount);
                        break;
                    case ProgressType.MaxAmount:
                        if (_data.Progress > amount)
                            _data.Progress = amount;
                        break;
                    case ProgressType.CurrentAmount:
                        _data.Progress = amount;
                        break;
                }

                if (_data.Progress.CheckCount(GetRequireFinishCount()))
                {
                    _data.Progress = new BigDigit(GetRequireFinishCount().Mantissa, GetRequireFinishCount().E10);
                    _data.IsComplete = true;
                }
            }
        }

        public void SetProgress(int newCurrentStage, BigDigit newProgress)
        {
            _data.CurrentStage = newCurrentStage;
            _data.Progress = newProgress;
            if (_data.Progress.CheckCount(GetRequireFinishCount()))
            {
                _data.IsComplete = true;
            }
        }

        private BigDigit GetRequireFinishCount()
        {
            return _model.GetRequireFinishCount();
        }

        public bool CheckCount()
        {
            return _data.Progress.CheckCount(GetRequireCount());
        }

        public BigDigit GetRequireCount()
        {
            return _model.GetRequireCount(_data.CurrentStage);
        }

        public void NextStage()
        {
            _data.CurrentStage++;
        }

        public GameReward GetReward()
        {
            var reward = new GameReward();
            //return _model.GetReward(_data.CurrentStage);
            return reward;
        }

        public void ClearProgress()
        {
            _data.Clear();
        }
    }
}
