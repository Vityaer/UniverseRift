using Ui.LoadingScreen;
using UIController.GameSystems;
using VContainer;
using VContainer.Unity;
using VContainerUi;
using VContainerUi.Interfaces;
using VContainerUi.Model;

namespace Installers
{
    public class GameUiScope : IdleGameLifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //builder.RegisterEntryPoint<MainSwipeController>(Lifetime.Singleton);
            ConfigureWindows(builder);
            base.Configure(builder);
        }

        private void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<WindowState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterEntryPoint<WindowsController>()
                .WithParameter(UiScope.Local);
        }
    }
}
