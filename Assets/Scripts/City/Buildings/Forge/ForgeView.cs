using City.Buildings.Abstractions;
using UIController.ItemVisual;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Forge
{
    public class ForgeView : BaseBuildingView
    {
        public RectTransform Content;
        public ForgeItemVisual LeftItem;
        public ForgeItemVisual RightItem;
        public Button WeaponPanelButton;
        public Button ArmorPanelButton;
        public Button BootsPanelButton;
        public Button AmuletPanelButton;
        public Button ButtonSynthesis;

        public ForgeItemVisual ForgeItemVisualPrefab;
    }
}
