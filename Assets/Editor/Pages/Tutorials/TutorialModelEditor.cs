using Editor.Common;
using Models.Tutorials;
using Sirenix.OdinInspector;

namespace Editor.Pages.Tutorials
{
    public class TutorialModelEditor : BaseModelEditor<TutorialModel>
    {
        public TutorialModelEditor(TutorialModel tutorialModel)
        {
            m_model = tutorialModel;
        }
        
        [ShowInInspector]
        [HorizontalGroup("1")]
        [LabelText("Model")]
        [LabelWidth(150)]
        public TutorialModel Model
        {
            get => m_model;
            set => m_model = value;
        }
    }
}