using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Hero;
using Models.Heroes.Skills.Actions.Effects;
using UnityEngine;

namespace Models.Heroes.Actions
{
    public partial class ActionEffect
    {
        private void ExecuteSimpleAction()
        {
            Debug.Log("simple action: " + SimpleAction.ToString());
            switch (SimpleAction)
            {
                case EffectSimpleAction.Damage:
                    foreach (HeroController heroController in listTarget)
                        heroController.ApplyDamage(new Strike(Amount, heroController.hero.Model.Characteristics.Main.Attack, typeNumber: TypeNumber));
                    break;
                case EffectSimpleAction.Heal:
                    foreach (HeroController heroController in listTarget)
                        heroController.GetHeal((int)Mathf.Floor(Amount), TypeNumber);
                    break;
                case EffectSimpleAction.HealFromDamage:
                    foreach (HeroController heroController in listTarget)
                        heroController.GetHeal((int)Mathf.Floor(Amount), TypeNumber);
                    break;
            }
        }

        private void ExecuteBuff()
        {
            switch (EffectBuff)
            {
                case EffectBuff.HateGood:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Good, Amount);
                    break;
                case EffectBuff.HateBad:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Bad, Amount);
                    break;
                case EffectBuff.HateUndead:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Undead, Amount);
                    break;
                case EffectBuff.HateElf:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Elf, Amount);
                    break;
                case EffectBuff.HatePeople:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.People, Amount);
                    break;
                case EffectBuff.HateGods:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.God, Amount);
                    break;
                case EffectBuff.HateDarkGods:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.DarkGod, Amount);
                    break;
                case EffectBuff.HateBoss:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.SetHate(Attachment.Boss, Amount);
                    break;
            }
        }
        private void ExecuteDebuff()
        {
        }
        private void ExecuteDots()
        {
            switch (EffectDots)
            {
                case EffectDots.Poison:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Poison, Amount, TypeNumber, Rounds);
                    break;
                case EffectDots.Bleending:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Bleending, Amount, TypeNumber, Rounds);
                    break;
                case EffectDots.Rot:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Rot, Amount, TypeNumber, Rounds);
                    break;
                case EffectDots.Corrosion:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Corrosion, Amount, TypeNumber, Rounds);
                    break;
                case EffectDots.Combustion:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDot(DotType.Combustion, Amount, TypeNumber, Rounds);
                    break;
            }
        }
        private void ExecuteChangeCharacteristic()
        {
            switch (EffectChangeCharacteristic)
            {
                case EffectChangeCharacteristic.ChangeMaxHP:
                    foreach (HeroController heroController in listTarget)
                        heroController.ChangeMaxHP((int)Mathf.Floor(Amount), TypeNumber);
                    break;
                case EffectChangeCharacteristic.ChangeAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangePhysicalAttack((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeArmor:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeArmor((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeInitiative:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeInitiative((int)Mathf.Floor(Amount), TypeNumber, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeMagicResistance:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeMagicResistance(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSimpleAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeCountTargetForSimpleAttack((int)Mathf.Floor(Amount), Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeCountTargetForSpell:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeCountTargetForSpell((int)Mathf.Floor(Amount), Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeProbabilityCriticalAttack:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeProbabilityCriticalAttack(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeEfficiencyHeal:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeEfficiencyHeal(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeDodge:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeDodge(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeAccuracy:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeAccuracy(Amount, Rounds);
                    break;
                case EffectChangeCharacteristic.ChangeStamina:
                    foreach (HeroController heroController in listTarget)
                        heroController.hero.ChangeStamina((int)Mathf.Floor(Amount), Rounds);
                    break;
            }
        }
        private void ExecuteStatusHero()
        {
            Debug.Log("effectStatus: " + EffectStatus.ToString() + " on listTarget.Count:" + listTarget.Count.ToString());
            switch (EffectStatus)
            {
                case EffectStatus.Stun:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Stun, (int)Amount);
                    break;
                case EffectStatus.Petrification:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Petrification, (int)Amount);
                    break;
                case EffectStatus.Freezing:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Freezing, (int)Amount);
                    break;
                case EffectStatus.Astral:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Astral, (int)Amount);
                    break;
                case EffectStatus.Silence:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetDebuff(State.Silence, (int)Amount);
                    break;
            }
        }
        private void ExecuteMark()
        {
            switch (EffectMark)
            {
                case EffectMark.Hellish:
                    foreach (HeroController heroController in listTarget)
                        heroController.statusState.SetMark(MarkType.Hellish, Amount, Rounds);
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
