using City.Buildings.Abstractions;
using City.TrainCamp;
using UIController.Buttons;
using UIController.Observers;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Tavern
{
    public class TavernView : BaseBuildingView
    {
        public Button SpecialHireButton;
        public Button SimpleHireButton;
        public Button FriendHireButton;

        public ButtonWithObserverResource CostOneHireController;
        public ButtonWithObserverResource CostManyHireController;

        public ResourceObjectCost ResourceObjectCostOneHire;
        public ResourceObjectCost ResourceObjectCostManyHire;

        [Header("Observers")]
        public ObserverResourceController ObserverSimpleHire;
        public ObserverResourceController ObserverSpecialHire;

    }
}
