using City.Buildings.LongTravels;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class LongTravelTryCountAchievment : GameAchievment
    {
        [Inject] private readonly LongTravelController _longTravelController;

        protected override void Subscribe()
        {
            _longTravelController.OnTryFight.Subscribe(_ => AddProgress(new BigDigit(1))).AddTo(Disposables);
        }
    }
}
