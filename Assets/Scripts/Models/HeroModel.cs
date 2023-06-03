using Network.DataServer.Models;
using System;

namespace Models
{
    [Serializable]
    public class HeroModel : BaseModel
    {
        public string HeroId;
        public string Name;
        public int Level;
        public int Rating;
        public CostumeSave Costume = new CostumeSave();

        public HeroModel() { }

        public HeroModel(DataHero dataHero)
        {
            Id = $"{dataHero.Id}";
            HeroId = dataHero.HeroId;
            Level = dataHero.Level;
            Rating = dataHero.Rating;
            Costume = new CostumeSave();
        }

        public HeroModel(InfoHero hero)
        {
            Id = $"{hero.generalInfo.Id}";
            Name = hero.generalInfo.Name;
            HeroId = hero.generalInfo.HeroId;
            Level = hero.generalInfo.Level;
            Rating = hero.generalInfo.RatingHero;
            Costume = new CostumeSave();
        }
    }
}
