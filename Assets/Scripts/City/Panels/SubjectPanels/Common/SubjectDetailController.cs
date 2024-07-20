using City.Panels.SubjectPanels.Resources;
using Common;
using Common.Inventories.Splinters;
using Common.Resourses;
using UIController.Inventory;

namespace City.Panels.SubjectPanels.Common
{
    public class SubjectDetailController
    {
        private readonly ResourcePanelController _resourcePanelController;
        private readonly ItemPanelController _itemPanelController;
        private readonly SplinterPanelController _splinterPanelController;

        public SubjectDetailController(
            ItemPanelController itemPanelController,
            ResourcePanelController resourcePanelController,
            SplinterPanelController splinterPanelController
            )
        {
            _itemPanelController = itemPanelController;
            _resourcePanelController = resourcePanelController;
            _splinterPanelController = splinterPanelController;
        }

        public void ShowData(BaseObject baseObject)
        {
            switch (baseObject)
            {
                case GameResource gameResource:
                    _resourcePanelController.ShowData(gameResource);
                    break;
                case GameItem gameItem:
                    _itemPanelController.ShowData(gameItem);
                    break;
                case GameSplinter gameSplinter:
                    _splinterPanelController.ShowData(gameSplinter, false);
                    break;
            }
        }
    }
}
