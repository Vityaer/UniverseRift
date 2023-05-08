using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace City.TrainCamp
{

    [System.Serializable]
    public class LevelUp
    {
        public string Name;
        public int level;
        public TypeIncrease typeIncrease = TypeIncrease.Mulitiply;

        [SerializeField] private ListResource list;
        [SerializeField] private List<float> listIncrease;
        public ListResource List => list;
        public List<float> ListIncrease => listIncrease;
    }
}
