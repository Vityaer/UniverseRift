using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    [System.Serializable]
    public class LevelUp
    {
        public string Name;
        public int level;
        public CostIncreaseType typeIncrease = CostIncreaseType.Mulitiply;

        [SerializeField] private ListResource list;
        [SerializeField] private List<float> listIncrease;
        public ListResource List => list;
        public List<float> ListIncrease => listIncrease;
    }
}
