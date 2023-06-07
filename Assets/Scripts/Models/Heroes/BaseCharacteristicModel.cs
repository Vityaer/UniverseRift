namespace Models.Heroes
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
                Attack = this.Attack,
                Defense = this.Defense,
                Speed = this.Speed,
                typeMovement = this.typeMovement,
                Mellee = this.Mellee,
                typeStrike = this.typeStrike,
                CanRetaliation = this.CanRetaliation,
                CountCouterAttack = this.CountCouterAttack
            };
        }
    }
}
