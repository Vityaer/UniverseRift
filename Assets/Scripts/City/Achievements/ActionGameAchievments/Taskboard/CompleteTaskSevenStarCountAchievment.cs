using City.Buildings.TaskGiver;
using Models.Common.BigDigits;
using VContainer;
using UniRx;

namespace City.Achievements.ActionGameAchievments.Taskboard
{
    public class CompleteTaskSevenStarCountAchievment : GameAchievment
    {
        [Inject] private readonly TaskboardController _taskboardController;

        protected override void Subscribe()
        {
            _taskboardController.OnCompleteTask.Subscribe(CompleteTask).AddTo(Disposables);
        }

        private void CompleteTask(BaseTaskController taskController)
        {
            if (taskController.Model.Rating == 7)
                AddProgress(new BigDigit(1));
        }
    }
}
