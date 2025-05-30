﻿using Fight.Common.Strikes;
using Fight.HeroControllers.Generals;
using Fight.HeroStates;
using Fight.Rounds;
using Models;
using Models.Heroes;
using Models.Heroes.HeroCharacteristics;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Hero
{
    [Serializable]
    public class GameHeroFight
    {
        public readonly HeroModel Model;
        public readonly HeroData HeroData;

        public HeroStatus StatusState;

        [field: SerializeField] public float Health { get; private set; }

        public float MaxHealth => Model.Characteristics.HP;
        public BaseCharacteristicModel GetBaseCharacteristic => Model.Characteristics.Main;

        private ReactiveCommand<float> _onGetDamage = new();
        
        public IObservable<float> OnGetDamage => _onGetDamage;
        
        public GameHeroFight(GameHero gameHero, HeroStatus statusState)
        {
            Model = gameHero.Model.Clone();
            HeroData = gameHero.HeroData;
            StatusState = statusState;
            Health = MaxHealth;
            StatusState.SetMaxHealth(MaxHealth);
        }

        public void ChangeHealth(float newMaxHealth, float newCurrentHealth)
        {
            Health = newCurrentHealth;
            StatusState.ChangeHP(Health);
            StatusState.SetMaxHealth(newMaxHealth);
        }

        public void GetDamage(Strike strike)
        {
            float calcDamage = CalculateDamage(strike);

            if (calcDamage < 0)
                return;

            _onGetDamage.Execute(Health > calcDamage ? calcDamage : Health);
            
            Health = (Health > calcDamage) ? Health - (int)calcDamage : 0;
            StatusState.ChangeHP(Health);
            StatusState.ChangeStamina(10);
        }

        public void GetHeal(float heal, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            //heal = (int)Mathf.Floor(heal * resistances.EfficiencyHeal);
            //float calcHeal = heal;
            //if (typeNumber == RoundTypeNumber.Percent)
            //{
            //    calcHeal = (int)Mathf.Floor(MaxHP * (heal / 100f));
            //}
            //characts.HP = characts.HP + (int)calcHeal;
            //if (characts.HP > MaxHP) characts.HP = MaxHP;
        }

        public void ChangeMaxHP(float amount, RoundTypeNumber typeNumber, List<Round> rounds = null)
        {
            //if (typeNumber == RoundTypeNumber.Num)
            //{
            //    if (characts.HP == MaxHP) characts.HP += (int)amount;
            //    MaxHP += (int)amount;
            //}
            //else
            //{
            //    if (characts.HP == MaxHP) characts.HP = (int)Mathf.Floor(characts.HP * (1 + amount / 100f));
            //    MaxHP = (int)Mathf.Floor(MaxHP * (1 + amount / 100f));
            //}
        }

        public void ChangePhysicalAttack(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
        {
            //if (typeNumber == RoundTypeNumber.Num)
            //{
            //    characts.Damage += amount;
            //}
            //else
            //{
            //    characts.Damage = (int)Mathf.Floor(characts.Damage * (1 + amount / 100f));
            //}
        }

        public void ChangeInitiative(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
        {
            //if (typeNumber == RoundTypeNumber.Num)
            //{
            //    characts.Initiative += amount;
            //}
            //else
            //{
            //    characts.Initiative = (int)Mathf.Floor(characts.Initiative * (1 + amount / 100f));
            //}
        }

        public void ChangeArmor(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
        {
            //if (typeNumber == RoundTypeNumber.Num)
            //{
            //    characts.GeneralArmor += amount;
            //}
            //else
            //{
            //    characts.GeneralArmor = (int)Mathf.Floor(characts.GeneralArmor * (1 + amount / 100f));
            //}
        }
        public void ChangeProbabilityCriticalAttack(float amount, List<Round> rounds)
        {
            //characts.ProbabilityCriticalAttack += amount / 100f;
        }
        public void ChangeDamageCriticalAttack(float amount, List<Round> rounds)
        {
            //characts.DamageCriticalAttack += amount / 100f;
        }

        public void ChangeAccuracy(float amount, List<Round> rounds)
        {
            //characts.Accuracy += amount / 100f;
        }

        public void ChangeDodge(float amount, List<Round> rounds)
        {
            //characts.Dodge += amount / 100f;
        }

        public void ChangeCleanDamage(float amount, List<Round> rounds)
        {
            //characts.CleanDamage += amount / 100f;
        }

        public void ChangeMagicResistance(float amount, List<Round> rounds)
        {
            //resistances.MagicResistance += amount / 100f;
        }

        public void ChangeCritResistance(float amount, List<Round> rounds)
        {
            //resistances.CritResistance += amount / 100f;
        }

        public void ChangePoisonResistance(float amount, List<Round> rounds)
        {
            //resistances.PoisonResistance += amount / 100f;
        }

        public void ChangeEfficiencyHeal(float amount, List<Round> rounds)
        {
            //resistances.EfficiencyHeal += amount / 100f;
        }

        public void ChangeCountTargetForSimpleAttack(int amount, List<Round> rounds)
        {
            //characts.CountTargetForSimpleAttack += amount;
        }

        public void ChangeStamina(int amount, List<Round> rounds)
        {
        }

        public void ChangeCountTargetForSpell(int amount, List<Round> rounds)
        {
            //characts.CountTargetForSpell += amount;
        }

        public void SetHate(Attachment race, float amount)
        {

        }

        //Core
        private float CalculateDamage(Strike strike)
        {
            float result = 0;
            switch (strike.type)
            {
                case TypeStrike.Physical:
                    int allArmor = Model.Characteristics.Main.Defense;
                    allArmor += StatusState.GetAllBuffArmor();
                    result = strike.GetDamage(allArmor);
                    break;
                case TypeStrike.Critical:
                    result = strike.GetDamage();
                    //result *= (1 - resistances.CritResistance);
                    break;
                case TypeStrike.Magical:
                    result = strike.GetDamage();
                    //result *= (1 - resistances.MagicResistance);
                    break;
                case TypeStrike.Poison:
                    result = strike.GetDamage();
                    //result *= (1 - resistances.PoisonResistance);
                    break;

            }
            result = Mathf.Floor(result);
            return result;
        }

        public void PrepareSkills(HeroController master)
        {
            //foreach (Skill skill in skills)
            //{
            //    skill.CreateSkill(master, currentBreakthrough);
            //}
        }
    }
}
