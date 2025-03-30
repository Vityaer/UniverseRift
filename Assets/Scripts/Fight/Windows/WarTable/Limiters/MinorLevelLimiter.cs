using Hero;

namespace Fight.WarTable
{
    public class MinorLevelLimiter : IWarLimiter
    {
        private readonly int m_level;
        
        public MinorLevelLimiter(int level)
        {
            m_level = level;
        }

        public bool Check(GameHero gameHero)
        {
            return gameHero.HeroData.Level >= m_level;
        }
    }
}