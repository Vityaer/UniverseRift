using UIController.Observers;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace MainPages.MenuHud
{
    public class MenuHudView : UiView
    {
        public Button PlayerPanelButton;

        public ObserverResourceController GoldObserverResource;
        public ObserverResourceController DiamondObserverResource;
    }
}
