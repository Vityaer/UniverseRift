using Models.Heroes.HeroCharacteristics.Abstractions;
using System;
using System.Collections.Generic;

namespace Models.Heroes.HeroCharacteristics
{
    [System.Serializable]
    public class Characteristics
    {
        public int limitLevel;
        public float Damage;
        public float HP;
        public float Initiative;
        public float ProbabilityCriticalAttack;
        public float DamageCriticalAttack;
        public float Accuracy;
        public float CleanDamage;
        public float Dodge;
        public int CountTargetForSimpleAttack = 1;
        public int CountTargetForSpell = 1;
        public BaseCharacteristicModel Main = new();

        public Characteristics Clone()
        {
            return new Characteristics()
            {
                limitLevel = this.limitLevel,
                Damage = this.Damage,
                HP = this.HP,
                Initiative = this.Initiative,
                ProbabilityCriticalAttack = this.ProbabilityCriticalAttack,
                DamageCriticalAttack = this.DamageCriticalAttack,
                Accuracy = this.Accuracy,
                CleanDamage = this.CleanDamage,
                Dodge = this.Dodge,
                CountTargetForSimpleAttack = this.CountTargetForSpell,
                CountTargetForSpell = this.CountTargetForSpell,
                Main = this.Main.Clone()
            };
        }
    }
}
