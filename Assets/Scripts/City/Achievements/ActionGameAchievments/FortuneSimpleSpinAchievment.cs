using City.Buildings.WheelFortune;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class FortuneSimpleSpinAchievment : GameAchievment
    {
        [Inject] private readonly FortuneWheelController _fortuneWheelController;

        protected override void Subscribe()
        {
            _fortuneWheelController.OnSimpleRotate.Subscribe(AddProgress).AddTo(Disposables);
        }
    }
}
