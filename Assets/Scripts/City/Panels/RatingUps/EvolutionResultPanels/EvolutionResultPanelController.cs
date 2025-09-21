using Common.Db.CommonDictionaries;
using Hero;
using Models.Heroes.HeroCharacteristics;
using UiExtensions.Scroll.Interfaces;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.Panels.RatingUps.EvolutionResultPanels
{
    public class EvolutionResultPanelController : UiPanelController<EvolutionResultPanelView>
    {
        private readonly CommonDictionaries m_commonDictionaries;
        
        private GameHero m_currentHero;

        public EvolutionResultPanelController(CommonDictionaries commonDictionaries)
        {
            m_commonDictionaries = commonDictionaries;
        }

        public void OpenEvolvedHero(GameHero currentHero)
        {
            m_currentHero = currentHero;
            FillOldInformation();
            m_currentHero.UpRating();
            MessagesPublisher.OpenWindowPublisher.OpenWindow<EvolutionResultPanelController>(openType: OpenType.Exclusive);
            FillNewInformation();
        }

        private void FillOldInformation()
        {
            View.RatingHeroController.ShowRating(m_currentHero.HeroData.Rating);
            View.HealthProgress.OldStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries, TypeCharacteristic.HP):N0}";
            View.DamageProgress.OldStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries,TypeCharacteristic.Damage):N0}";
            View.SpeedProgress.OldStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries,TypeCharacteristic.Initiative):N0}";
        }

        private void FillNewInformation()
        {
            View.RatingHeroController.AddStarWithAnimation();
            View.HealthProgress.NewStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries,TypeCharacteristic.HP):N0}";
            View.DamageProgress.NewStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries,TypeCharacteristic.Damage):N0}";
            View.SpeedProgress.NewStatValue.text = $"{m_currentHero.GetCharacteristic(m_commonDictionaries,TypeCharacteristic.Initiative):N0}";
        }
    }
}
