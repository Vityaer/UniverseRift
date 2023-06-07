using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.Rounds
{
    [System.Serializable]
    public class Round : ICloneable
    {
        public float amount;
        public RoundTypeNumber typeNumber;

        public void SetData(float amount, RoundTypeNumber typeNumber)
        {
            this.amount = amount;
            this.typeNumber = typeNumber;
        }
        public Round(float amount, RoundTypeNumber typeNumber)
        {
            this.amount = amount;
            this.typeNumber = typeNumber;
        }
        public void Add(float other) { amount += other; }
        public bool AmountEqualsZero() { return Mathf.Abs(amount) < 0.01f; }
        public bool IsPercent { get => typeNumber == RoundTypeNumber.Percent ? true : false; }
        public object Clone()
        {
            return new Round(amount, typeNumber);
        }
    }
}