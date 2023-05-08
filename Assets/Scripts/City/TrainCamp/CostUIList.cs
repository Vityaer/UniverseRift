using System.Collections.Generic;
using UnityEngine;

public class CostUIList : MonoBehaviour
{
    public List<ResourceObjectCost> costObject = new List<ResourceObjectCost>();

    public void ShowCosts(ListResource resourcesCost)
    {
        for (int i = 0; i < resourcesCost.List.Count; i++)
        {
            costObject[i].SetData(resourcesCost.List[i]);
        }
        for (int i = resourcesCost.List.Count; i < costObject.Count; i++)
        {
            costObject[i].Hide();
        }
    }
}
