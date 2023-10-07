using City.Acievements;
using Editor.Common;
using Models.Achievments;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Editor.Pages.Achievments
{
    public class AchievmentModelEditor : BaseModelEditor<AchievmentModel>
    {
        public AchievmentModelEditor(AchievmentModel model)
        {
            _model = model;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Id")]
        [LabelWidth(200)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        [ShowInInspector]
        [HorizontalGroup("2")]
        [LabelText("Type")]
        [LabelWidth(200)]
        public AchievmentType Type
        {
            get => _model.Type;
            set => _model.Type = value;
        }

        [ShowInInspector]
        [HorizontalGroup("3")]
        [LabelText("Progress Type")]
        [LabelWidth(200)]
        public ProgressType ProgressType
        {
            get => _model.ProgressType;
            set => _model.ProgressType = value;
        }

        [ShowInInspector]
        [HorizontalGroup("4")]
        [LabelText("Stages")]
        [LabelWidth(200)]
        public List<AchievmentStageModel> Stages
        {
            get => _model.Stages;
            set => _model.Stages = value;
        }

    }
}
