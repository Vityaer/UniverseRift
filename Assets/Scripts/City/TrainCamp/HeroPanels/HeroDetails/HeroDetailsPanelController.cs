using Hero;
using Models.Heroes;
using TMPro;
using UiExtensions.Scroll.Interfaces;
using UnityEngine;

namespace City.TrainCamp.HeroPanels.HeroDetails
{
    public class HeroDetailsPanelController : UiPanelController<HeroDetailsPanelView>
    {
        public void SetData(GameHero hero)
        {
            View.ProbabilityCriticalAttackText.text = $"{hero.Model.Characteristics.ProbabilityCriticalAttack}:N2";
            View.DamageCriticalAttackText.text = $"{hero.Model.Characteristics.DamageCriticalAttack}:N2";
            View.AccuracyText.text = $"{hero.Model.Characteristics.Accuracy}:N2";
            View.CleanDamageText.text = $"{hero.Model.Characteristics.CleanDamage}:N2";
            View.DodgeText.text = $"{hero.Model.Characteristics.Dodge}:N2";
            View.CanRetaliationText.text = $"{hero.Model.Characteristics.Main.CanRetaliation}";
            View.MelleeText.text = $"{hero.Model.Characteristics.Main.Mellee}";
        }
    }
}