using Models.City.TrainCamp;
using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.Heroes.PowerUps
{
    public class RatingUpContainer : BaseModel
    {
        public List<ResourceData> Cost = new();
        public List<RequirementHeroModel> RequirementHeroes = new();
    }
}
