using System.Collections.Generic;
using Hero;

namespace Fight.WarTable
{
    public class WarTableLimiter
    {
        public List<IWarLimiter> WarLimiters = new List<IWarLimiter>();

        public WarTableLimiter(List<IWarLimiter> warLimiters)
        {
            WarLimiters = warLimiters;
        }

        public bool CheckHero(GameHero hero)
        {
            return WarLimiters.TrueForAll(limiter => limiter.Check(hero));
        }
    }
}