using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.City.Alchemies;
using Sirenix.OdinInspector;
using Utils;

namespace Editor.Pages.Buildings.AlchemyPanels
{
    public class AlchemyPanelPageEditor : BasePageEditor
    {
        private CommonDictionaries m_dictionaries;

        public AlchemyPanelPageEditor(CommonDictionaries commonDictionaries)
        {
            m_dictionaries = commonDictionaries;
            Init();
        }
        
        public override void Init()
        {
            base.Init();
            AlchemyPanelBuildingModel model;
            if (m_dictionaries.Buildings.TryGetValue(nameof(AlchemyPanelBuildingModel), out var building))
            {
                model = building as AlchemyPanelBuildingModel;
            }
            else
            {
                model = new AlchemyPanelBuildingModel(m_dictionaries);
                m_dictionaries.Buildings.Add(nameof(AlchemyPanelBuildingModel), model);
            }

            AlchemyPanelBuildingModelEditor = new AlchemyPanelBuildingModelEditor(model, m_dictionaries);
            DataExist = true;
        }
        
        public override void Save()
        {
            m_dictionaries.Buildings[nameof(AlchemyPanelBuildingModel)] = AlchemyPanelBuildingModelEditor.GetModel();
            var buildings = m_dictionaries.Buildings.ToList();
            EditorUtils.Save(buildings);
            base.Save();
        }

        [ShowInInspector]
        [ShowIf(nameof(DataExist))]
        [HorizontalGroup("3")]
        [LabelText("Alchemy Panel")]
        [PropertyOrder(2)]
        public AlchemyPanelBuildingModelEditor AlchemyPanelBuildingModelEditor;

    }
}