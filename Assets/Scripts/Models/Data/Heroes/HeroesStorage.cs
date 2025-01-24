using System.Collections.Generic;

namespace Models.Data.Heroes
{
    public class HeroesStorage : BaseDataModel
    {
        public List<HeroData> ListHeroes = new();
        public int MaxCountHeroes;
    }
}
