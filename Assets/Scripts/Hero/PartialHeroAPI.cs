using Fight.Common.Strikes;
using Fight.Rounds;
using System.Collections.Generic;
using UnityEngine;

public partial class GameHero
{
    public HeroStateAPI statusState;
    public void GetDamage(Strike strike)
    {
        float calcDamage = CalculateDamage(strike);
        characts.HP = (characts.HP > calcDamage) ? characts.HP - (int)calcDamage : 0;
    }

    public void GetHeal(float heal, RoundTypeNumber typeNumber = RoundTypeNumber.Num)
    {
        heal = (int)Mathf.Floor(heal * resistances.EfficiencyHeal);
        float calcHeal = heal;
        if (typeNumber == RoundTypeNumber.Percent)
        {
            calcHeal = (int)Mathf.Floor(MaxHP * (heal / 100f));
        }
        characts.HP = characts.HP + (int)calcHeal;
        if (characts.HP > MaxHP) characts.HP = MaxHP;
    }

    public void ChangeMaxHP(float amount, RoundTypeNumber typeNumber, List<Round> rounds = null)
    {
        if (typeNumber == RoundTypeNumber.Num)
        {
            if (characts.HP == MaxHP) characts.HP += (int)amount;
            MaxHP += (int)amount;
        }
        else
        {
            if (characts.HP == MaxHP) characts.HP = (int)Mathf.Floor(characts.HP * (1 + amount / 100f));
            MaxHP = (int)Mathf.Floor(MaxHP * (1 + amount / 100f));
        }
    }

    public void ChangePhysicalAttack(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
    {
        if (typeNumber == RoundTypeNumber.Num)
        {
            characts.Damage += amount;
        }
        else
        {
            characts.Damage = (int)Mathf.Floor(characts.Damage * (1 + amount / 100f));
        }
    }

    public void ChangeInitiative(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
    {
        if (typeNumber == RoundTypeNumber.Num)
        {
            characts.Initiative += amount;
        }
        else
        {
            characts.Initiative = (int)Mathf.Floor(characts.Initiative * (1 + amount / 100f));
        }
    }

    public void ChangeArmor(int amount, RoundTypeNumber typeNumber, List<Round> rounds)
    {
        if (typeNumber == RoundTypeNumber.Num)
        {
            characts.GeneralArmor += amount;
        }
        else
        {
            characts.GeneralArmor = (int)Mathf.Floor(characts.GeneralArmor * (1 + amount / 100f));
        }
    }
    public void ChangeProbabilityCriticalAttack(float amount, List<Round> rounds)
    {
        characts.ProbabilityCriticalAttack += amount / 100f;
    }
    public void ChangeDamageCriticalAttack(float amount, List<Round> rounds)
    {
        characts.DamageCriticalAttack += amount / 100f;
    }

    public void ChangeAccuracy(float amount, List<Round> rounds)
    {
        characts.Accuracy += amount / 100f;
    }

    public void ChangeDodge(float amount, List<Round> rounds)
    {
        characts.Dodge += amount / 100f;
    }

    public void ChangeCleanDamage(float amount, List<Round> rounds)
    {
        characts.CleanDamage += amount / 100f;
    }

    public void ChangeMagicResistance(float amount, List<Round> rounds)
    {
        resistances.MagicResistance += amount / 100f;
    }

    public void ChangeCritResistance(float amount, List<Round> rounds)
    {
        resistances.CritResistance += amount / 100f;
    }

    public void ChangePoisonResistance(float amount, List<Round> rounds)
    {
        resistances.PoisonResistance += amount / 100f;
    }

    public void ChangeEfficiencyHeal(float amount, List<Round> rounds)
    {
        resistances.EfficiencyHeal += amount / 100f;
    }

    public void ChangeCountTargetForSimpleAttack(int amount, List<Round> rounds)
    {
        characts.CountTargetForSimpleAttack += amount;
    }

    public void ChangeStamina(int amount, List<Round> rounds)
    {
    }

    public void ChangeCountTargetForSpell(int amount, List<Round> rounds)
    {
        characts.CountTargetForSpell += amount;
    }

    public void SetHate(Attachment race, float amount)
    {

    }

    //Core
    private float CalculateDamage(Strike strike)
    {
        float result = 0;
        switch (strike.type)
        {
            case TypeStrike.Physical:
                int allArmor = characts.GeneralArmor;
                allArmor += statusState.GetAllBuffArmor();
                result = strike.GetDamage(allArmor);
                break;
            case TypeStrike.Critical:
                result = strike.GetDamage();
                result *= (1 - resistances.CritResistance);
                break;
            case TypeStrike.Magical:
                result = strike.GetDamage();
                result *= (1 - resistances.MagicResistance);
                break;
            case TypeStrike.Poison:
                result = strike.GetDamage();
                result *= (1 - resistances.PoisonResistance);
                break;

        }
        result = Mathf.Floor(result);
        return result;
    }
}
