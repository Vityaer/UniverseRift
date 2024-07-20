using City.Buildings.TaskGiver;
using City.TrainCamp;
using Hero;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments.Evolutions
{
    public class EvolutionThreeRatingCountAchievment : GameAchievment
    {
        [Inject] private readonly HeroEvolutionPanelController _heroEvolutionPanelController;

        protected override void Subscribe()
        {
            _heroEvolutionPanelController.OnRatingUp.Subscribe(OnHeroRatingUp).AddTo(Disposables);
        }

        private void OnHeroRatingUp(GameHero gameHero)
        {
            if(gameHero.HeroData.Rating == 3)
                AddProgress(new BigDigit(1));
        }
    }
}
