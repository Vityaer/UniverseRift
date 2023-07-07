using Models;
using TMPro;
using UIController.Inventory;
using UnityEngine;
using VContainer;

namespace UIController.ItemVisual
{
    public class ForgeItemObjectCost : MonoBehaviour
    {
        public TextMeshProUGUI textAmount;

        private InventoryController _inventoryController;
        private GameItem _requireItem;
        private int _amountRequire;

        [Inject]
        public void Construct(InventoryController inventoryController)
        {
            _inventoryController = inventoryController;
        }

        public void SetInfo(GameItem item, int amount)
        {
            _requireItem = item;
            _amountRequire = amount;
            CheckItems();
        }

        public void CheckItems()
        {
            int currentCount = HowManyThisItems(_requireItem);
            string result = currentCount >= _amountRequire ? "<color=black>" : "<color=red>";

            result = $"{result}{currentCount}</color> / {_amountRequire}";
            textAmount.text = result;
        }


        private int HowManyThisItems(GameItem item)
        {
            var workItem = _inventoryController.GameInventory.InventoryObjects[item.Id];
            return workItem != null ? workItem.Amount : 0;
        }
    }
}