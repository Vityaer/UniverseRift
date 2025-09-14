using City.Buildings.Abstractions;
using System.Collections.Generic;
using UIController.ItemVisual.Forges;
using UIController.Misc.CustomComponents;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.Forge
{
    public class ForgeView : BaseBuildingView
    {
        public RectTransform Content;

        public ItemForgeIngredients LeftItem;
        public ItemForgeCost RightItem;

        public Button WeaponPanelButton;
        public Button ArmorPanelButton;
        public Button BootsPanelButton;
        public Button AmuletPanelButton;
        public Button ButtonSynthesis;

        public ForgeItemVisual ForgeItemVisualPrefab;
        
        public GridOverrider GridOverrider;

    }
}
