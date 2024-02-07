using City.Buildings.Voyage;
using Models.Common.BigDigits;
using VContainer;
using UniRx;

namespace City.Achievements.ActionGameAchievments
{
    public class CompleteTravelAchievment : GameAchievment
    {
        [Inject] private readonly VoyageController _voyageController;

        protected override void Subscribe()
        {
            _voyageController.OnTravelComplete.Subscribe(_ => CompleteTravel()).AddTo(Disposables);
        }

        private void CompleteTravel()
        {
            AddProgress(new BigDigit(1));
        }
    }
}
