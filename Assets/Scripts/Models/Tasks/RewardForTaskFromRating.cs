using Common.Inventories.Resourses;
using Common.Resourses;
using UnityEngine;

namespace City.Buildings.TaskGiver
{
    [System.Serializable]
    public class RewardForTaskFromRating
    {
        public int rating;
        public GameResource res;
        public float delta = 0.25f;

        public GameResource GetReward()
        {
            return new GameResource(res.Type, res.Amount) * Random.Range(1 - delta, 1 + delta);
        }
    }
}