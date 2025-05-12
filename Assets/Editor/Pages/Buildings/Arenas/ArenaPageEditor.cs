using Db.CommonDictionaries;
using Editor.Common.Pages.Buildings.Abstract;
using Models.City.Arena;

namespace Editor.Common.Pages.Buildings.Arenas
{
    public class ArenaPageEditor : AbstractBuildingPageEditor<ArenaBuildingModel>
    {
        protected override string Name => nameof(ArenaBuildingModel);
        
        public ArenaPageEditor(CommonDictionaries commonDictionaries) : base(commonDictionaries)
        {
        }
    }
}