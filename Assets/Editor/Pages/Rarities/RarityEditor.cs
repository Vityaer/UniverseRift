using Editor.Common;
using Models;
using Sirenix.OdinInspector;

namespace Editor.Units
{
    [HideReferenceObjectPicker]
    public class RarityEditor : BaseModelEditor<RarityModel>
    {
        public RarityEditor(RarityModel model)
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