using City.Buildings.TaskGiver;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class CompleteTaskCountAchievment : GameAchievment
    {
        [Inject] private readonly TaskboardController _taskboardController;

        protected override void Subscribe()
        {
            _taskboardController.OnCompleteTask.Subscribe(_ => CompleteTask()).AddTo(Disposables);
        }

        private void CompleteTask()
        {
            AddProgress(new BigDigit(1));
        }
    }
}
