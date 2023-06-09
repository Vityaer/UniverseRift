namespace Models.Heroes.Actions
{
    public enum TypeEvent
    {
        OnStartFight,
        OnStrike,
        OnTakingDamage,
        OnHPLess50,
        OnHPLess30,
        OnHeal,
        OnSpell,
        OnEndRound,
        OnDeathHero,
        OnDeathFriend,
        OnDeathEnemy
    }
}