using City.Buildings.Forge;
using City.Panels.Inventories;
using Common.Db.CommonDictionaries;
using UIController.Inventory;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual.Forges
{
    public class ItemForgeIngredients : UiView
    {
        [Inject] private InventoryPanelController m_inventoryPanelController;
        [Inject] private CommonDictionaries m_commonDictionaries;

        public ItemSliderController ItemSliderController;
        public SubjectCell SubjectCell;

        private GameItemRelation m_relation;
        private GameItem m_item;

        public bool IsEnough =>
            (HowManyThisItems(m_item) >= m_relation.Model.RequireCount); 

        public void SetInfo(GameItemRelation relation)
        {
            m_relation = relation;
            m_item = new GameItem(m_commonDictionaries.Items[relation.Model.ItemIngredientName], 0);
            SubjectCell.Init(null, m_commonDictionaries);
            SubjectCell.SetData(m_item);
            CheckItems();
        }

        public void CheckItems()
        {
            int currentCount = HowManyThisItems(m_item);
            ItemSliderController.SetAmount(currentCount, m_relation.Model.RequireCount);
            //string result = currentCount >= _relation.Model.RequireCount ? "<color=black>" : "<color=red>";

            //result = $"{result}{currentCount}</color> / {_relation.Model.RequireCount}";
            //textAmount.text = result;
        }


        private int HowManyThisItems(GameItem item)
        {
            return m_inventoryPanelController.GameInventory.InventoryObjects.TryGetValue(item.Id, out var value)
                ? value.Amount
                : 0;
        }
    }
}
