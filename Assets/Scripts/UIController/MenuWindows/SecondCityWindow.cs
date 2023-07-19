using MainPages.City;
using MainPages.MenuHud;
using MainPages.SecondCity;
using Ui.MainMenu.MenuButtons;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.MenuWindows
{
    public class SecondCityWindow : WindowBase
    {
        public override string Name => nameof(SecondCityWindow);

        public SecondCityWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<MenuHudController>();
            AddController<SecondCityPageController>();
            AddController<CityUiController>();
            AddController<MenuButtonsController>();

        }
    }
}
