using Db.CommonDictionaries;
using Editor.Common.Pages.Buildings.Abstract;
using Models.City.Sanctuaries;

namespace Editor.Common.Pages.Buildings.Sanctuaries
{
    public class SanctuaryPageEditor : AbstractBuildingPageEditor<SanctuaryBuildingModel>
    {
        protected override string Name => nameof(SanctuaryBuildingModel);
        
        public SanctuaryPageEditor(CommonDictionaries commonDictionaries) : base(commonDictionaries)
        {
        }
    }
}