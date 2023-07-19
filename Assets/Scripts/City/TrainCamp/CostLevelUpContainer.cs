using Common.Resourses;
using Models;
using Models.Common;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    public class CostLevelUpContainer : BaseModel
    {
        public List<CostLevelUpModel> LevelsCost = new List<CostLevelUpModel>();

        public CostLevelUpContainer()
        {
        }


        public List<GameResource> GetCostForLevelUp(int level)
        {
            var result = new List<GameResource>();

            var work = LevelsCost[0];

            for (int i = 0; i < LevelsCost.Count; i++)
            {
                if (LevelsCost[i].level == level)
                {
                    work = LevelsCost[i];
                    break;
                }
                if (LevelsCost[i].level < level)
                    work = LevelsCost[i];
            }

            GameResource res = null;
            for (int i = 0; i < work.Cost.Count; i++)
            {
                var data = work.Cost[i];
                res = new GameResource(data.Type, data.Amount);

                switch (work.typeIncrease)
                {
                    case CostIncreaseType.Mulitiply:
                        res = res * Mathf.Pow(1 + work.ListIncrease[i] / 100f, level - work.level);
                        break;
                    case CostIncreaseType.Add:
                        res.AddResource(work.ListIncrease[i]);
                        break;
                }
                result.Add(res);
            }

            return result;
        }
    }
}