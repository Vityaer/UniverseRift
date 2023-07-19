using Models.Data.Common;
using System;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class BuildingWithFightTeamsData : SimpleBuildingData
    {
        public RecordsData<TeamFightData> TeamRecords = new RecordsData<TeamFightData>();
    }
}
