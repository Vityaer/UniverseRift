using Editor.Common;
using Models.Dialogs;
using Sirenix.OdinInspector;

namespace Editor.Pages.Dialogs
{
    public class DialogModelEditor : BaseModelEditor<DialogModel>
    {
        public DialogModelEditor(DialogModel dialogModel)
        {
            m_model = dialogModel;
        }

        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelWidth(150)]
        [LabelText("Model")]
        public DialogModel Model
        {
            get => m_model;
            set => m_model = value;
        }
    }
}