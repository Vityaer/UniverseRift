using Editor.Common;
using Models;
using Sirenix.OdinInspector;

namespace Editor.Pages.Splinters
{
    public class SplinterModelEditor : BaseModelEditor<SplinterModel>
    {
        public SplinterModelEditor(SplinterModel model)
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
