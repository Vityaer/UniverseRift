using System.Collections.Generic;
using System.Linq;
using Common.Db.CommonDictionaries;
using Editor.Common;
using Models.City.Alchemies;
using Sirenix.OdinInspector;

namespace Editor.Pages.Buildings.AlchemyPanels
{
    public class AlchemyPanelBuildingModelEditor : BaseModelEditor<AlchemyPanelBuildingModel>
    {
        private readonly CommonDictionaries m_dictionary;
        
        private string[] AllProducts => m_dictionary.Products.Select(c => c.Value).Select(r => r.Id).ToArray();
        
        public AlchemyPanelBuildingModelEditor(AlchemyPanelBuildingModel model, CommonDictionaries commonDictionaries)
        {
            m_dictionary = commonDictionaries;
            m_model = model;
        }
        
        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(250)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Sets")]
        [LabelWidth(250)]
        [ValueDropdown(nameof(AllProducts), IsUniqueList = true, DropdownWidth = 250, SortDropdownItems = true)]
        public List<string> Sets
        {
            get => m_model.Products;
            set => m_model.Products = value;
        }
    }
}