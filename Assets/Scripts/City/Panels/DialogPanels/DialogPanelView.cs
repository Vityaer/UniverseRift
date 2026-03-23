using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using VContainerUi.Abstraction;

namespace City.Panels.DialogPanels
{
    public class DialogPanelView : UiView
    {
        public LocalizeStringEvent NPCName;
        public TMP_Text Sentence;
        public Image Avatar;
        public Button NextSentenceButton;

        public float PrintSymbolAnimationTime;
    }
}