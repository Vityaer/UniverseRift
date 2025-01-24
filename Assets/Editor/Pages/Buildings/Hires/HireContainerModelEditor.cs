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
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(150)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }


        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Resource")]
        [LabelWidth(150)]
        public ResourceData Cost
        {
            get => _model.Cost;
            set => _model.Cost = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("ChanceHires")]
        [LabelWidth(150)]
        public List<HireModel> ChanceHires
        {
            get => _model.ChanceHires;
            set => _model.ChanceHires = value;
        }
    }
}
