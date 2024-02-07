using Campaign;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class AutoFightHoursAchievment : GameAchievment
    {
        [Inject] private readonly GoldHeapController _goldHeapController;

        protected override void Subscribe()
        {
            _goldHeapController.OnRewardGetHour.Subscribe(AddProgress).AddTo(Disposables);
        }
    }
}
