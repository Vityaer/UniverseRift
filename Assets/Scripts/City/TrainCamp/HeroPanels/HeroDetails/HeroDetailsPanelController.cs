using Hero;
using Models.Heroes;
using TMPro;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;

namespace City.TrainCamp.HeroPanels.HeroDetails
{
    public class HeroDetailsPanelController : UiPanelController<HeroDetailsPanelView>
    {
        [SerializeField] private TextMeshProUGUI _probabilityCriticalAttackText;
        [SerializeField] private TextMeshProUGUI _damageCriticalAttackText;
        [SerializeField] private TextMeshProUGUI _accuracyText;
        [SerializeField] private TextMeshProUGUI _cleanDamageText;
        [SerializeField] private TextMeshProUGUI _dodgeText;
        [SerializeField] private TextMeshProUGUI _magicResistanceText;
        [SerializeField] private TextMeshProUGUI _critResistanceText;
        [SerializeField] private TextMeshProUGUI _poisonResistanceText;
        [SerializeField] private TextMeshProUGUI _petrificationResistanceText;
        [SerializeField] private TextMeshProUGUI _freezingResistanceText;
        [SerializeField] private TextMeshProUGUI _astralResistanceText;
        [SerializeField] private TextMeshProUGUI _dumbResistanceText;
        [SerializeField] private TextMeshProUGUI _silinceResistanceText;
        [SerializeField] private TextMeshProUGUI _stunResistanceText;
        [SerializeField] private TextMeshProUGUI _efficiencyHealText;
        [SerializeField] private TextMeshProUGUI _canRetaliationText;
        [SerializeField] private TextMeshProUGUI _countCouterAttackText;
        [SerializeField] private TextMeshProUGUI _melleeText;

        public void SetData(GameHero hero)
        {
        }
    }
}