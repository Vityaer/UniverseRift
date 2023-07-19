using City.Buildings.Forge;
using Db.CommonDictionaries;
using UIController.Inventory;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual.Forges
{
    public class ItemForgeIngredients : UiView
    {
        [Inject] private InventoryController _inventoryController;
        [Inject] private CommonDictionaries _commonDictionaries;

        public ItemSliderController ItemSliderController;
        public SubjectCell SubjectCell;

        private GameItemRelation _relation;
        private GameItem _item;

        public bool IsEnough =>
            (HowManyThisItems(_item) >= _relation.Model.RequireCount); 

        public void SetInfo(GameItemRelation relation)
        {
            _relation = relation;
            _item = new GameItem(_commonDictionaries.Items[relation.Model.ItemIngredientName], 0);
            SubjectCell.SetData(_item);
            CheckItems();
        }

        public void CheckItems()
        {
            int currentCount = HowManyThisItems(_item);
            ItemSliderController.SetAmount(currentCount, _relation.Model.RequireCount);
            //string result = currentCount >= _relation.Model.RequireCount ? "<color=black>" : "<color=red>";

            //result = $"{result}{currentCount}</color> / {_relation.Model.RequireCount}";
            //textAmount.text = result;
        }


        private int HowManyThisItems(GameItem item)
        {
            if (_inventoryController.GameInventory.InventoryObjects.TryGetValue(item.Id, out var value))
            {
                return value.Amount;
            }
            else
            {
                return 0;
            }
        }
    }
}
