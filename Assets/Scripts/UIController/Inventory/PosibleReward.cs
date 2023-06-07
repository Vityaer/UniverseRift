using System;
using System.Collections.Generic;

namespace UIController.Inventory
{
    [Serializable]
    public class PosibleReward
    {
        public List<SplinerPosibilityObject> posibilityObjectRewards = new List<SplinerPosibilityObject>();
        float sumAll = 0;
        public float GetAllSum
        {
            get
            {
                if (sumAll > 0) { return sumAll; }
                else
                {
                    for (int i = 0; i < posibilityObjectRewards.Count; i++) sumAll += posibilityObjectRewards[i].posibility;
                    return sumAll;
                }

            }
        }
        public float PosibleNumObject(int num)
        {
            if (sumAll <= 0f) sumAll = GetAllSum;
            return posibilityObjectRewards[num].posibility / sumAll * 100f;
        }

        public void Add(string ID, float percent = 1f)
        {
            posibilityObjectRewards.Add(new SplinerPosibilityObject(ID, percent));
        }
    }
}