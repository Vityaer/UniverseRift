using Campaign;
using Campaign.GoldHeaps;
using MainPages.Campaign;
using MainPages.City.CityUi;
using MainPages.MenuHud;
using Ui.MainMenu.MenuButtons;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.MenuWindows
{
    public class CampaignWindow : WindowBase
    {
        public override string Name => nameof(CampaignWindow);

        public CampaignWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<MenuHudController>();
            AddController<CampaignPageController>();
            AddController<GoldHeapController>();
            AddController<CityUiController>();
            AddController<MenuButtonsController>();
        }
    }
}
