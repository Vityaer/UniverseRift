using Common;
using Common.Players;
using Db.CommonDictionaries;
using Initializable;
using LocalizationSystems;
using Misc.Json;
using Misc.Json.Impl;
using Models.Common;
using Services.TimeLocalizeServices;
using System;
using UIController.Inventory;
using VContainer;
using VContainer.Unity;

namespace Installer
{
    [Serializable]
    public class ProjectLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<JsonConverter>(Lifetime.Singleton).As<IJsonConverter>();
            builder.Register<CommonDictionaries>(Lifetime.Singleton);
            builder.Register<CommonGameData>(Lifetime.Singleton);
            builder.Register<GameInventory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LocalizationSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayersStorage>(Lifetime.Singleton).AsSelf();
            builder.Register<TimeLocalizeService>(Lifetime.Singleton).AsSelf();
            builder.RegisterEntryPoint<ProjectInitialize>();
            base.Configure(builder);
        }
    }
}