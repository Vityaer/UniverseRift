using Fight.HeroControllers.Generals;
using System;
using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
    public class GeneralInfoHero
    {
        public string HeroId;
        public string Race;
        public string Vocation;
        public int Rating = 1;
        public Rare Rare = Rare.C;

        public GeneralInfoHero Clone()
        {
            return new GeneralInfoHero()
            {
                HeroId = this.HeroId,
                Race = this.Race,
                Vocation = this.Vocation,
                Rating = this.Rating,
                Rare = this.Rare
            };
        }
    }
}
