using Fight.HeroControllers.Generals;
using System;
using UnityEngine;

namespace Models.Heroes
{
    [System.Serializable]
    public class GeneralInfoHero
    {
        public string Name;
        public string HeroId;
        public string Race;
        public string ClassHero;
        public int Rating = 1;
        public string ViewId;
        public string Rarity;
        public string AvatarPath;

        public GeneralInfoHero Clone()
        {
            return new GeneralInfoHero()
            {
                Name = this.Name,
                HeroId = this.HeroId,
                Race = this.Race,
                ClassHero = this.ClassHero,
                Rating = this.Rating,
                ViewId = this.ViewId,
                Rarity = this.Rarity,
                AvatarPath = this.AvatarPath
            };
        }
    }
}
