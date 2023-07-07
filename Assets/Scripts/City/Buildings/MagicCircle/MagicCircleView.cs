using City.Buildings.Abstractions;
using City.TrainCamp;
using UIController.Buttons;
using UnityEngine;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleView : BaseBuildingView
    {
        public ButtonWithObserverResource OneHire;
        public ButtonWithObserverResource ManyHire;

        public ResourceObjectCost ResourceObjectCostOneHire;
        public ResourceObjectCost ResourceObjectCostManyHire;
    }
}
