using Db.CommonDictionaries;
using Editor.Common;
using Models.City.AbstactBuildingModels;
using Models.City.Forges;
using Models.City.MagicCircles;
using Pages.Buildings.Forge;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Editor.Common.Pages.Buildings.Abstract;
using Models.City.Guilds;
using Utils;

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
