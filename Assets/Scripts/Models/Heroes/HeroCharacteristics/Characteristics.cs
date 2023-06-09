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
        public BaseCharacteristicModel baseCharacteristic;

        public Characteristics Clone()
        {
            return new Characteristics
            {
                limitLevel = limitLevel,
                Damage = Damage,
                HP = HP,
                Initiative = Initiative,
                ProbabilityCriticalAttack = ProbabilityCriticalAttack,
                DamageCriticalAttack = DamageCriticalAttack,
                Accuracy = Accuracy,
                CleanDamage = CleanDamage,
                Dodge = Dodge,
                CountTargetForSimpleAttack = CountTargetForSimpleAttack,
                CountTargetForSpell = CountTargetForSpell,
                baseCharacteristic = baseCharacteristic
            };
        }
    }
}
