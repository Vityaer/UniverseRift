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
        [Header("Requirements")]
        [SerializeField] private ListResource list;
        public List<RequirementHeroModel> requirementHeroes = new List<RequirementHeroModel>();
        public ListResource Cost => list;


        public void UpdateData(HeroModel hero)
        {
            requirementHeroes.ForEach(x => x.UpdateData(hero));
        }
    }
}
