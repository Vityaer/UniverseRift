using City.TrainCamp;
using Editor.Common;
using Models.Common;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Pages.Heroes.CostLevelUp
{

    [HideReferenceObjectPicker]
    public class CostLevelUpContainerModelEditor : BaseModelEditor<CostLevelUpContainer>
    {
        public CostLevelUpContainerModelEditor(CostLevelUpContainer model)
        {
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Stages")]
        [LabelWidth(300)]
        public List<CostLevelUpModel> LevelsCost
        {
            get => _model.LevelsCost;
            set => _model.LevelsCost = value;
        }
    }
}
