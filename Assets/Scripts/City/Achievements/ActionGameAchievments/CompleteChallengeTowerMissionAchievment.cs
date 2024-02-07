using City.Buildings.Tower;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class CompleteChallengeTowerMissionAchievment : GameAchievment
    {
        [Inject] private readonly ChallengeTowerController _challengeTowerController;

        protected override void Subscribe()
        {
            _challengeTowerController.OnCompleteMission.Subscribe(CompleteMission).AddTo(Disposables);
        }

        private void CompleteMission(int index)
        {
            AddProgress(new BigDigit(index));
        }
    }
}
