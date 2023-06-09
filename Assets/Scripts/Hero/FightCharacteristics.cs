using Models.Heroes.HeroCharacteristics;

namespace Hero
{
    [System.Serializable]
    public class FightCharacteristics : Characteristics
    {
        public int GeneralAttack = 0, GeneralArmor = 0;
        public float MaxHP;
        public FightCharacteristics(Characteristics heroCharacts)
        {
            this.baseCharacteristic = heroCharacts.baseCharacteristic;
            this.limitLevel = heroCharacts.limitLevel;
            this.ProbabilityCriticalAttack = heroCharacts.ProbabilityCriticalAttack;
            this.DamageCriticalAttack = heroCharacts.DamageCriticalAttack;
            this.Accuracy = heroCharacts.Accuracy;
            this.CleanDamage = heroCharacts.CleanDamage;
            this.Dodge = heroCharacts.Dodge;
            this.CountTargetForSimpleAttack = heroCharacts.CountTargetForSimpleAttack;
            this.CountTargetForSpell = heroCharacts.CountTargetForSpell;

        }
    }
}