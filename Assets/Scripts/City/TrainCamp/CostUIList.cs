using Common.Resourses;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    public class CostUIList : MonoBehaviour
    {
        public List<ResourceObjectCost> CostObject = new List<ResourceObjectCost>();

        public void ShowCosts(ListResource resourcesCost)
        {
            for (int i = 0; i < resourcesCost.List.Count; i++)
            {
                CostObject[i].SetData(resourcesCost.List[i]);
            }

            for (int i = resourcesCost.List.Count; i < CostObject.Count; i++)
            {
                CostObject[i].Hide();
            }
        }
    }
}