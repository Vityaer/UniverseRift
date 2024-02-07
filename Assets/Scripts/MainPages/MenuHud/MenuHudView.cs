using UIController.GameObservers;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace MainPages.MenuHud
{
    public class MenuHudView : UiView
    {
        public Button PlayerPanelButton;
        public Image Avatar;

        public ObserverResourceController GoldObserverResource;
        public ObserverResourceController DiamondObserverResource;
    }
}
