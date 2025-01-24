using Hero;
using UiExtensions.Scroll.Interfaces;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace City.TrainCamp.HeroPanels.HeroDetails
{
    public class HeroDetailsPanelController : UiPanelController<HeroDetailsPanelView>
    {
        public void SetData(GameHero hero)
        {
            var characteristics = hero.Model.Characteristics;
            SetStat(View.ProbabilityCriticalAttackText, characteristics.ProbabilityCriticalAttack.ToString("N2"));
            SetStat(View.DamageCriticalAttackText, characteristics.DamageCriticalAttack.ToString("N2"));
            SetStat(View.AccuracyText, characteristics.Accuracy.ToString("N2"));
            SetStat(View.CleanDamageText, characteristics.CleanDamage.ToString("N2"));
            SetStat(View.DodgeText, characteristics.Dodge.ToString("N2"));
            SetStat(View.CanRetaliationText, characteristics.Main.CanRetaliation.ToString());
            SetStat(View.MelleeText, characteristics.Main.Mellee.ToString());
        }

        private void SetStat(LocalizeStringEvent localizeStringEvent, string text)
        {
            if (localizeStringEvent.StringReference.TryGetValue("Stat", out var variable))
            {
                var stringVariable = variable as StringVariable;
                stringVariable.Value = text;
            }
        }
    }
}