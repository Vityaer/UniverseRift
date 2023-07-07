using City.Panels.SubjectPanels;
using City.Panels.SubjectPanels.Resources;
using Ui.Misc.Widgets;
using UIController.Inventory;
using UIController.ItemVisual;
using UnityEngine;

namespace City.Panels.Inventories
{
    public class InventoryView : BasePanel
    {
        public SubjectCell CellPrefab;
        public Transform GridParent;
        public GameObject panelInventory;
        public GameObject panelController;
    }
}
