using Assets.Scripts.Models.Heroes;
using Models.Heroes;
using Network.DataServer.Models;
using System;

namespace Models
{
    [Serializable]
    public class HeroData : BaseModel
    {
        public string HeroId;
        public string Name;
        public int Level;
        public int Rating;
        public CostumeSave Costume = new CostumeSave();

        public HeroData() { }

        public HeroData(DataHero dataHero)
        {
            Id = $"{dataHero.Id}";
            HeroId = dataHero.HeroId;
            Level = dataHero.Level;
            Rating = dataHero.Rating;
            Costume = new CostumeSave();
        }

        public HeroData(HeroModel hero)
        {
            Id = $"{hero.Id}";
            Name = hero.General.Name;
            HeroId = hero.General.HeroId;
            Level = hero.General.Level;
            Rating = hero.General.RatingHero;
            Costume = new CostumeSave();
        }
    }
}
