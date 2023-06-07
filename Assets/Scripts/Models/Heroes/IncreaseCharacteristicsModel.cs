using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
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
                increaseDamage = this.increaseDamage,
                increaseHP = this.increaseHP,
                increaseInitiative = this.increaseInitiative,
                increaseProbabilityCriticalAttack = this.increaseProbabilityCriticalAttack,
                increaseDamageCriticalAttack = this.increaseDamageCriticalAttack,
                increaseDodge = this.increaseDodge,
                increaseAccuracy = this.increaseAccuracy,
                increaseCleanDamage = this.increaseCleanDamage,
                increaseMagicResistance = this.increaseMagicResistance,
                increaseCritResistance = this.increaseCritResistance,
                increasePoisonResistance = this.increasePoisonResistance
            };
        }
    }
}
