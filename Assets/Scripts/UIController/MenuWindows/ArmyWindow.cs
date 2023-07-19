using Ui.MainMenu.MenuButtons;
using VContainer;
using VContainerUi.Abstraction;

namespace UIController.MenuWindows
{
    public class ArmyWindow : WindowBase
    {
        public override string Name => nameof(ArmyWindow);

        public ArmyWindow(IObjectResolver container) : base(container)
        {
        }

        protected override void AddControllers()
        {
            AddController<PageArmyController>();
            AddController<MenuButtonsController>();
        }
    }
}
