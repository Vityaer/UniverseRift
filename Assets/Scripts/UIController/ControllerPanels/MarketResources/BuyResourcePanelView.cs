using City.TrainCamp;
using Models.City.Markets;
using System.Collections.Generic;
using Ui.Misc.Widgets;
using UIController.Buttons;
using UIController.ControllerPanels.CountControllers;
using UIController.ItemVisual;
using UnityEngine;

namespace UIController.ControllerPanels.MarketResources
{
    public class BuyResourcePanelView : BasePanel
    {
        [field: SerializeField] public SubjectCell MainImage { get; private set; }
        [field: SerializeField] public CountPanelController CountController { get; private set; }
        [field: SerializeField] public ButtonWithObserverResource BuyButton { get; private set; }

        public ResourceObjectCost ResourceObjectCost;

    }
}
