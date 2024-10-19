using System;
using UnityEngine;

namespace Fight.Rounds
{
    [Serializable]
    public class Round
    {
        public float Amount;
        public RoundTypeNumber TypeNumber;

        public bool IsPercent { get => TypeNumber == RoundTypeNumber.Percent ? true : false; }

        public void SetData(float amount, RoundTypeNumber typeNumber)
        {
            this.Amount = amount;
            this.TypeNumber = typeNumber;
        }

        public Round()
        {
            Amount = 0;
            TypeNumber = RoundTypeNumber.Percent;
        }

        public Round(float amount, RoundTypeNumber typeNumber)
        {
            this.Amount = amount;
            this.TypeNumber = typeNumber;
        }

        public void Add(float other)
        {
            Amount += other;
        }

        public bool AmountEqualsZero()
        {
            return Mathf.Abs(Amount) < 0.01f;
        }

        public object Clone()
        {
            return new Round(Amount, TypeNumber);
        }
    }
}