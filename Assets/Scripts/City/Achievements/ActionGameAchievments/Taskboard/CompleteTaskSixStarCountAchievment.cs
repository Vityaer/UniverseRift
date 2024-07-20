using City.Buildings.TaskGiver;
using Models.Common.BigDigits;
using VContainer;
using UniRx;

namespace City.Achievements.ActionGameAchievments.Taskboard
{
    public class CompleteTaskSixStarCountAchievment : GameAchievment
    {
        [Inject] private readonly TaskboardController _taskboardController;

        protected override void Subscribe()
        {
            _taskboardController.OnCompleteTask.Subscribe(CompleteTask).AddTo(Disposables);
        }

        private void CompleteTask(TaskController taskController)
        {
            if (taskController.Model.Rating == 6)
                AddProgress(new BigDigit(1));
        }
    }
}
