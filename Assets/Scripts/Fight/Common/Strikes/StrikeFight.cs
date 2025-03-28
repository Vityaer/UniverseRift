﻿using Fight.Rounds;
using UnityEngine;

namespace Fight.Common.Strikes
{
    public class Strike
    {
        public TypeStrike type = TypeStrike.Physical;
        public float bonusNum = 0f;
        public float bonusPercent = 0f;
        public RoundTypeNumber typeNumber;
        public float baseAttack;
        public int skillAttack;
        public bool isMellee = true;

        public void AddBonus(float bonus, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
        {
            if (typeNumber == RoundTypeNumber.Num)
            {
                bonusNum += bonus;
            }
            else
            {
                bonusPercent += bonus;
            }
        }

        public float GetDamage(int skillDefense = 0)
        {
            float result = 0f;
            result = baseAttack * (1 + bonusPercent / 100f);
            result += bonusNum;
            if (type == TypeStrike.Physical)
            {
                int skillFactor = skillAttack - skillDefense;
                if (skillFactor >= -19)
                    result *= 1f + skillFactor * 0.05f;
            }
            if (result < 0) Debug.Log("negative damage");
            return Mathf.Ceil(result);
        }

        public Strike(
            float baseAttack,
            int skillAttack,
            RoundTypeNumber typeNumber = RoundTypeNumber.Num,
            TypeStrike typeStrike = TypeStrike.Physical,
            bool isMellee = true
            )
        {
            this.baseAttack = baseAttack;
            this.typeNumber = typeNumber;
            type = typeStrike;
            this.skillAttack = skillAttack;
            this.isMellee = isMellee;
        }

        public override string ToString()
        {
            return GetDamage().ToString();
        }
    }
}