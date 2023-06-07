using City.TrainCamp;
using Common.Resourses;
using System;
using UnityEngine;

namespace UIController.Buttons
{
    public class ButtonWithObserverResource : MonoBehaviour
    {
        public ResourceObjectCost resourceObserver;
        public ButtonCostController buttonCostComponent;
        public Resource cost;

        public void ChangeCost(Resource cost)
        {
            resourceObserver.SetData(cost);
        }

        public void ChangeCost(Action<int> d)
        {
            resourceObserver.SetData(cost);
            buttonCostComponent.UpdateCostWithoutInfo(cost, d);
        }

        public void ChangeCost(Resource cost, Action<int> d)
        {
            resourceObserver.SetData(cost);
            buttonCostComponent.UpdateCostWithoutInfo(cost, d);
        }
    }
}