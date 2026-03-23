using Editor.Common;
using Models.City.Hires;
using Models.Data.Inventories;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Editor.Pages.Buildings.Hires
{
    public class HireContainerModelEditor : BaseModelEditor<HireContainerModel>
    {
        public HireContainerModelEditor(HireContainerModel model)
        {
            m_model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }


        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Resource")]
        [LabelWidth(150)]
        public ResourceData Cost
        {
            get => m_model.Cost;
            set => m_model.Cost = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("ChanceHires")]
        [LabelWidth(150)]
        public List<HireModel> ChanceHires
        {
            get => m_model.ChanceHires;
            set => m_model.ChanceHires = value;
        }
    }
}
