using Assets.Scripts.ClientServices;
using ClientServices;
using Common.Heroes;
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

            builder.Register<ClientRewardService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ResourceStorageController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<HeroesStorageController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

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
