using System;
using UnityEngine;

namespace Models.Heroes.HeroCharacteristics
{
    [Serializable]
    public class IncreaseCharacteristicsModel : ICloneable
    {
        public float increaseDamage;
        public float increaseHP;
        public float increaseInitiative;
        public float increaseProbabilityCriticalAttack;
        public float increaseDamageCriticalAttack;
        public float increaseDodge;
        public float increaseAccuracy;
        public float increaseCleanDamage;

        [Header("Increace resistance")]
        public float increaseMagicResistance;
        public float increaseCritResistance;
        public float increasePoisonResistance;

        public object Clone()
        {
            return new IncreaseCharacteristicsModel
            {
                increaseDamage = increaseDamage,
                increaseHP = increaseHP,
                increaseInitiative = increaseInitiative,
                increaseProbabilityCriticalAttack = increaseProbabilityCriticalAttack,
                increaseDamageCriticalAttack = increaseDamageCriticalAttack,
                increaseDodge = increaseDodge,
                increaseAccuracy = increaseAccuracy,
                increaseCleanDamage = increaseCleanDamage,
                increaseMagicResistance = increaseMagicResistance,
                increaseCritResistance = increaseCritResistance,
                increasePoisonResistance = increasePoisonResistance
            };
        }
    }
}
