using City.TrainCamp;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CostLevelUp", menuName = "Custom ScriptableObject/CostLevelUp", order = 58)]
[System.Serializable]
public class CostLevelUp : ScriptableObject
{
    [Header("levels")]
    [SerializeField] private List<LevelUp> levelsCost = new List<LevelUp>();

    public List<LevelUp> GetLevels => levelsCost;

    public ListResource GetCostForLevelUp(int level)
    {
        bool find = false;
        int previousStage = 0;

        ListResource result = new ListResource();

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
                    case TypeIncrease.Mulitiply:
                        result.List[i] = result.List[i] * Mathf.Pow((1 + levelsCost[previousStage].ListIncrease[i] / 100f), level - levelsCost[previousStage].level);
                        break;
                    case TypeIncrease.Add:
                        Resource res = new Resource(result.List[i].Name, (levelsCost[previousStage].ListIncrease[i] * (level - levelsCost[previousStage].level)));
                        result.List[i].AddResource(res);
                        break;
                }
            }
        }
        return result;
    }
}


