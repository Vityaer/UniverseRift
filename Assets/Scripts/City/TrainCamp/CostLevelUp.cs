using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    [CreateAssetMenu(fileName = "CostLevelUp", menuName = "Custom ScriptableObject/CostLevelUp", order = 58)]
    [System.Serializable]
    public class CostLevelUp : ScriptableObject
    {
        [Header("levels")]
        [SerializeField] private List<LevelUp> levelsCost = new List<LevelUp>();

        public List<LevelUp> GetLevels => levelsCost;

        public ListResource GetCostForLevelUp(int level)
        {
            var find = false;
            var previousStage = 0;

            var result = new ListResource();

            for (int i = 0; i < levelsCost.Count; i++)
            {
                if (levelsCost[i].level == level)
                {
                    result = levelsCost[i].List;
                    find = true;
                    break;
                }
                if (levelsCost[i].level < level)
                    previousStage = i;
            }

            if (find == false)
            {
                result = (ListResource)levelsCost[previousStage].List.Clone();
                for (int i = 0; i < result.List.Count; i++)
                {
                    switch (levelsCost[previousStage].typeIncrease)
                    {
                        case CostIncreaseType.Mulitiply:
                            result.List[i] = result.List[i] * Mathf.Pow(1 + levelsCost[previousStage].ListIncrease[i] / 100f, level - levelsCost[previousStage].level);
                            break;
                        case CostIncreaseType.Add:
                            Resource res = new Resource(result.List[i].Name, levelsCost[previousStage].ListIncrease[i] * (level - levelsCost[previousStage].level));
                            result.List[i].AddResource(res);
                            break;
                    }
                }
            }
            return result;
        }
    }
}