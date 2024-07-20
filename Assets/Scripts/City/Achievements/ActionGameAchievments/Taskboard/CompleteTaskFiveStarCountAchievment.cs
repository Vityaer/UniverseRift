using City.Buildings.TaskGiver;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments.Taskboard
{
    public class CompleteTaskFiveStarCountAchievment : GameAchievment
    {
        [Inject] private readonly TaskboardController _taskboardController;

        protected override void Subscribe()
        {
            _taskboardController.OnCompleteTask.Subscribe(CompleteTask).AddTo(Disposables);
        }

        private void CompleteTask(TaskController taskController)
        {
            if(taskController.Model.Rating == 5)
                AddProgress(new BigDigit(1));
        }
    }
}
