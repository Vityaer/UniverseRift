using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Models.Heroes.Skills.Actions.Effects;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public partial class ActionEffect
    {
        private void ExecuteSimpleAction()
        {
            Debug.Log("simple action: " + simpleAction.ToString());
            switch (simpleAction)
            {
                case EffectSimpleAction.Damage:
                    foreach (HeroController heroController in listTarget)
                        heroController.GetDamage(new Strike(amount, heroController.hero.characts.GeneralAttack, typeNumber: typeNumber));
                    break;
                case EffectSimpleAction.Heal:
                    foreach (HeroController heroController in listTarget)
                        heroController.GetHeal((int)Mathf.Floor(amount), typeNumber);
                    break;
                case EffectSimpleAction.HealFromDamage:
                    foreach (HeroController heroController in listTarget)
                        heroController.GetHeal((int)Mathf.Floor(amount), typeNumber);
                    break;
            }
        }

        private void ExecuteBuff()
        {
            switch (effectBuff)
            {
                case EffectBuff.HateGood:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Good, amount);
                    break;
                case EffectBuff.HateBad:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Bad, amount);
                    break;
                case EffectBuff.HateUndead:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Undead, amount);
                    break;
                case EffectBuff.HateElf:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Elf, amount);
                    break;
                case EffectBuff.HatePeople:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.People, amount);
                    break;
                case EffectBuff.HateGods:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.God, amount);
                    break;
                case EffectBuff.HateDarkGods:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.DarkGod, amount);
                    break;
                case EffectBuff.HateBoss:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Boss, amount);
                    break;
            }
        }
        private void ExecuteDebuff()
        {
        }
        private void ExecuteDots()
        {
            switch (effectDots)
            {
                case EffectDots.Poison:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Poison, amount, typeNumber, rounds);
                    break;
                case EffectDots.Bleending:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Bleending, amount, typeNumber, rounds);
                    break;
                case EffectDots.Rot:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Rot, amount, typeNumber, rounds);
                    break;
                case EffectDots.Corrosion:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Corrosion, amount, typeNumber, rounds);
                    break;
                case EffectDots.Combustion:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Combustion, amount, typeNumber, rounds);
                    break;
            }
        }
        private void ExecuteChangeCharacteristic()
        {
            switch (effectChangeCharacteristic)
            {
                case EffectChangeCharacteristic.ChangeMaxHP:
                    foreach (HeroController heroController in listTarget)
                        heroController.ChangeMaxHP((int)Mathf.Floor(amount), typeNumber);
                    break;
                case EffectChangeCharacteristic.ChangeAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangePhysicalAttack((int)Mathf.Floor(amount), typeNumber, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeArmor:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeArmor((int)Mathf.Floor(amount), typeNumber, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeInitiative:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeInitiative((int)Mathf.Floor(amount), typeNumber, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeMagicResistance:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeMagicResistance(amount, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSimpleAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeCountTargetForSimpleAttack((int)Mathf.Floor(amount), rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSpell:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeCountTargetForSpell((int)Mathf.Floor(amount), rounds);
                    break;
                case EffectChangeCharacteristic.ChangeProbabilityCriticalAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeProbabilityCriticalAttack(amount, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeEfficiencyHeal:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeEfficiencyHeal(amount, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeDodge:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeDodge(amount, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeAccuracy:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeAccuracy(amount, rounds);
                    break;
                case EffectChangeCharacteristic.ChangeStamina:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeStamina((int)Mathf.Floor(amount), rounds);
                    break;
            }
        }
        private void ExecuteStatusHero()
        {
            Debug.Log("effectStatus: " + effectStatus.ToString() + " on listTarget.Count:" + listTarget.Count.ToString());
            switch (effectStatus)
            {
                case EffectStatus.Stun:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Stun, (int)amount);
                    break;
                case EffectStatus.Petrification:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Petrification, (int)amount);
                    break;
                case EffectStatus.Freezing:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Freezing, (int)amount);
                    break;
                case EffectStatus.Astral:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Astral, (int)amount);
                    break;
                case EffectStatus.Silence:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Silence, (int)amount);
                    break;
            }
        }
        private void ExecuteMark()
        {
            switch (effectMark)
            {
                case EffectMark.Hellish:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetMark(MarkType.Hellish, amount, rounds);
                    break;
            }
        }
        private void ExecuteSpecial()
        {

        }
        private void ExecuteOther()
        {

        }
    }
}
