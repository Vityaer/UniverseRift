using Assets.Scripts.ClientServices;
using City.Buildings.TaskGiver;
using City.Panels.Inventories;
using ClientServices;
using Common;
using Common.Heroes;
using Db.CommonDictionaries;
using Fight.AI;
using Fight.Factories;
using Fight.Grid;
using Initializable;
using MessagePipe;
using Misc.Json;
using Misc.Json.Impl;
using Models.Common;
using UIController.Inventory;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerUi;
using VContainerUi.Model;
using VContainerUi.Services.Impl;

namespace Installer
{
    public class ProjectLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            ConfigureMessagePipe(builder);
            builder.Register<JsonConverter>(Lifetime.Singleton).As<IJsonConverter>();
            builder.Register<CommonDictionaries>(Lifetime.Singleton);
            builder.Register<CommonGameData>(Lifetime.Singleton);
            builder.Register<CommonGameData>(Lifetime.Singleton);
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<GameTaskProvider>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();


            builder.RegisterEntryPoint<ProjectInitialize>();

            builder.Register<WindowState>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterEntryPoint<WindowsController>()
                .WithParameter(UiScope.Project);
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