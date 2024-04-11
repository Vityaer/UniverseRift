using City.TrainCamp.HeroInstances;
using ClientServices;
using Common.Authentications;
using Common.Heroes;
using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi;
using VContainerUi.Model;
using VContainerUi.Services.Impl;

namespace Installers
{
    public class GameUiScope : IdleGameLifetimeScope
    {
        [SerializeField] HeroInstancesController _heroInstancesController;

        protected override void Configure(IContainerBuilder builder)
        {
            ConfigureMessagePipe(builder);
            ConfigureWindows(builder);

            builder.Register<ClientRewardService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ResourceStorageController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<HeroesStorageController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.RegisterComponent(_heroInstancesController).AsSelf();
            base.Configure(builder);

            builder.Register<GameEntryPoint>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<WindowState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterEntryPoint<WindowsController>()
                .WithParameter(UiScope.Local);
        }

        private void ConfigureMessagePipe(IContainerBuilder builder)
        {
            builder.Register<UiMessagesReceiverService>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<UiMessagesPublisherService>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            var options = builder.RegisterMessagePipe();
            RegisterMessages(builder, options);
            builder.RegisterBuildCallback(c
                 => GlobalMessagePipe.SetProvider(c.AsServiceProvider()));
        }

        private void RegisterMessages(IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterUiSignals(options);
        }
    }
}
