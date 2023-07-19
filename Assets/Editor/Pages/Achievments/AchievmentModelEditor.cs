using Editor.Common;
using Models.Achievments;
using Sirenix.OdinInspector;

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
        [LabelWidth(50)]
        public string Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

    }
}
