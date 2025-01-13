using Hero;
using System.Collections.Generic;

namespace City.Panels.SelectHeroes
{
    public class GameHeroLevelComparer : IComparer<GameHero>
    {
        public int Compare(GameHero x, GameHero y)
        {
            if (x.HeroData.Level > y.HeroData.Level)
            {
                return -1;
            }
            else if (x.HeroData.Level < y.HeroData.Level)
            {
                return 1;
            }
            else
            {
                if (x.HeroData.Rating > y.HeroData.Rating)
                {
                    return -1;
                }
                else if (x.HeroData.Rating < y.HeroData.Rating)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
