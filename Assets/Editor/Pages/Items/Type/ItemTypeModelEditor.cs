using Editor.Common;
using Models.Items;
using Sirenix.OdinInspector;

namespace Editor.Pages.Items.Type
{
    [HideReferenceObjectPicker]
    public class ItemTypeModelEditor : BaseModelEditor<ItemType>
    {
        public ItemTypeModelEditor(ItemType model)
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
        [HorizontalGroup("1")]
        [LabelText("Name")]
        [LabelWidth(50)]
        public string Name
        {
            get => _model.Name;
            set => _model.Name = value;
        }
    }
}
