using City.Buildings.Forge;
using City.TrainCamp;
using Db.CommonDictionaries;
using UIController.Inventory;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.ItemVisual.Forges
{
    public class ItemForgeCost : UiView
    {
        [Inject] private CommonDictionaries _commonDictionaries;

        public ResourceObjectCost ResourceObjectCost;
        public SubjectCell SubjectCell;

        private GameItemRelation _relation;
        private GameItem _item;

        public void SetInfo(GameItemRelation relation)
        {
            _relation = relation;
            _item = new GameItem(_commonDictionaries.Items[relation.Model.ResultItemName], 0);
            SubjectCell.SetData(_item);
            SetCost();
        }

        private void SetCost()
        {
            ResourceObjectCost.SetData(_relation.Cost);
        }
    }
}