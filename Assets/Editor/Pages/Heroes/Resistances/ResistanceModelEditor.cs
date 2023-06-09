using Editor.Common;
using Models.Heroes;
using Sirenix.OdinInspector;

namespace Editor.Pages.Heroes.Resistances
{
    [HideReferenceObjectPicker]
    public class ResistanceModelEditor : BaseModelEditor<ResistanceModel>
    {
        public ResistanceModelEditor(ResistanceModel model)
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
