﻿using City.TrainCamp;
using Common.Resourses;
using Models.City.Mines;
using UnityEngine;

namespace City.Buildings.Mines
{
    [System.Serializable]
    public class MineData
    {
        public TypeMine type;
        public int currentCount = 0, maxCount = 2;
        public CostLevelUp ResourceOnLevelProduction, ResourceOnLevelUP;
        public int maxStore = 50;
        public TypeStore typeStore = TypeStore.Percent;
        public ListResource costCreate;
        public GameObject prefabMine;
        public Sprite image { get => prefabMine.GetComponent<SpriteRenderer>().sprite; }
        public void AddMine()
        {
            currentCount += 1;
        }
    }
}