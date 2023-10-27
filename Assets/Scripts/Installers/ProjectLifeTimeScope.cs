using Common;
using Db.CommonDictionaries;
using Initializable;
using Misc.Json;
using Misc.Json.Impl;
using Models.Common;
using UIController.Inventory;
using VContainer;
using VContainer.Unity;

namespace Installer
{
    public class ProjectLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<JsonConverter>(Lifetime.Singleton).As<IJsonConverter>();
            builder.Register<CommonDictionaries>(Lifetime.Singleton);
            builder.Register<CommonGameData>(Lifetime.Singleton);
            builder.Register<GameInventory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterEntryPoint<ProjectInitialize>();
            base.Configure(builder);
        }
    }
}