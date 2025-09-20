using Hero;
using Models;

namespace Fight.Common.WarTable
{
    public class RaceWarLimiter : IWarLimiter
    {
        private readonly string m_race;
        
        public RaceWarLimiter(string race)
        {
            m_race = race;
        }

        public bool Check(GameHero gameHero)
        {
            return gameHero.Model.General.Race.Equals(m_race);
        }
    }
}