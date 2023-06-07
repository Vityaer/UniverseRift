using Fight.Common.Strikes;

namespace Models.Heroes.Characteristics
{
    [System.Serializable]
    public class BaseCharacteristicModel
    {
        public int Attack;
        public int Defense;
        public int Speed;
        public TypeMovement typeMovement;
        public bool Mellee;
        public TypeStrike typeStrike;
        public bool CanRetaliation = true;
        public int CountCouterAttack = 1;

        public BaseCharacteristicModel Clone()
        {
            return new BaseCharacteristicModel
            {
                Attack = Attack,
                Defense = Defense,
                Speed = Speed,
                typeMovement = typeMovement,
                Mellee = Mellee,
                typeStrike = typeStrike,
                CanRetaliation = CanRetaliation,
                CountCouterAttack = CountCouterAttack
            };
        }
    }
}
