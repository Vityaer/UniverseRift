using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.City.Hires
{
    public class HireContainerModel : BaseModel
    {
        public ResourceData Cost;
        public List<HireModel> ChanceHires = new(); 
    }
}
