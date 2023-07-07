using Common.Resourses;
using Models.Heroes;
using System.Collections.Generic;
using UnityEngine;

namespace Models.City.TrainCamp
{
    [System.Serializable]
    public class LevelUpRaitingModel : BaseModel
    {
        public int Level;
        public List<GameResource> Cost = new List<GameResource>();
        public List<RequirementHeroModel> RequirementHeroes = new List<RequirementHeroModel>();



        public void UpdateData(HeroModel hero)
        {
            RequirementHeroes.ForEach(x => x.UpdateData(hero));
        }
    }
}
