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
            m_model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(50)]
        public string Id
        {
            get => m_model.Id;
            set => m_model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Stages")]
        [LabelWidth(300)]
        public List<CostLevelUpModel> LevelsCost
        {
            get => m_model.LevelsCost;
            set => m_model.LevelsCost = value;
        }
    }
}
