using Editor.Common;
using Models.Fights.Misc;
using Sirenix.OdinInspector;

namespace Editor.Pages.Locations
{
    [HideReferenceObjectPicker]
    public class LocationModelEditor : BaseModelEditor<LocationModel>
    {
        public LocationModelEditor(LocationModel model)
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
