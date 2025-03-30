using Hero;

namespace Fight.WarTable
{
    public interface IWarLimiter
    {
        public bool Check(GameHero gameHero);
    }
}