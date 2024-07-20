using City.Buildings.Abstractions;
using City.TrainCamp;
using System.Collections.Generic;
using UIController.Buttons;
using UIController.GameObservers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace City.Buildings.MagicCircle
{
    public class MagicCircleView : BaseBuildingView
    {
        public Dictionary<string, Button> RaceSelectButtons = new();
        public ButtonWithObserverResource OneHire;
        public ButtonWithObserverResource ManyHire;

        public ResourceObjectCost ResourceObjectCostOneHire;
        public ResourceObjectCost ResourceObjectCostManyHire;

        [Header("Observers")]
        public ObserverResourceController ObserverRaceHireCard;
    }
}
