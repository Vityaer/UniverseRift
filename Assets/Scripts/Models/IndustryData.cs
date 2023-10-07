using City.Buildings.Mines;
using Models.City.Mines;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class IndustryData : BaseModel
    {
        public List<MineData> Mines = new List<MineData>();
    }
}
