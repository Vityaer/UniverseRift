using System;
using System.Collections.Generic;

namespace UIController.Rewards.PosibleRewards
{
    [Serializable]
    public class PosibleReward
    {
        public List<PosibleGameObject> PosibilityObjectRewards = new List<PosibleGameObject>();
        private float _sumAll = 0;

        public float GetAllSum
        {
            get
            {
                if (_sumAll > 0) { return _sumAll; }
                else
                {
                    for (int i = 0; i < PosibilityObjectRewards.Count; i++) _sumAll += PosibilityObjectRewards[i].Posibility;
                    return _sumAll;
                }

            }
        }

        public float PosibleNumObject(int num)
        {
            if (_sumAll <= 0f) _sumAll = GetAllSum;
            return PosibilityObjectRewards[num].Posibility / _sumAll * 100f;
        }

        public RewardData GetReward(int tryCount)
        {
            var result = new RewardData();
            foreach (var posibleItem in PosibilityObjectRewards)
            {
                switch (posibleItem)
                {
                    case PosibleGameResource posibleGameResource:
                        //result.Add(posibleGameResource.GetResource(tryCount));
                        break;
                }
            }
            return result;
        }
    }
}