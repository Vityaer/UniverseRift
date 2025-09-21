using System.Collections.Generic;
using City.Buildings.Abstractions;
using UIController.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.ControllerPanels.AlchemyPanels
{
    public class AlchemyPanelView : BaseBuildingView
    {
        [field: SerializeField] public Button AlchemyButton { get; private set; }
        public SliderTime SliderTimeAlchemy;
        public RewardUIController RewardView;
        
        public List<ButtonWithObserverResource> ProductButtons;
        public List<RewardUIController> ProductsRewards;
    }
}
