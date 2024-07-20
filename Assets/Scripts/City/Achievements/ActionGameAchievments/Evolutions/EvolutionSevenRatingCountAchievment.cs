using City.TrainCamp;
using Hero;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments.Evolutions
{
    public class EvolutionSevenRatingCountAchievment : GameAchievment
    {
        [Inject] private readonly HeroEvolutionPanelController _heroEvolutionPanelController;

        protected override void Subscribe()
        {
            _heroEvolutionPanelController.OnRatingUp.Subscribe(OnHeroRatingUp).AddTo(Disposables);
        }

        private void OnHeroRatingUp(GameHero gameHero)
        {
            if (gameHero.HeroData.Rating == 7)
                AddProgress(new BigDigit(1));
        }
    }
}
