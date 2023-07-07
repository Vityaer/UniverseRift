using Fight.Common.Strikes;
using System;

namespace Models.Heroes.HeroCharacteristics
{
    [System.Serializable]
    public class BaseCharacteristicModel
    {
        public int Attack;
        public int Defense;
        public int Speed;
        public TypeMovement MovementType;
        public bool Mellee;
        public TypeStrike AttackType;
        public bool CanRetaliation = true;
        public int CountCouterAttack = 1;

        public BaseCharacteristicModel Clone()
        {
            return new BaseCharacteristicModel()
            {
                Attack = this.Attack,
                Defense = this.Defense,
                Speed = this.Speed,
                MovementType = this.MovementType,
                Mellee = this.Mellee,
                AttackType = this.AttackType,
                CanRetaliation= this.CanRetaliation,
                CountCouterAttack= this.CountCouterAttack
            };
        }
    }
}
