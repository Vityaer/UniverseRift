using MainPages.Events;
using MainPages.MenuHud;
using Ui.MainMenu.MenuButtons;
using VContainer;
using VContainerUi.Abstraction;

namespace Assets.Scripts.UIController.MenuWindows
{
    public class EventWindow : WindowBase
    {
        public override string Name => nameof(EventWindow);

        public EventWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<EventsPageController>();
            AddController<MenuButtonsController>();
        }
    }
}
