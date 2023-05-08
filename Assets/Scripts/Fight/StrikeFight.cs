using UnityEngine;

public class Strike
{
    public TypeStrike type = TypeStrike.Physical;
    public float bonusNum = 0f;
    public float bonusPercent = 0f;
    public TypeNumber typeNumber;
    public float baseAttack;
    public int skillAttack;
    public bool isMellee = true;

    public void AddBonus(float bonus, TypeNumber typeNumber = TypeNumber.Num)
    {
        if (typeNumber == TypeNumber.Num)
        {
            this.bonusNum += bonus;
        }
        else
        {
            this.bonusPercent += bonus;
        }
    }

    public float GetDamage(int skillDefense = 0)
    {
        float result = 0f;
        result = (baseAttack * (1 + bonusPercent / 100f));
        result += bonusNum;
        if (type == TypeStrike.Physical)
        {
            int skillFactor = skillAttack - skillDefense;
            if (skillFactor >= -19)
                result *= 1f + (skillFactor) * 0.05f;
        }
        if (result < 0) Debug.Log("negative damage");
        return Mathf.Ceil(result);
    }

    public Strike(float baseAttack, int skillAttack, TypeNumber typeNumber = TypeNumber.Num, TypeStrike typeStrike = TypeStrike.Physical, bool isMellee = true)
    {
        this.baseAttack = baseAttack;
        this.typeNumber = typeNumber;
        this.type = typeStrike;
        this.skillAttack = skillAttack;
        this.isMellee = isMellee;
    }

    public override string ToString()
    {
        return GetDamage().ToString();
    }
}

public enum TypeStrike
{
    Physical,
    Critical,
    Magical,
    Clean,
    Electrical,
    Fiery,
    Hew,
    Plosive,
    Elemental,
    Holy,
    Dark,
    Poison
}