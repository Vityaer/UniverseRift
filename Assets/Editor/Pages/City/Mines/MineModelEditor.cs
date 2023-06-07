using Editor.Common;
using Models.City.Mines;
using Sirenix.OdinInspector;

namespace Editor.Pages.City.Mines
{
    [HideReferenceObjectPicker]
    public class MineModelEditor : BaseModelEditor<MineModel>
    {
        public MineModelEditor(MineModel model)
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
