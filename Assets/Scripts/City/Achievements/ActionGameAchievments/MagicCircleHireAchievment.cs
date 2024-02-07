using City.Buildings.MagicCircle;
using City.Buildings.Tavern;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class MagicCircleHireAchievment : GameAchievment
    {
        [Inject] private readonly MagicCircleController _magicCircleController;

        protected override void Subscribe()
        {
            _magicCircleController.OnRaceHire.Subscribe(x => AddProgress(new BigDigit(x))).AddTo(Disposables);
        }
    }
}
