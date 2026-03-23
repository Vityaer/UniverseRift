using Editor.Common;
using Models;
using Sirenix.OdinInspector;

namespace Editor.Pages.Rating
{
    [HideReferenceObjectPicker]
    public class RatingModelEditor : BaseModelEditor<RatingModel>
    {
        public RatingModelEditor(RatingModel model)
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
        [HorizontalGroup("1")]
        [LabelText("Name")]
        [LabelWidth(50)]
        public string Name
        {
            get => m_model.Name;
            set => m_model.Name = value;
        }
    }
}
