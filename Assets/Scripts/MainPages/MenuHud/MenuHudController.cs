using VContainer;
using VContainer.Unity;
using VContainerUi.Abstraction;

namespace MainPages.MenuHud
{
    public class MenuHudController : UiController<MenuHudView>, IInitializable
    {
        [Inject] private readonly IObjectResolver _resolver;

        public void Initialize()
        {
            _resolver.Inject(View.GoldObserverResource);
            _resolver.Inject(View.DiamondObserverResource);
        }
    }
}
