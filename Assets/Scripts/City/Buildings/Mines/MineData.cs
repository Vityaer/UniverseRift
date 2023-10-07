using City.TrainCamp;
using Common.Resourses;
using Models.City.Mines;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Mines
{
    [System.Serializable]
    public class MineData
    {
        public int Id;
        public string MineId;
        public string PlaceId;
        public int PlayerId;
        public int Level;
        public string LastDateTimeGetIncome;
    }
}