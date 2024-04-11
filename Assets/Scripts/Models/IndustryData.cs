using City.Buildings.Mines;
using City.Buildings.Mines.Panels.Travels;
using Fight.Common;
using Models.City.Mines;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class IndustryData : BaseModel
    {
        public List<MineData> Mines = new();
        public List<MineMissionData> MissionDatas = new();
    }
}
