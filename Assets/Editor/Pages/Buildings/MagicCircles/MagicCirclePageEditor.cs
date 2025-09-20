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
using Common.Db.CommonDictionaries;
using Utils;

namespace Editor.Pages.Buildings.MagicCircles
{
    public class MagicCirclePageEditor : BasePageEditor
    {
        private CommonDictionaries _dictionaries;
        private string NAME => nameof(MagicCircleBuildingModel);

        public MagicCirclePageEditor(CommonDictionaries commonDictionaries)
        {
            _dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            MagicCircleBuildingModel magicCircleBuildingModel;
            if (_dictionaries.Buildings.ContainsKey(NAME))
            {

                magicCircleBuildingModel = _dictionaries.Buildings[NAME] as MagicCircleBuildingModel;
            }
            else
            {
                magicCircleBuildingModel = new MagicCircleBuildingModel();
                magicCircleBuildingModel.Id = NAME;
                _dictionaries.Buildings.Add(NAME, magicCircleBuildingModel);
            }

            MagicCircle = magicCircleBuildingModel;
            MagicCircle.SetCommonDictionary(_dictionaries);
            DataExist = true;
        }

        public override void Save()
        {
            _dictionaries.Buildings[NAME] = MagicCircle;
            var buildings = _dictionaries.Buildings.Values.ToList();

            EditorUtils.Save(buildings);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("MagicCircle")]
        [PropertyOrder(2)]
        public MagicCircleBuildingModel MagicCircle;
    }
}
