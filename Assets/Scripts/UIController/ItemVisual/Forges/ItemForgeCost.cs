using City.Buildings.Forge;
using City.TrainCamp;
using Common.Db.CommonDictionaries;
using UIController.Inventory;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual.Forges
{
    public class ItemForgeCost : UiView
    {
        [Inject] private CommonDictionaries m_commonDictionaries;

        public ResourceObjectCost ResourceObjectCost;
        public SubjectCell SubjectCell;

        private GameItemRelation m_relation;
        private GameItem m_item;

        public void SetInfo(GameItemRelation relation)
        {
            m_relation = relation;
            m_item = new GameItem(m_commonDictionaries.Items[relation.Model.ResultItemName], 0);
            SubjectCell.SetData(m_item);
            SetCost();
        }

        private void SetCost()
        {
            ResourceObjectCost.SetData(m_relation.Cost);
        }
    }
}