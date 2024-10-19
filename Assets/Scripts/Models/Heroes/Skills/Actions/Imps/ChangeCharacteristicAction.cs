using Fight.HeroControllers.Generals;
using Models.Heroes.Actions;
using Models.Heroes.Skills.Actions.Effects;
using Models.Heroes.Skills.Actions.Imps.SimpleActions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Models.Heroes.Skills.Actions.Imps
{
    public class ChangeCharacteristicAction : ContinuousAction
    {
        public EffectChangeCharacteristic EffectChangeCharacteristic;

        public override void ExecuteAction()
        {
            switch (EffectChangeCharacteristic)
            {
                case EffectChangeCharacteristic.ChangeMaxHP:
                    foreach (HeroController heroController in ListTarget)
                        heroController.ChangeMaxHP((int)Mathf.Floor(Amount), TypeNumber);
                    break;
                case EffectChangeCharacteristic.ChangeAttack:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangePhysicalAttack((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeArmor:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeArmor((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeInitiative:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeInitiative((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeMagicResistance:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeMagicResistance(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSimpleAttack:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeCountTargetForSimpleAttack((int)Mathf.Floor(Amount), Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSpell:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeCountTargetForSpell((int)Mathf.Floor(Amount), Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeProbabilityCriticalAttack:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeProbabilityCriticalAttack(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeEfficiencyHeal:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeEfficiencyHeal(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeDodge:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeDodge(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeAccuracy:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeAccuracy(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeStamina:
                    foreach (HeroController heroController in ListTarget)
                        heroController.hero.ChangeStamina((int)Mathf.Floor(Amount), Rounds);
                    break;
            }
        }
    }
}
