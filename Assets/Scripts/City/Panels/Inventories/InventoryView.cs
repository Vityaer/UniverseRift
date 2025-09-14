using System.Collections.Generic;
using Common.Resourses;
using Ui.Misc.Widgets;
using UIController.ItemVisual;
using UIController.Misc.CustomComponents;
using UnityEngine;
using UnityEngine.UI;

namespace City.Panels.Inventories
{
    public class InventoryView : BasePanel
    {
        public SubjectCell CellPrefab;
        public Transform GridParent;
        public GameObject ControllerPanel;
        public GridOverrider GridOverrider;
        public Dictionary<InventoryPageType, InventoryButton> PagesButton;
        public ScrollRect ScrollRect;
        public List<ResourceType> BannedResourceTypes;
    }
}
