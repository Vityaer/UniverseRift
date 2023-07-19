using System.Collections.Generic;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace City.Buildings.Abstractions
{
    public class BaseBuildingView : UiView
    {
        public Button ButtonCloseBuilding;
        public List<UiView> AutoInjectObjects = new List<UiView>();
    }
}
