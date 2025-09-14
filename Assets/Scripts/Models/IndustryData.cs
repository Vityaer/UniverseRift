using System;
using System.Collections.Generic;
using City.Buildings.Mines;
using City.Buildings.Mines.Panels.Travels;

namespace Models
{
    [Serializable]
    public class IndustryData : BaseModel
    {
        public List<MineData> Mines = new();
        public List<MineMissionData> MissionDatas = new();
        public MineMissionData BossMissionData = new();
        public int MineEnergy;
        public string DateTimeCreate;
    }
}