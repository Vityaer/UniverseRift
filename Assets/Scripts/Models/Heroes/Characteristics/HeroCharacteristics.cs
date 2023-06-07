using Models.Heroes.Characteristics;

namespace Models.Heroes
{
    [System.Serializable]
    public class HeroCharacteristics
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
        public BaseCharacteristicModel baseCharacteristic;

        public HeroCharacteristics Clone()
        {
            return new HeroCharacteristics
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
                CountTargetForSimpleAttack = this.CountTargetForSimpleAttack,
                CountTargetForSpell = this.CountTargetForSpell,
                baseCharacteristic = this.baseCharacteristic
            };
        }
    }
}
