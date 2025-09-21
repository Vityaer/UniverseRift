using Common.Resourses;
using System.Collections.Generic;
using Common.Inventories.Resourses;
using UnityEngine;
using VContainer;

namespace City.TrainCamp
{
    public class CostUIList : MonoBehaviour
    {
        public List<ResourceObjectCost> CostObject = new List<ResourceObjectCost>();

        public void InjectAll(IObjectResolver objectResolver)
        {
            foreach (var cost in CostObject)
            {
                objectResolver.Inject(cost);
            }
        }

        public void ShowCosts(List<GameResource> resources)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                CostObject[i].SetData(resources[i]);
            }

            for (int i = resources.Count; i < CostObject.Count; i++)
            {
                CostObject[i].Hide();
            }
        }
    }
}