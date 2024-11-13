using MainPages.City;
using MainPages.MenuHud;
using Ui.MainMenu.MenuButtons;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.MenuWindows
{
    public class CityWindow : WindowBase
    {
        public override string Name => nameof(CityWindow);

        public CityWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<MenuHudController>();
            AddController<CityPageController>();
            AddController<CityUiController>();
            AddController<MenuButtonsController>();
        }
    }
}
