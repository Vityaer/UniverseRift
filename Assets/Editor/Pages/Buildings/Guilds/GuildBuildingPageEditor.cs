using Db.CommonDictionaries;
using Editor.Common.Pages.Buildings.Abstract;
using Models.City.Guilds;

namespace Editor.Common.Pages.Buildings.Guilds
{
    public class GuildBuildingPageEditor : AbstractBuildingPageEditor<GuildBuildingModel>
    {
        protected override string Name => nameof(GuildBuildingModel);

        public GuildBuildingPageEditor(CommonDictionaries commonDictionaries) : base(commonDictionaries)
        {
        }
    }
}