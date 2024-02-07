using City.TrainCamp;
using Models.Heroes;
using UIController.ItemVisual;
using UnityEngine;

namespace Models.City.TrainCamp
{
    [System.Serializable]
    public class RequirementHeroModel
    {
        public int Rating;
        public int Count;
        public RequireRaceType RequireRace;
        public string Race;
        public HeroModel DataHero;
        public string IconPath;
    }
}
