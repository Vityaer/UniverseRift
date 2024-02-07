using City.Buildings.Tower;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class TryDeathTowerMissionAchievment : GameAchievment
    {
        [Inject] private readonly ChallengeTowerController _challengeTowerController;

        protected override void Subscribe()
        {
            _challengeTowerController.OnTryMission.Subscribe(_ => AddProgress(new BigDigit(1))).AddTo(Disposables);
        }
    }
}
