using Hero;

namespace Fight.Common.WarTable
{
    public interface IWarLimiter
    {
        public bool Check(GameHero gameHero);
    }
}