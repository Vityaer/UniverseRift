using Hero;
using Models.Heroes.HeroCharacteristics;
using UiExtensions.Scroll.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.RatingUps.EvolutionResultPanels
{
    public class EvolutionResultPanelController : UiPanelController<EvolutionResultPanelView>
    {
        private GameHero _currentHero;

        public void OpenEvolvedHero(GameHero currentHero)
        {
            _currentHero = currentHero;
            FillOldInformation();
            _currentHero.UpRating();
            MessagesPublisher.OpenWindowPublisher.OpenWindow<EvolutionResultPanelController>(openType: OpenType.Exclusive);
            FillNewInformation();
        }

        private void FillOldInformation()
        {
            View.RatingHeroController.ShowRating(_currentHero.HeroData.Rating);
            View.HealthProgress.OldStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.HP):N0}";
            View.DamageProgress.OldStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.Damage):N0}";
            View.SpeedProgress.OldStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.Initiative):N0}";
        }

        private void FillNewInformation()
        {
            View.RatingHeroController.AddStarWithAnimation();
            View.HealthProgress.NewStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.HP):N0}";
            View.DamageProgress.NewStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.Damage):N0}";
            View.SpeedProgress.NewStatValue.text = $"{_currentHero.GetCharacteristic(TypeCharacteristic.Initiative):N0}";
        }
    }
}
