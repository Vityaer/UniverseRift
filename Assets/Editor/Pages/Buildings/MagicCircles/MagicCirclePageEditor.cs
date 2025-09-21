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
        private readonly CommonDictionaries m_dictionaries;
        private static string Name => nameof(MagicCircleBuildingModel);

        public MagicCirclePageEditor(CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;
            Init();
        }

        public override void Init()
        {
            base.Init();
            MagicCircleBuildingModel magicCircleBuildingModel;
            if (m_dictionaries.Buildings.ContainsKey(Name))
            {

                magicCircleBuildingModel = m_dictionaries.Buildings[Name] as MagicCircleBuildingModel;
            }
            else
            {
                magicCircleBuildingModel = new MagicCircleBuildingModel();
                magicCircleBuildingModel.Id = Name;
                m_dictionaries.Buildings.Add(Name, magicCircleBuildingModel);
            }

            MagicCircle = magicCircleBuildingModel;
            MagicCircle.SetCommonDictionary(m_dictionaries);
            DataExist = true;
        }

        public override void Save()
        {
            m_dictionaries.Buildings[Name] = MagicCircle;
            var buildings = m_dictionaries.Buildings.Values.ToList();

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
